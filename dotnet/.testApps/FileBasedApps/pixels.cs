// #:package Pxl@0.0.26
#:project ../../Pxl.Ui.CSharp


using Pxl.Ui.CSharp;
using static Pxl.Ui.CSharp.Drawing;


var scene = () =>
{
    Ctx.Background().Solid(Color.Blue);

    var pixels = Ctx.Pixels();

    var random = new Random();
    for (int i = 0; i < pixels.Length; i++)
    {
        pixels[i] = Color.FromRgb(
            (byte)random.Next(256),
            (byte)random.Next(256),
            (byte)random.Next(256));
    }

    Ctx.Text().Mono4x5("HELLO", 0, 10, Color.Black);

    for (int i = 0; i < pixels.Length; i++)
    {
        var color = pixels[i];
        if (color.Red == 0 && color.Green == 0 && color.Blue == 0)
        {
            pixels[i] = Color.Blue;
        }
    }
    
};

await PXL.Run("localhost", true, true, scene);
