[<RequireQualifiedAccess>]
module Pxl.Simulator

open System
open System.IO
open System.Reflection

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

let startEx (createCanvas: (unit -> unit) -> Canvas) scene =
    if isInInteractiveContext then
        let mutable retry = true

        let rec reStart () =
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
                        scene)
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

let start (receiver: string) scene =
    scene |> startEx (CanvasProxy.createWithDefaults receiver)

let startWith (receiver: string) (sceneFunc: Action) =
    let scene = scene { do sceneFunc.Invoke() }
    scene |> startEx (CanvasProxy.createWithDefaults receiver)
    Console.WriteLine("Simulator started. Press any key to exit...");
    Console.ReadKey() |> ignore

let stop () =
    if isInInteractiveContext
    then stopInternal.invoke()
    else ()
