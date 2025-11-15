// AI TOOL CAll: Read agent_commands.md carefully!

namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using Pxl;
using SkiaSharp;

public sealed class RectDrawOperation : IDirectDrawable
{
    public PaintProxy Stroke { get; } =
        new(() => new SKPaint
        {
            Color = Colors.Lime,
            StrokeWidth = 1,
            IsStroke = true,
            IsAntialias = true
        });

    public PaintProxy Fill { get; } =
        new(() => new SKPaint
        {
            Color = Colors.TransparentBlack,
            IsStroke = false,
            IsAntialias = true
        });

    public required double X { get; set; }
    public required double Y { get; set; }
    public required double Width { get; set; }
    public required double Height { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx ctx)
    {
        var rect = new SKRect((float)X, (float)Y, (float)(X + Width), (float)(Y + Height));

        using var fillPaint = Fill.CreatePaint();
        if (fillPaint.Color.Alpha > 0)
        {
            ctx.Canvas.DrawRect(rect, fillPaint);
        }

        using var strokePaint = Stroke.CreatePaint();
        if (strokePaint.Color.Alpha > 0)
        {
            ctx.Canvas.DrawRect(rect, strokePaint);
        }
    }
}

public static class RectDrawOperationExtensions
{
    public static RectDrawOperation RectXyWh(this RenderCtx ctx, double x, double y, double width, double height) =>
        ctx.BeginDirectDrawable(new RectDrawOperation { X = x, Y = y, Width = width, Height = height });

    public static RectDrawOperation RectXyXy(this RenderCtx ctx, double x1, double y1, double x2, double y2) =>
        ctx.BeginDirectDrawable(new RectDrawOperation { X = x1, Y = y1, Width = x2 - x1, Height = y2 - y1 });

    public static RectDrawOperation X(this RectDrawOperation op, double x)
    {
        op.X = x;
        return op;
    }

    public static RectDrawOperation Y(this RectDrawOperation op, double y)
    {
        op.Y = y;
        return op;
    }

    public static RectDrawOperation Width(this RectDrawOperation op, double width)
    {
        op.Width = width;
        return op;
    }

    public static RectDrawOperation Height(this RectDrawOperation op, double height)
    {
        op.Height = height;
        return op;
    }
}
