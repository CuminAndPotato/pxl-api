// AI TOOL CAll: Read agent_commands.md carefully!

namespace Pxl.Ui.CSharp;

using System.ComponentModel;
using Pxl;
using SkiaSharp;

public sealed class LineDrawOperation : IDirectDrawable
{
    public PaintProxy<LineDrawOperation> Stroke =>
        new(this, () => new SKPaint
        {
            Color = Colors.Lime,
            StrokeWidth = 1,
            IsStroke = true,
            IsAntialias = true
        });

    public required double X1 { get; set; }
    public required double Y1 { get; set; }
    public required double X2 { get; set; }
    public required double Y2 { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void End(RenderCtx ctx)
    {
        using var paint = Stroke.CreatePaint();
        ctx.Canvas.DrawLine((float)X1, (float)Y1, (float)X2, (float)Y2, paint);
    }
}

public static class LineDrawOperationExtensions
{
    public static LineDrawOperation Line(this RenderCtx ctx, double x1, double y1, double x2, double y2) =>
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
}
