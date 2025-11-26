#:package Pxl@0.0.24

using Pxl.Ui.CSharp;
using SkiaSharp;
using static Pxl.Ui.CSharp.Drawing;


var scene = () =>
{
    // Background with radial gradient (light to dark)
    Ctx.RectXyWh(0, 0, 24, 24).Fill.RadialGradient(
        new SKPoint(12, 12),
        17,
        [Colors.LightBlue, Colors.DarkBlue]
    );

    // Face with radial gradient (yellow to orange)
    Ctx.Circle(12, 12, 10).Fill.RadialGradient(
        new SKPoint(10, 10),
        12,
        [Colors.Yellow, Colors.Gold, Colors.Orange]
    );

    // Face outline
    Ctx.Circle(12, 12, 10).Stroke.Solid(Colors.DarkOrange, strokeWidth: 1);

    // Left eye with gradient
    Ctx.Circle(9, 10, 1.5).Fill.RadialGradient(
        new SKPoint(9, 10),
        1.5f,
        [Colors.White, Colors.Blue, Colors.Black]
    );

    // Right eye with gradient
    Ctx.Circle(15, 10, 1.5).Fill.RadialGradient(
        new SKPoint(15, 10),
        1.5f,
        [Colors.White, Colors.Blue, Colors.Black]
    );

    // Smile with gradient (red to dark red)
    Ctx.RectXyWh(8, 15, 8, 2.5).Fill.VerticalGradient(
        2.5f,
        Colors.Red, Colors.DarkRed
    );

    // Smile outline
    Ctx.RectXyWh(8, 15, 8, 2.5).Stroke.Solid(Colors.Maroon);

    // Rosy cheeks with radial gradients
    Ctx.Circle(7, 13, 1.5).Fill.RadialGradient(
        new SKPoint(7, 13),
        1.5f,
        [Colors.Pink, Colors.LightPink, new SKColor(255, 192, 203, 0)]
    );

    Ctx.Circle(17, 13, 1.5).Fill.RadialGradient(
        new SKPoint(17, 13),
        1.5f,
        [Colors.Pink, Colors.LightPink, new SKColor(255, 192, 203, 0)]
    );

    // Display seconds
    var seconds = DateTime.Now.Second;
    Ctx.Text().Mono4x5(Ctx.Now.Second.ToString(), 4, 4).Brush.Solid(Colors.Black);
};

await Simulator.Run("localhost", true, scene);
