namespace Pxl

open SkiaSharp
open Pxl
open System
open System.Runtime.InteropServices
open System.Collections.Generic
open System.Threading

type Buttons =
    {
        lowerButtonPressed : bool
        upperButtonPressed : bool
    }

module Interop =
    // we know that the struct layout of SkColor in our case is RGBA8888 (see above)

    let copyPixelSpanToArray (srcPixelxSpan: Span<byte>) (dest: Color[]) =
        let colorSpan = MemoryMarshal.Cast<byte, Color>(srcPixelxSpan)
        colorSpan.CopyTo(dest)

    let pixelSpanToArray (srcPixelxSpan: Span<byte>) =
        let dest = Array.zeroCreate<Color>(srcPixelxSpan.Length / 4)
        copyPixelSpanToArray srcPixelxSpan dest
        dest

type IDirectDrawable =
    abstract member End: RenderCtx -> unit

// we use this to provide extensions like line, pxl, etc.
and DrawEntry(ctx: RenderCtx) =
    member _.Ctx = ctx

and RenderCtxInstances =
    static member val internal RenderContexts = Dictionary<int, RenderCtx>()

and RenderCtx
    (
        width: int,
        height: int,
        fps: int,
        ?onEndCycle: Color array -> unit
    )
    =

    let _skImageInfo = SKImageInfo(width, height, SKColorType.Bgra8888)
    let _skSurface = SKSurface.Create(_skImageInfo)
    let _skCanvas = _skSurface.Canvas
    let _skBmp = new SKBitmap(_skImageInfo)

    let _width = float width
    let _height = float height

    let defaultClearBackground = true

    let mutable _now = DateTimeOffset.MinValue
    let mutable _cycleNo = -1L
    let mutable _startTime = DateTimeOffset.MinValue
    let mutable _clear = defaultClearBackground
    let mutable _buttons = { lowerButtonPressed = false; upperButtonPressed = false }

    let mutable _currentDirectDrawable : IDirectDrawable option = None

    [<CompiledName("WidthF")>] member _.width = _width
    [<CompiledName("HeightF")>] member _.height = _height
    [<CompiledName("Width")>] member _.widthInt = width
    [<CompiledName("Height")>] member _.heightInt = height
    [<CompiledName("HalfWidth")>] member _.halfWidth = float _width / 2.0
    [<CompiledName("HalfHeight")>] member _.halfHeight = float _height / 2.0

    [<CompiledName("Now")>] member _.now = _now
    [<CompiledName("CycleNo")>] member _.cycleNo = _cycleNo
    [<CompiledName("Elapsed")>] member _.elapsed = _startTime - _now
    [<CompiledName("Fps")>] member _.fps = fps

    [<CompiledName("SkiaCanvas")>] member _.skiaCanvas = _skCanvas
    [<CompiledName("SkiaBitmap")>] member internal _.skiaBitmap = _skBmp
    [<CompiledName("SkiaSurface")>] member internal _.skiaSurface = _skSurface
    [<CompiledName("SkiaImageInfo")>] member internal _.skiaImageInfo = _skImageInfo
    
    [<CompiledName("CurrentDirectDrawable")>] member internal _.currentDirectDrawable = _currentDirectDrawable

    [<CompiledName("Buttons")>] member _.buttons = _buttons

    member _.ClearScreenOnCycleCompleted(value) =
        _clear <- value

    [<Obsolete("F# compat only")>]
    member _.GetRawSnapshot() =
        use intermediateImage = SKImage.FromBitmap(_skBmp)
        intermediateImage.PeekPixels()

    [<Obsolete("F# compat only")>]
    member this.GetSnapshot() =
        Interop.pixelSpanToArray (this.GetRawSnapshot().GetPixelSpan())

    member internal this.PrepareCycle(startTime: DateTimeOffset, now: DateTimeOffset, buttons: Buttons) =
        _startTime <- startTime
        _now <- now
        _cycleNo <- _cycleNo + 1L
        _buttons <- buttons
        if _clear then
            _skCanvas.Clear(SKColors.Black)
        _clear <- defaultClearBackground
        _skCanvas.ResetMatrix()
        RenderCtxInstances.RenderContexts[Thread.CurrentThread.ManagedThreadId] <- this

    member internal this.BeginDirectDrawable(directDrawable: 'a when 'a :> IDirectDrawable) : 'a =
        this.EndDirectDrawable()
        let dd = directDrawable :> IDirectDrawable
        _currentDirectDrawable <- Some dd
        directDrawable

    member internal this.EndDirectDrawable() =
        match _currentDirectDrawable with
        | Some drawable -> drawable.End(this)
        | None -> ()

        _currentDirectDrawable <- None

    member internal this.Flush() =
        do this.EndDirectDrawable()
        
        do _skCanvas.Flush()

        let couldRead = _skSurface.ReadPixels(_skImageInfo, _skBmp.GetPixels(), _skImageInfo.RowBytes, 0, 0)
        if couldRead |> not then
            failwith "Failed to read pixels from SKSurface"
    
    member internal this.FlushAndGetPixelSpan() =
        this.Flush()
        // we know that the struct layout of SkColor in our case: see above
        _skBmp.GetPixelSpan()

    member internal this.FlushAndCopy(dest: Color[]) =
        let srcPixelxSpan = this.FlushAndGetPixelSpan()
        MemoryMarshal.Cast<byte, Color>(srcPixelxSpan).CopyTo(dest)

    member internal this.FlushAndCopy(dest: SKColor[]) =
        let srcPixelxSpan = this.FlushAndGetPixelSpan()
        MemoryMarshal.Cast<byte, SKColor>(srcPixelxSpan).CopyTo(dest)
        ()

    member internal this.EndCycle(dest: Color[]) =
        this.FlushAndCopy(dest)
        onEndCycle |> Option.iter (fun f -> f dest)
        ()
