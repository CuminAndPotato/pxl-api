#I "../.pxlLocalDev"

#r "SkiaSharp.dll"
#r "Pxl.dll"
#r "Pxl.Ui.dll"
#r "Pxl.Ui.CSharp.dll"

open Pxl
open Pxl.Ui



let myScene = fun () ->
    Pxl.DrawLine(0, 0, Pxl.width, Pxl.height) |> ignore
    ()



myScene |> Simulator.startWith "localhost"


