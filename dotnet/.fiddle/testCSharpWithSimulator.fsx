#I "../.pxlLocalDev"

#r "SkiaSharp.dll"
#r "Pxl.dll"
#r "Pxl.Ui.dll"
#r "Pxl.Ui.CSharp.dll"

open Pxl
open Pxl.Ui



let myScene = fun (ctx: RenderCtx) ->
    ctx.DrawLine(0, 0, ctx.width, ctx.height) |> ignore
    ()


let videScene =
    scene {
        let! ctx = getCtx ()
        do myScene ctx
    }


videScene |> Simulator.start "localhost"

