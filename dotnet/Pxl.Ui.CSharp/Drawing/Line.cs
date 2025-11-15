// AI TOOL CAll: Read agent_commands.md carefully!

namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using Pxl;
using Pxl.Ui.CSharp.Base;

public sealed class LineDrawOperation : IDirectDrawable
{
    [BuilderStyle] public required double X1 { get; set; }
    [BuilderStyle] public required double Y1 { get; set; }
    [BuilderStyle] public required double X2 { get; set; }
    [BuilderStyle] public required double Y2 { get; set; }
    [BuilderStyle] public Color Color { get; set; } = Colors.Lime;
    [BuilderStyle] public double Thickness { get; set; } = 1.0;
    [BuilderStyle] public bool AntiAlias { get; set; } = false;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx value)
    {
        var skiaCanvas = value.canvas;
        var skColor = new SkiaSharp.SKColor(Color.r, Color.g, Color.b, Color.a);
        using var paint = new SkiaSharp.SKPaint
        {
            Color = skColor,
            StrokeWidth = (float)Thickness,
            IsAntialias = AntiAlias,
            Style = SkiaSharp.SKPaintStyle.Stroke
        };
        skiaCanvas.DrawLine((float)X1, (float)Y1, (float)X2, (float)Y2, paint);
    }
}

public static class LineDrawOperationExtensions
{
    public static LineDrawOperation DrawLine(this RenderCtx ctx, double x1, double y1, double x2, double y2) =>
        ctx.BeginDirectDrawable(new LineDrawOperation { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 });

    public static LineDrawOperation X1(this LineDrawOperation op, double x1)
    {
        op.X1 = x1;
        return op;
    }

    public static LineDrawOperation Y1(this LineDrawOperation op, double y1)
    {
        op.Y1 = y1;
        return op;
    }

    public static LineDrawOperation X2(this LineDrawOperation op, double x2)
    {
        op.X2 = x2;
        return op;
    }

    public static LineDrawOperation Y2(this LineDrawOperation op, double y2)
    {
        op.Y2 = y2;
        return op;
    }

    public static LineDrawOperation Color(this LineDrawOperation op, Color color)
    {
        op.Color = color;
        return op;
    }

    public static LineDrawOperation Thickness(this LineDrawOperation op, double thickness)
    {
        op.Thickness = thickness;
        return op;
    }

    public static LineDrawOperation AntiAlias(this LineDrawOperation op, bool antiAlias)
    {
        op.AntiAlias = antiAlias;
        return op;
    }
}
