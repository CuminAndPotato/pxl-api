namespace Pxl.Ui.CSharp;

using Pxl;

public partial class DrawingContext
{
    public PaintProxy<RectDrawOperation> Background =>
        RenderCtx
            .BeginDirectDrawable(new RectDrawOperation { X = 0, Y = 0, Width = RenderCtx.Width, Height = RenderCtx.Height })
            .Fill;
}
