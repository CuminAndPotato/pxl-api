namespace Pxl.Ui.CSharp;

using SkiaSharp;

public record FontInfo(SKTypeface Typeface, double DefaultHeight, double DefaultAscent);

public static class Fonts
{
    public static readonly FontInfo Var3x5 = Load("cg-pixel-3x5-prop.otf", 5, 0);
    public static readonly FontInfo Mono3x5 = Load("cg-pixel-3x5-mono.otf", 5, 0);
    public static readonly FontInfo Var4x5 = Load("cg-pixel-4x5-prop.otf", 5, 0);
    public static readonly FontInfo Mono4x5 = Load("cg-pixel-4x5-mono.otf", 5, 0);
    public static readonly FontInfo Mono6x6 = Load("6x6-pixel-yc-fs.ttf", 6, 0);
    public static readonly FontInfo Mono7x10 = Load("7kh10.ttf", 12, -2);
    public static readonly FontInfo Var10x10 = Load("super04b.ttf", 10, 0);
    public static readonly FontInfo Mono10x10 = Load("10x10-monospaced-font.ttf", 16, -6);
    public static readonly FontInfo Mono16x16 = Load("ascii-sector-16x16-tileset.otf", 16, -2);

    private static FontInfo Load(string fileName, double defaultHeight, double defaultAscent)
    {
        var assembly = typeof(TextDrawOperation).Assembly;
        var resourceName = $"Pxl.Ui.CSharp.Drawing.Fonts.{fileName}";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream != null)
            return new FontInfo(SKTypeface.FromStream(stream), defaultHeight, defaultAscent);
        throw new InvalidOperationException($"Font resource '{resourceName}' not found.");
    }
}
