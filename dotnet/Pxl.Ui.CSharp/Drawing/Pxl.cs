// AI TOOL CAll: Read agent_commands.md carefully!

namespace PxlClock;

using Pxl;
using Pxl.Ui.CSharp;

public partial class PxlDrawOperation(double x, double y) : IDirectDrawable
{
    [BuilderStyle] public double X { get; set; } = x;
    [BuilderStyle] public double Y { get; set; } = y;
    [BuilderStyle] public Color Color { get; set; } = Colors.lime;
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
    public static PxlDrawOperation Pxl(this RenderCtx ctx, double x, double y) => ctx.BeginDirectDrawable(new PxlDrawOperation(x, y));

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
