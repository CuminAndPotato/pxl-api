// AI TOOL CAll: Read agent_commands.md carefully!

namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using Pxl;
using SkiaSharp;

public sealed class TextDrawOperation : IDirectDrawable
{
    public TextDrawOperation()
    {
        Fill = new PaintProxy<TextDrawOperation>(this, () => new SKPaint
        {
            Color = Colors.White,
            IsStroke = false,
            IsAntialias = false
        });
    }

    public PaintProxy<TextDrawOperation> Fill { get; }

    public required string Text { get; set; }
    public required double X { get; set; }
    public required double Y { get; set; }
    public FontInfo Font { get; set; } = Fonts.Var4x5;
    public SKFontEdging Edging { get; set; } = SKFontEdging.Alias;
    public SKFontHinting Hinting { get; set; } = SKFontHinting.None;
    public double? Size { get; set; }
    public double ScaleX { get; set; } = 1.0;
    public double SkewX { get; set; } = 0.0;
    public bool Embolden { get; set; } = false;
    public bool BaselineSnap { get; set; } = false;
    public bool LinearMetrics { get; set; } = false;
    public bool Subpixel { get; set; } = false;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx ctx)
    {
        using var font = new SKFont(Font.Typeface, (float)(Size ?? Font.DefaultHeight))
        {
            Edging = Edging,
            Hinting = Hinting,
            ScaleX = (float)ScaleX,
            SkewX = (float)SkewX,
            Embolden = Embolden,
            BaselineSnap = BaselineSnap,
            LinearMetrics = LinearMetrics,
            Subpixel = Subpixel,
        };
        using var fillPaint = Fill.CreatePaint();

        // Y position includes ascent and font size (ascent is typically negative or 0)
        var drawY = (float)(Y + Font.DefaultAscent + Font.DefaultHeight);
        ctx.Canvas.DrawText(Text, (float)X, drawY, font, fillPaint);
    }
}

public class TextProxy(RenderCtx ctx)
{
    public TextDrawOperation Font(string text, double x, double y, FontInfo font) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = font });

    public TextDrawOperation Var3x5(string text, string fontName, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Var3x5 });

    public TextDrawOperation Var3x5(string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Var3x5 });

    public TextDrawOperation Mono3x5(string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Mono3x5 });

    public TextDrawOperation Var4x5(string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Var4x5 });

    public TextDrawOperation Mono4x5(string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Mono4x5 });

    public TextDrawOperation Mono6x6(string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Mono6x6 });

    public TextDrawOperation Mono7x10(string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Mono7x10 });

    public TextDrawOperation Var10x10(string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Var10x10 });

    public TextDrawOperation Mono10x10(string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Mono10x10 });

    public TextDrawOperation Mono16x16(string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y, Font = Fonts.Mono16x16 });
}

public static class TextDrawOperationExtensions
{
    public static TextProxy Text(this RenderCtx ctx) => new(ctx);

    public static TextDrawOperation Text(this RenderCtx ctx, string text, double x, double y) =>
        ctx.BeginDirectDrawable(new TextDrawOperation { Text = text, X = x, Y = y });

    public static TextDrawOperation Text(this TextDrawOperation op, string text)
    {
        op.Text = text;
        return op;
    }

    public static TextDrawOperation X(this TextDrawOperation op, double x)
    {
        op.X = x;
        return op;
    }

    public static TextDrawOperation Y(this TextDrawOperation op, double y)
    {
        op.Y = y;
        return op;
    }

    public static TextDrawOperation Edging(this TextDrawOperation op, SKFontEdging edging)
    {
        op.Edging = edging;
        return op;
    }

    public static TextDrawOperation Hinting(this TextDrawOperation op, SKFontHinting hinting)
    {
        op.Hinting = hinting;
        return op;
    }

    public static TextDrawOperation Size(this TextDrawOperation op, double size)
    {
        op.Size = size;
        return op;
    }

    public static TextDrawOperation ScaleX(this TextDrawOperation op, double scaleX)
    {
        op.ScaleX = scaleX;
        return op;
    }

    public static TextDrawOperation SkewX(this TextDrawOperation op, double skewX)
    {
        op.SkewX = skewX;
        return op;
    }

    public static TextDrawOperation Embolden(this TextDrawOperation op, bool embolden = true)
    {
        op.Embolden = embolden;
        return op;
    }

    public static TextDrawOperation BaselineSnap(this TextDrawOperation op, bool baselineSnap = true)
    {
        op.BaselineSnap = baselineSnap;
        return op;
    }

    public static TextDrawOperation LinearMetrics(this TextDrawOperation op, bool linearMetrics = true)
    {
        op.LinearMetrics = linearMetrics;
        return op;
    }

    public static TextDrawOperation Subpixel(this TextDrawOperation op, bool subpixel = true)
    {
        op.Subpixel = subpixel;
        return op;
    }
}
