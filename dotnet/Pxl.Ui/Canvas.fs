namespace Pxl

// --------------------------------------------------------------------------
// All types and signatures have to be mirrored
// in the dotnet / node implementations.
//
// see:
//   src/Pxl.BufferedHttpCanvas/BufferedHttpCanvas.fs
//   src/Pxl.Ui/Canvas.fs
//   src/pxl-local-display/src/domain.ts
//   src/pxl-local-display/src/server.ts
// --------------------------------------------------------------------------

open System

type CanvasMetadata =
    {
        width: int
        height: int
        fps: int
    }

[<RequireQualifiedAccess>]
type CanvasMode = On | Off

type Canvas
    (
        metadata: CanvasMetadata,
        sendFrameBufferSize: int,
        onEnd: unit -> unit
    )
    =
    let bbq = new BoundedBlockingQueue<_>(sendFrameBufferSize, onEnd)

    let onFrameReceivedEvent = new Event<Color[]>()

    let mutable _activeChannel = 0

    do
        bbq.Consume(
            onFrameReceivedEvent.Trigger,
            // TODO: What now?
            (fun () -> ())
        )

    [<CLIEvent>]
    member _.OnFrameReceived = onFrameReceivedEvent.Publish

    member _.ActiveChannel
        with get() = _activeChannel
        and set(v) = _activeChannel <- v
    member _.Metadata = metadata
    member _.SendBufferSize = sendFrameBufferSize
    member _.PushFrameSafe(channel, pixels: Color[]) =
        if _activeChannel = channel then
            bbq.Push(pixels)
    member _.Ct = bbq.Cts.Token

    interface IDisposable with
        member _.Dispose() =
            (bbq :> IDisposable).Dispose()


[<RequireQualifiedAccess>]
module CanvasProxy =

    open System.Net.Sockets
    open System.Threading.Tasks
    open FsHttp
    open Pxl

    // --------------------------------------------------------------------------
    // All types and signatures have to be mirrored
    // in the dotnet / node implementations.
    //
    // see:
    //   src/Pxl.BufferedHttpCanvas/BufferedHttpCanvas.fs
    //   src/Pxl.Ui/Canvas.fs
    //   src/pxl-local-display/src/domain.ts
    //   src/pxl-local-display/src/server.ts
    // --------------------------------------------------------------------------

    let bpp = 3

    let defaultSendBufferSize = 20
    let defaultMetadataRoute = "metadata"

    let invariantServicePorts =
        {|
            httpMetadata = 5001
            tcpFrames = 5002
            tcpFramesForDeviceInDevMode = 5004
        |}

    // TODO:
    // - FsHttp port exhaustion issue
    // - use a streaming model instead of copying
    // - and then use FsHttp to stream the data

    // Info: This really has to play nice with the logic in Runner regarding exception handling and retries

    let frameBytesToColors (bytes: byte[]) =
        let mutable i = 0
        [|
            while i < bytes.Length do
                yield Color.rgb(bytes[i], bytes[i + 1], bytes[i + 2])
                i <- i + bpp
        |]

    let frameColorsToBytes (colors: Color[]) =
        [|
            for c in colors do
                byte c.r
                byte c.g
                byte c.b
        |]

    let create
        remote
        useHttps
        httpMetadataPort
        httpMetadataRoute
        tcpFramesPort
        secondaryRemotes
        sendBufferSize
        onEnd
        =
        let clientMetadata =
            let protocol = if useHttps then "https" else "http"
            http {
                GET $"{protocol}://{remote}:{httpMetadataPort}/{httpMetadataRoute}"
                config_timeoutInSeconds 3.0
            }
            |> Request.send
            |> Response.assert2xx
            // |> Response.deserializeJson<CanvasMetadata>
            |> Response.toJson
            |> fun json ->
                // Why? Np reflection / AOT compat.
                let names : CanvasMetadata = { width = 0; height = 0; fps = 0 }
                let res =
                    {
                        CanvasMetadata.width = json.GetProperty(nameof names.width).GetInt32()
                        height = json.GetProperty(nameof names.height).GetInt32()
                        fps = json.GetProperty(nameof names.fps).GetInt32()
                    }
                res

        let createTcpClientAndStream (remote: string, tcpFramesPort: int) =
            let client = new TcpClient(remote, tcpFramesPort)
            let stream = client.GetStream()
            client, stream

        let primaryClient, primaryStream =
            createTcpClientAndStream (remote, tcpFramesPort)

        let secondaryClientsAndStreams =
            secondaryRemotes |> List.map createTcpClientAndStream

        let sendFrame (pixels: Color array) =
            let bytes = frameColorsToBytes pixels

            // this shall fail and break through to the canvas / onEnd / restart logic
            do primaryStream.Write(bytes, 0, bytes.Length)

            for _, stream in secondaryClientsAndStreams do
                // TODO: This is not optimal - but ok since we know that the whole thing
                // is only used during developing apps (currently).
                Task.Run(fun () ->
                    try stream.Write(bytes, 0, bytes.Length)
                    with ex -> printfn $"Could not send frame to secondary display: {ex.Message}"
                ) |> ignore

        let dispose () =
            primaryStream.Dispose()
            primaryClient.Dispose()
            for client, stream in secondaryClientsAndStreams do
                stream.Dispose()
                client.Dispose()
            onEnd ()

        let canvas =
            let metadata =
                {
                    width = clientMetadata.width
                    height = clientMetadata.height
                    fps = clientMetadata.fps
                }
            new Canvas(
                metadata, 
                sendBufferSize |> Option.defaultValue defaultSendBufferSize, 
                dispose
                )
        do canvas.OnFrameReceived.Add(sendFrame)
        canvas

