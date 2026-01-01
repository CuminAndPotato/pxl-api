[<RequireQualifiedAccess>]
module Pxl.Ui.Simulator

open System
open Pxl

// this mechanism prevents multiple running evaluators when loading the assembly multiple times
let private stopInternal =
    let stopFuncId = "__Pxl.Draw.Fsi.Eval.stop"
    let ensureStop () =
        match AppDomain.CurrentDomain.GetData(stopFuncId) with
        | null -> ()
        | stopFunc -> (stopFunc :?> (unit -> unit)) ()
    let setStopFunc (stopFunc: unit -> unit) =
        ensureStop ()
        AppDomain.CurrentDomain.SetData(stopFuncId, box stopFunc)
    let invoke () =
        setStopFunc (fun () -> ())
    {|
        setFunc = setStopFunc
        invoke = invoke
    |}

let start (createCanvas: (unit -> unit) -> Canvas) scene =
    if ApiEnv.isInInteractiveContext then
        let mutable retry = true

        let rec reStart () =
            retry <- true
            
            let onCanvasEnd () =
                if retry then
                    let sleepTimeInMs = 2000
                    printfn $"Error in flushing Canvas. Waiting {sleepTimeInMs}ms before retrying ..."
                    System.Threading.Thread.Sleep(sleepTimeInMs)
                    reStart ()

            try
                let canvas = createCanvas onCanvasEnd
                // printfn $"Canvas created - Metadata: {canvas.Metadata}"
                let stop =
                    Evaluation.startVide(
                        canvas,
                        RenderCtx(canvas.Metadata.width, canvas.Metadata.height, canvas.Metadata.fps),
                        (fun ex ->
                            printfn $"Error in evaluating App logic: {ex.Message}"
                            onCanvasEnd ()
                        ),
                        Reality.forRealTime (),
                        (fun () -> { lowerButtonPressed = false; upperButtonPressed = false }),
                        scene,
                        ignore
                        )
                stopInternal.setFunc (
                    fun () ->
                        retry <- false
                        stop ()
                        (canvas :> IDisposable).Dispose())
            with ex ->
                printfn $"Error in creating Canvas (ending it): {ex.Message}"
                onCanvasEnd ()

        stopInternal.invoke()
        reStart ()
    else
        ()

let startActionSimple primaryDeviceAddress secondaryDeviceAddresses (sceneFunc: System.Action) =
    let getTcpFramesPort deviceAddress =
        if
            not (String.IsNullOrWhiteSpace deviceAddress) 
            && deviceAddress.Trim() <> "127.0.0.1"
            && deviceAddress.Trim() <> "localhost" 
        then
            CanvasProxy.invariantServicePorts.tcpFramesForDeviceInDevMode
        else
            CanvasProxy.invariantServicePorts.tcpFrames
    let secondaryDeviceAddresses =
        secondaryDeviceAddresses
        |> Seq.toList
        |> List.map (fun addr -> addr, getTcpFramesPort addr)
    let createCanvas =
        CanvasProxy.create
            primaryDeviceAddress
            false
            CanvasProxy.invariantServicePorts.httpMetadata
            CanvasProxy.defaultMetadataRoute
            (getTcpFramesPort primaryDeviceAddress)
            secondaryDeviceAddresses
            None
    let scene = scene { do sceneFunc.Invoke() }
    do start createCanvas scene       

let stop () =
    if ApiEnv.isInInteractiveContext
    then stopInternal.invoke()
    else ()
