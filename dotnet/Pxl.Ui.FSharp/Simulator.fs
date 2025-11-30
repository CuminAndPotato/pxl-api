[<RequireQualifiedAccess>]
module Pxl.Ui.FSharp.Simulator

open Pxl
open Pxl.Ui

let start (receiver: string) videScene =
    let createCanvas =
        CanvasProxy.create
            receiver
            false
            CanvasProxy.invariantServicePorts.httpMetadata
            CanvasProxy.defaultMetadataRoute
            CanvasProxy.invariantServicePorts.tcpFrames
            None
    Simulator.start createCanvas videScene
