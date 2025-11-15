[<RequireQualifiedAccess>]
module Pxl.Ui.FSharp.Simulator

open Pxl
open Pxl.Ui

let start (receiver: string) videScene =
    Simulator.start
        (CanvasProxy.createWithDefaults receiver)
        videScene
