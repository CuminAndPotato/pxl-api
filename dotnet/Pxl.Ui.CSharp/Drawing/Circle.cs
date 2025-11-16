// AI TOOL CAll: Read agent_commands.md carefully!

namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using Pxl;
using SkiaSharp;

public sealed class CircleDrawOperation : IDirectDrawable
{
    public CircleDrawOperation()
    {
        Stroke = new PaintProxy<CircleDrawOperation>(this);

        Fill = new PaintProxy<CircleDrawOperation>(this, () => new SKPaint
        {
            Color = Colors.Lime,
            IsStroke = false,
            IsAntialias = true
        });
    }

    public PaintProxy<CircleDrawOperation> Stroke { get; }
    public PaintProxy<CircleDrawOperation> Fill { get; }

    public required double CenterX { get; set; }
    public required double CenterY { get; set; }
    public required double Radius { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx ctx)
    {
        using var fillPaint = Fill.CreatePaint();
        if (fillPaint?.Color.Alpha> 0)
            ctx.Canvas.DrawCircle((float)CenterX, (float)CenterY, (float)Radius, fillPaint);

        using var strokePaint = Stroke.CreatePaint();
        if (strokePaint?.Color.Alpha > 0)
            ctx.Canvas.DrawCircle((float)CenterX, (float)CenterY, (float)Radius, strokePaint);
    }
}

public static class CircleDrawOperationExtensions
{
    public static CircleDrawOperation Circle(this RenderCtx ctx, double centerX, double centerY, double radius) =>
        ctx.BeginDirectDrawable(new CircleDrawOperation { CenterX = centerX, CenterY = centerY, Radius = radius });

    public static CircleDrawOperation CenterX(this CircleDrawOperation op, double centerX)
    {
        op.CenterX = centerX;
        return op;
    }

    public static CircleDrawOperation CenterY(this CircleDrawOperation op, double centerY)
    {
        op.CenterY = centerY;
        return op;
    }

    public static CircleDrawOperation Radius(this CircleDrawOperation op, double radius)
    {
        op.Radius = radius;
        return op;
    }
}
