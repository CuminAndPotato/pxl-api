// AI TOOL CAll: Read agent_commands.md carefully!

namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using Pxl;
using SkiaSharp;

public sealed class ArcDrawOperation : IDirectDrawable
{
    public ArcDrawOperation()
    {
        Stroke = new PaintProxy<ArcDrawOperation>(this);

        Fill = new PaintProxy<ArcDrawOperation>(this, () => new SKPaint
        {
            Color = Colors.TransparentBlack,
            IsStroke = false,
            IsAntialias = true
        });
    }

    public PaintProxy<ArcDrawOperation> Stroke { get; }
    public PaintProxy<ArcDrawOperation> Fill { get; }

    public required double X { get; set; }
    public required double Y { get; set; }
    public required double Width { get; set; }
    public required double Height { get; set; }
    public required double StartAngle { get; set; }
    public required double SweepAngle { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx ctx)
    {
        var rect = new SKRect((float)X, (float)Y, (float)(X + Width), (float)(Y + Height));
        var centerX = rect.MidX;
        var centerY = rect.MidY;

        using var fillPaint = Fill.CreatePaint();
        if (fillPaint?.Color.Alpha > 0)
        {
            using var path = new SKPath();
            path.MoveTo(centerX, centerY);
            path.ArcTo(rect, (float)StartAngle, (float)SweepAngle, false);
            path.Close();
            ctx.Canvas.DrawPath(path, fillPaint);
        }

        using var strokePaint = Stroke.CreatePaint();
        if (strokePaint?.Color.Alpha > 0)
        {
            using var path = new SKPath();
            path.MoveTo(centerX, centerY);
            path.ArcTo(rect, (float)StartAngle, (float)SweepAngle, false);
            path.Close();
            ctx.Canvas.DrawPath(path, strokePaint);
        }
    }
}

public static class ArcDrawOperationExtensions
{
    public static ArcDrawOperation Arc(this RenderCtx ctx, double x, double y, double width, double height, double startAngle, double sweepAngle) =>
        ctx.BeginDirectDrawable(new ArcDrawOperation { X = x, Y = y, Width = width, Height = height, StartAngle = startAngle, SweepAngle = sweepAngle });

    public static ArcDrawOperation ArcCenter(this RenderCtx ctx, double centerX, double centerY, double radius, double startAngle, double sweepAngle) =>
        ctx.BeginDirectDrawable(new ArcDrawOperation
        {
            X = centerX - radius,
            Y = centerY - radius,
            Width = radius * 2,
            Height = radius * 2,
            StartAngle = startAngle,
            SweepAngle = sweepAngle
        });

    public static ArcDrawOperation X(this ArcDrawOperation op, double x)
    {
        op.X = x;
        return op;
    }

    public static ArcDrawOperation Y(this ArcDrawOperation op, double y)
    {
        op.Y = y;
        return op;
    }

    public static ArcDrawOperation Width(this ArcDrawOperation op, double width)
    {
        op.Width = width;
        return op;
    }

    public static ArcDrawOperation Height(this ArcDrawOperation op, double height)
    {
        op.Height = height;
        return op;
    }

    public static ArcDrawOperation StartAngle(this ArcDrawOperation op, double startAngle)
    {
        op.StartAngle = startAngle;
        return op;
    }

    public static ArcDrawOperation SweepAngle(this ArcDrawOperation op, double sweepAngle)
    {
        op.SweepAngle = sweepAngle;
        return op;
    }
}
