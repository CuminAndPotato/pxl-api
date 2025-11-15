[<RequireQualifiedAccess>]
module Pxl.Ui.FSharp.Simulator

open Pxl

let start (receiver: string) videScene =
    Simulator.start
        (CanvasProxy.createWithDefaults receiver)
        videScene
