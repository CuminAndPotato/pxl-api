using Pxl.Ui.CSharp;
using static Pxl.Ui.CSharp.DrawingContext;

var scene = () =>
{
    Ctx.Background.Solid(Colors.Blue);

    var random = new Random();
    for (int i = 0; i < Ctx.Pixels.Length; i++)
    {
        Ctx.Pixels[i] = Color.FromRgb(
            (byte)random.Next(256),
            (byte)random.Next(256),
            (byte)random.Next(256));
    }

    Ctx.Text.Mono4x5("HELLO", 0, 10, Colors.Black);

    for (int i = 0; i < Ctx.Pixels.Length; i++)
    {
        var color = Ctx.Pixels[i];
        if (color.Red == 0 && color.Green == 0 && color.Blue == 0)
        {
            Ctx.Pixels[i] = Colors.Blue;
        }
    }
};


await PXL.Simulate(scene);
// await PXL.SendToDevice(scene, "192.168.178.52");
