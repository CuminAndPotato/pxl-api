// AI TOOL CAll: Read agent_commands.md carefully!

namespace Pxl.Ui.CSharp;

using Pxl;
using Pxl.Ui;
using Pxl.Ui.CSharp;
using Pxl.Ui.CSharp.Base;

public sealed class PxlDrawOperation : IDirectDrawable
{
    [BuilderStyle] public required double X { get; set; }
    [BuilderStyle] public required double Y { get; set; }
    [BuilderStyle] public Color Color { get; set; } = Colors.Lime;
    [BuilderStyle] public double Size { get; set; } = 1.0;

    public void End(RenderCtx value)
    {
        var skiaCanvas = value.canvas;
        var skColor = new SkiaSharp.SKColor(Color.r, Color.g, Color.b, Color.a);
        using var paint = new SkiaSharp.SKPaint
        {
            Color = skColor,
            IsAntialias = false,
            Style = SkiaSharp.SKPaintStyle.Fill
        };
        if (Size <= 1.0)
            skiaCanvas.DrawPoint((float)X, (float)Y, paint);
        else
            skiaCanvas.DrawRect((float)X, (float)Y, (float)Size, (float)Size, paint);
    }
}

public static class PxlDrawOperationExtensions
{
    public static PxlDrawOperation Pxl(this RenderCtx ctx, double x, double y) =>
        ctx.BeginDirectDrawable(new PxlDrawOperation { X = x, Y = y });

    public static PxlDrawOperation X(this PxlDrawOperation op, double x)
    {
        op.X = x;
        return op;
    }

    public static PxlDrawOperation Y(this PxlDrawOperation op, double y)
    {
        op.Y = y;
        return op;
    }

    public static PxlDrawOperation Color(this PxlDrawOperation op, Color color)
    {
        op.Color = color;
        return op;
    }

    public static PxlDrawOperation Size(this PxlDrawOperation op, double size)
    {
        op.Size = size;
        return op;
    }
}
