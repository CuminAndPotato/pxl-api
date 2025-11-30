// #:package Pxl@0.0.26
#:project ../../Pxl.Ui.CSharp


using Pxl.Ui.CSharp;
using static Pxl.Ui.CSharp.DrawingContext;


var scene = () =>
{
    Ctx.Background.Solid(Colors.Blue);

    var pixels = Ctx.Pixels;

    var random = new Random();
    for (int i = 0; i < pixels.Length; i++)
    {
        pixels[i] = Color.FromRgb(
            (byte)random.Next(256),
            (byte)random.Next(256),
            (byte)random.Next(256));
    }

    Ctx.Text.Mono4x5("HELLO", 0, 10, Colors.Black);

    for (int i = 0; i < pixels.Length; i++)
    {
        var color = pixels[i];
        if (color.Red == 0 && color.Green == 0 && color.Blue == 0)
            pixels[i] = Colors.Blue;
    }
    
};

// await PXL.Run("192.168.178.52", true, true, scene);
await PXL.Run("192.168.178.52", false, false, scene);
