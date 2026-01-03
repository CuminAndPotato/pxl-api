namespace Pxl.Ui.FSharp

open System

type AppType =
    | ClockFace = 0
    | Sequence = 1
    | Debug = 2

[<AttributeUsage(AttributeTargets.All, AllowMultiple = false)>]
type AppFSharpV1Attribute(name: string) =
    inherit Attribute()
    member _.name = name
    member val defaultDurationInSeconds = 15.0 with get, set
    member val displayName = "" with get, set
    member val author = "" with get, set
    member val description = "" with get, set
    member val appType = AppType.ClockFace with get, set
