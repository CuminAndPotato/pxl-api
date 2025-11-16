namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using Pxl;
using SkiaSharp;

public sealed class PointDrawOperation : IDirectDrawable
{
    public PointDrawOperation()
    {
        Stroke = new PaintProxy<PointDrawOperation>(this, () => new SKPaint
        {
            Color = Colors.Lime,
            StrokeWidth = 1,
            IsStroke = true,
            IsAntialias = true
        });
    }

    public PaintProxy<PointDrawOperation> Stroke { get; }

    public required double X { get; set; }
    public required double Y { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx ctx)
    {
        using var paint = Stroke.CreatePaint();
        ctx.Canvas.DrawPoint((float)X, (float)Y, paint);
    }
}

public static class PointDrawOperationExtensions
{
    public static PointDrawOperation Point(this RenderCtx ctx, double x, double y) =>
        ctx.BeginDirectDrawable(new PointDrawOperation { X = x, Y = y });

    public static PointDrawOperation X(this PointDrawOperation op, double x)
    {
        op.X = x;
        return op;
    }

    public static PointDrawOperation Y(this PointDrawOperation op, double y)
    {
        op.Y = y;
        return op;
    }
}
