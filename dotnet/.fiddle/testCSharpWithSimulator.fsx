#I "../.pxlLocalDev/Pxl"

#r "SkiaSharp.dll"
#r "Pxl.dll"
#r "Pxl.Ui.dll"
#r "Pxl.Ui.CSharp.dll"

open Pxl



let scene = fun () ->
    for frame in Pxl.Ui.Scene.Frames do
        // draw something
        printfn $"Frame NOW = {frame.now}"




// eigentlich muss das gemacht werden, was der Simulator macht
let canvas = CanvasProxy.createWithDefaults "localhost" id

Pxl.Evaluation.startCSharp(
    canvas,
    RenderCtx(canvas.Metadata.width, canvas.Metadata.height, canvas.Metadata.fps),
    (fun ex ->
        // TODO: retry
        printfn $"Error in evaluating App logic: {ex.Message}"
        
    ),
    Reality.forRealTime (),
    (fun () -> { lowerButtonPressed = false; upperButtonPressed = false }),
    scene
    )
