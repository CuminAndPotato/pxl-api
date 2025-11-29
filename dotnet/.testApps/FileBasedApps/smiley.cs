// #:package Pxl@0.0.25
#:project ../../Pxl.Ui.CSharp


using Pxl.Ui.CSharp;
using static Pxl.Ui.CSharp.Drawing;


var scene = () =>
{
    // Background with radial gradient (light to dark)
    Ctx.RectXyWh(0, 0, 24, 24).Fill.RadialGradient(
        (12, 12),
        17,
        [Color.LightBlue, Color.DarkBlue]
    );

    // Face with radial gradient (yellow to orange)
    Ctx.Circle(12, 12, 10).Fill.RadialGradient(
        (10, 10),
        12,
        [Color.Yellow, Color.Gold, Color.Orange]
    );

    // Face outline
    Ctx.Circle(12, 12, 10).Stroke.Solid(Color.DarkOrange, strokeWidth: 1);

    // Left eye with gradient
    Ctx.Circle(9, 10, 1.5).Fill.RadialGradient(
        (9, 10),
        1.5f,
        [Color.White, Color.Blue, Color.Black]
    );

    // Right eye with gradient
    Ctx.Circle(15, 10, 1.5).Fill.RadialGradient(
        (15, 10),
        1.5f,
        [Color.White, Color.Blue, Color.Black]
    );

    // Smile with gradient (red to dark red)
    Ctx.RectXyWh(8, 15, 8, 2.5).Fill.VerticalGradient(
        2.5f,
        Color.Red, Color.DarkRed
    );

    // Smile outline
    Ctx.RectXyWh(8, 15, 8, 2.5).Stroke.Solid(Color.Maroon);

    // Rosy cheeks with radial gradients
    Ctx.Circle(7, 13, 1.5).Fill.RadialGradient(
        (7, 13),
        1.5f,
        [Color.Pink, Color.LightPink, Color.FromRgba(255, 192, 203, 0)]
    );

    Ctx.Circle(17, 13, 1.5).Fill.RadialGradient(
        (17, 13),
        1.5f,
        [Color.Pink, Color.LightPink, Color.FromRgba(255, 192, 203, 0)]
    );

    // Display seconds
    var seconds = DateTime.Now.Second;
    Ctx.Text().Mono4x5(Ctx.Now.Second.ToString(), 4, 4).Brush.Solid(Color.Black);
};

await PXL.Run("localhost", true, true, scene);
