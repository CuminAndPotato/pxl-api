namespace Pxl

open System.Runtime.InteropServices
open System.Runtime.CompilerServices

// Important: Don't change the layout;
// it has to be the same as the Skia Color struct
[<StructLayout(LayoutKind.Sequential)>]
type [<Struct>] Color =
    {
        r: byte
        g: byte
        b: byte
        a: byte
    }

    static member inline argb(a, r, g, b) =
        { a = byte a; r = byte r; g = byte g; b = byte b }

    static member inline rgba(r, g, b, a) =
        { a = byte a; r = byte r; g = byte g; b = byte b }

    static member inline rgb(r, g, b) =
        { a = 255uy; r = byte r; g = byte g; b = byte b }

    static member inline mono(v) =
        let v = byte v in Color.rgb(v, v, v)

    static member inline hsv(hue: float, saturation: float, value: float) =
        // Hue can be large or negative, so let’s normalize it into [0..360)
        let hue = hue % 360.0 |> (fun x -> if x < 0.0 then x + 360.0 else x)
        let saturation = clamp01 saturation
        let value = clamp01 value

        // C is the "chroma": the difference between the maximum and minimum
        // values of RGB (based on saturation and value)
        let c = value * saturation

        // Find the position within the 6 regions each of 60°
        // hh is basically (h / 60)
        let hh = hue / 60.0

        // X is an intermediate value determined by which region hue is in
        let x = c * (1.0 - abs(hh % 2.0 - 1.0))

        // Determine base RGB based on sector
        let r1, g1, b1 =
            if hh < 1.0 then c, x, 0.0
            elif hh < 2.0 then x, c, 0.0
            elif hh < 3.0 then 0.0, c, x
            elif hh < 4.0 then 0.0, x, c
            elif hh < 5.0 then x, 0.0, c
            else c, 0.0, x

        // m shifts all channels to match the actual value v
        let m = value - c

        // Convert rgb to 8-bit values (0..255)
        {
            a = 255uy
            r = byte (255.0 * (r1 + m))
            g = byte (255.0 * (g1 + m))
            b = byte (255.0 * (b1 + m))
        }

    static member hsva(h: float, s: float, v: float, a: float) =
        let color = Color.hsv(h, s, v)
        { color with a = a * 255.0 |> byte }

    /// Convert an RGB color (plus alpha) to HSV
    member this.toHSV() =
        let rf = float this.r / 255.0
        let gf = float this.g / 255.0
        let bf = float this.b / 255.0

        let maxVal = max rf (max gf bf)
        let minVal = min rf (min gf bf)
        let delta = maxVal - minVal

        // Compute H
        let mutable h =
            if delta < 1e-6 then
                0.0
            elif maxVal = rf then
                60.0 * (((gf - bf) / delta) % 6.0)
            elif maxVal = gf then
                60.0 * (((bf - rf) / delta) + 2.0)
            else
                60.0 * (((rf - gf) / delta) + 4.0)
        // Normalize hue to [0..360)
        if h < 0.0 then h <- h + 360.0

        // Compute S
        let s =
            if maxVal < 1e-6
            then 0.0
            else delta / maxVal

        // Compute V
        let v = maxVal

        (h, s, v)

    [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
    member this.opacity() =
        float this.a / 255.0

    [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
    member this.opacity(value: float) =
        let this = this
        { this with a = (clamp01 value) * 255.0 |> byte }

    [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
    member this.brightness() =
        let r, g, b = this.r, this.g, this.b
        // Standard brightness calculation using maximum value of the RGB components
        // This will return 1.0 for white (255,255,255)
        let maxColor =
            let maxRG = if r > g then r else g
            if maxRG > b then maxRG else b
        float maxColor / 255.0

    /// Return a new Color by scaling this color’s RGB values by the given factor.
    [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
    member this.brightness(value: float) =
        let value = clamp01 value
        {
            a = this.a
            r = int this.r * int (value * 256.0) >>> 8 |> byte
            g = int this.g * int (value * 256.0) >>> 8 |> byte
            b = int this.b * int (value * 256.0) >>> 8 |> byte
        }

    /// Return a new Color by setting (or shifting) this color’s hue.
    [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
    member this.hue(value: float) =
        // 1. Convert old color to HSV
        let (_,s,v) = this.toHSV()
        // 2. Create new color with the given hue, same s/v
        let newColor = Color.hsv(value, s, v)
        // 3. Preserve the existing alpha channel
        { a = this.a; r = newColor.r; g = newColor.g; b = newColor.b }
