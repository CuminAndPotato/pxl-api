// #:package Pxl@0.0.26
#:project ../../Pxl.Ui.CSharp

using Pxl.Ui.CSharp;
using SkiaSharp;
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

    // Spawn an empty layer for text with blur effect
    var textLayer = Ctx.Spawn(clearColor: Colors.TransparentBlack);
    
    // Draw "HELLO" on the spawned layer
    textLayer.Text.Mono4x5("HELLO", 0, 10, Colors.White);
    
    // Simple blur effect: sample neighboring pixels
    var textPixels = textLayer.Pixels;
    
    for (var y = 0; y < textPixels.Height; y++)
    {
        for (var x = 0; x < textPixels.Width; x++)
        {
            var r = 0;
            var g = 0;
            var b = 0;
            var a = 0;
            var count = 0;
            
            // Average 3x3 neighborhood
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    var nx = x + dx;
                    var ny = y + dy;
                    if (nx >= 0 && nx < textPixels.Width && ny >= 0 && ny < textPixels.Height)
                    {
                        var pixel = textPixels[nx, ny];
                        r += pixel.Red;
                        g += pixel.Green;
                        b += pixel.Blue;
                        a += pixel.Alpha;
                        count++;
                    }
                }
            }
            
            textPixels[x, y] = new SKColor(
                (byte)(r / count),
                (byte)(g / count),
                (byte)(b / count),
                (byte)(a / count));
        }
    }

    textLayer.Text.Mono4x5("HELLO", 0, 10, Colors.Black);

    // Apply the text layer back to main context with blend mode
    textLayer.Apply(0, 0, SKBlendMode.SrcOver);
};

await PXL.Simulate(scene);
