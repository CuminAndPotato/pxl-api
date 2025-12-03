namespace Pxl.Ui.CSharp;

using SkiaSharp;

/// <summary>
/// Blend modes control how colors are combined when drawing on top of existing content.
/// </summary>
public enum BlendMode
{
    /// <summary>
    /// Clear destination (replace with transparent).
    /// </summary>
    Clear = SKBlendMode.Clear,

    /// <summary>
    /// Replace destination with source.
    /// </summary>
    Source = SKBlendMode.Src,

    /// <summary>
    /// Keep destination, ignore source.
    /// </summary>
    Destination = SKBlendMode.Dst,

    /// <summary>
    /// Source over destination (default alpha blending).
    /// </summary>
    SourceOver = SKBlendMode.SrcOver,

    /// <summary>
    /// Destination over source.
    /// </summary>
    DestinationOver = SKBlendMode.DstOver,

    /// <summary>
    /// Source inside destination (mask source by destination alpha).
    /// </summary>
    SourceIn = SKBlendMode.SrcIn,

    /// <summary>
    /// Destination inside source (mask destination by source alpha).
    /// </summary>
    DestinationIn = SKBlendMode.DstIn,

    /// <summary>
    /// Source outside destination (inverse mask).
    /// </summary>
    SourceOut = SKBlendMode.SrcOut,

    /// <summary>
    /// Destination outside source (inverse mask).
    /// </summary>
    DestinationOut = SKBlendMode.DstOut,

    /// <summary>
    /// Source atop destination (source where destination is opaque).
    /// </summary>
    SourceAtop = SKBlendMode.SrcATop,

    /// <summary>
    /// Destination atop source (destination where source is opaque).
    /// </summary>
    DestinationAtop = SKBlendMode.DstATop,

    /// <summary>
    /// Exclusive OR (source XOR destination).
    /// </summary>
    Xor = SKBlendMode.Xor,

    /// <summary>
    /// Add source and destination (clamps to 1.0).
    /// </summary>
    Plus = SKBlendMode.Plus,

    /// <summary>
    /// Multiply source and destination (darkens).
    /// </summary>
    Modulate = SKBlendMode.Modulate,

    /// <summary>
    /// Screen blend mode (inverted multiply, lightens).
    /// </summary>
    Screen = SKBlendMode.Screen,

    /// <summary>
    /// Overlay blend mode (combination of multiply and screen).
    /// </summary>
    Overlay = SKBlendMode.Overlay,

    /// <summary>
    /// Darken: keeps the darker of source and destination.
    /// </summary>
    Darken = SKBlendMode.Darken,

    /// <summary>
    /// Lighten: keeps the lighter of source and destination.
    /// </summary>
    Lighten = SKBlendMode.Lighten,

    /// <summary>
    /// Color dodge (brightens destination based on source).
    /// </summary>
    ColorDodge = SKBlendMode.ColorDodge,

    /// <summary>
    /// Color burn (darkens destination based on source).
    /// </summary>
    ColorBurn = SKBlendMode.ColorBurn,

    /// <summary>
    /// Hard light (harsh version of overlay).
    /// </summary>
    HardLight = SKBlendMode.HardLight,

    /// <summary>
    /// Soft light (gentle version of overlay).
    /// </summary>
    SoftLight = SKBlendMode.SoftLight,

    /// <summary>
    /// Difference (absolute difference between source and destination).
    /// </summary>
    Difference = SKBlendMode.Difference,

    /// <summary>
    /// Exclusion (similar to difference but lower contrast).
    /// </summary>
    Exclusion = SKBlendMode.Exclusion,

    /// <summary>
    /// Multiply (darkens by multiplying colors).
    /// </summary>
    Multiply = SKBlendMode.Multiply,

    /// <summary>
    /// Hue blend mode (use source hue with destination saturation and luminosity).
    /// </summary>
    Hue = SKBlendMode.Hue,

    /// <summary>
    /// Saturation blend mode (use source saturation with destination hue and luminosity).
    /// </summary>
    Saturation = SKBlendMode.Saturation,

    /// <summary>
    /// Color blend mode (use source hue and saturation with destination luminosity).
    /// </summary>
    Color = SKBlendMode.Color,

    /// <summary>
    /// Luminosity blend mode (use source luminosity with destination hue and saturation).
    /// </summary>
    Luminosity = SKBlendMode.Luminosity
}

/// <summary>
/// Extension methods for converting between BlendMode and SKBlendMode.
/// </summary>
public static class BlendModeExtensions
{
    /// <summary>
    /// Converts a BlendMode to SKBlendMode.
    /// </summary>
    public static SKBlendMode ToSkia(this BlendMode mode) => (SKBlendMode)mode;

    /// <summary>
    /// Converts an SKBlendMode to BlendMode.
    /// </summary>
    public static BlendMode FromSkia(SKBlendMode mode) => (BlendMode)mode;
}
