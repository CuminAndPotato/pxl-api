using System;
using Pxl.Ui.CSharp;
using SkiaSharp;
using static Pxl.Ui.CSharp.Drawing;

// Scene 1: Line demonstration - diagonal cross
var lineScene = () =>
{
    // Diagonal line from top-left to bottom-right
    Ctx.Line(0, 0, 24, 24).Stroke.Solid(Colors.Cyan);

    // Diagonal line from top-right to bottom-left
    Ctx.Line(24, 0, 0, 24).Stroke.Solid(Colors.Magenta);

    // Horizontal center line
    Ctx.Line(0, 12, 24, 12).Stroke.Solid(Colors.Yellow);

    // Vertical center line
    Ctx.Line(12, 0, 12, 24).Stroke.Solid(Colors.Lime);
};

// Scene 2: Rectangle demonstration
var rectScene = () =>
{
    // Outer rectangle with stroke only
    Ctx.RectXyWh(2, 2, 20, 20)
        .Stroke.Solid(Colors.Red, strokeWidth: 2);

    // Inner filled rectangle
    Ctx.RectXyWh(8, 8, 8, 8);
    Ctx.RectXyWh(8, 8, 8, 8).Fill.Solid(Colors.Blue);
    Ctx.RectXyWh(8, 8, 8, 8).Stroke.Solid(Colors.White);

    // Alternative way using DrawRect2 (x1, y1, x2, y2)
    Ctx.RectXyXy(4, 4, 10, 10).Fill.Solid(Colors.Green);
    Ctx.RectXyXy(4, 4, 10, 10).Stroke.Solid(Colors.Yellow);
};

// Scene 3: Circle demonstration
var circleScene = () =>
{
    // Large circle with stroke only
    Ctx.Circle(12, 12, 10)
        .Stroke.Solid(Colors.Cyan, strokeWidth: 2);

    // Medium filled circle
    Ctx.Circle(12, 12, 7).Fill.Solid(Colors.Magenta);
    Ctx.Circle(12, 12, 7).Stroke.Solid(Colors.Yellow);

    // Small center circle
    Ctx.Circle(12, 12, 3).Fill.Solid(Colors.White);
};

// Scene 4: Combined demonstration - face
var combinedScene = () =>
{
    // Face outline (circle)
    Ctx.Circle(12, 12, 10).Fill.Solid(Colors.Yellow);
    Ctx.Circle(12, 12, 10).Stroke.Solid(Colors.Black);

    // Left eye
    Ctx.Circle(9, 10, 1.5).Fill.Solid(Colors.Black);

    // Right eye
    Ctx.Circle(15, 10, 1.5).Fill.Solid(Colors.Black);

    // Smile (using a rectangle for simplicity)
    Ctx.RectXyWh(8, 15, 8, 2).Fill.Solid(Colors.Red);
    Ctx.RectXyWh(8, 15, 8, 2).Stroke.Solid(Colors.Black);
};

// Scene 5: Grid pattern
var gridScene = () =>
{
    // Vertical lines
    for (int x = 0; x <= 24; x += 4)
        Ctx.Line(x, 0, x, 24).Stroke.Solid(Colors.DarkGray);

    // Horizontal lines
    for (int y = 0; y <= 24; y += 4)
        Ctx.Line(0, y, 24, y).Stroke.Solid(Colors.DarkGray);

    // Corner markers
    Ctx.Circle(0, 0, 2).Fill.Solid(Colors.Red);
    Ctx.Circle(24, 0, 2).Fill.Solid(Colors.Green);
    Ctx.Circle(0, 24, 2).Fill.Solid(Colors.Blue);
    Ctx.Circle(24, 24, 2).Fill.Solid(Colors.Yellow);
};

// Scene 6: Linear Gradient demonstration
var linearGradientScene = () =>
{
    // Horizontal gradient rectangle
    Ctx.RectXyWh(2, 2, 20, 8).Fill.HorizontalGradient(20, Colors.Red, Colors.Yellow, Colors.Green);

    // Vertical gradient rectangle
    Ctx.RectXyWh(2, 14, 20, 8).Fill.VerticalGradient(8, Colors.Blue, Colors.Cyan, Colors.White);

    // Diagonal gradient circle
    Ctx.Circle(12, 12, 10).Fill.LinearGradient(
        new SKPoint(2, 2),
        new SKPoint(22, 22),
        [Colors.Magenta, Colors.Purple, Colors.Blue]
    );
};

// Scene 7: Radial Gradient demonstration
var radialGradientScene = () =>
{
    // Background rectangle with radial gradient
    Ctx.RectXyWh(0, 0, 24, 24).Fill.RadialGradient(
        new SKPoint(12, 12),
        12,
        [Colors.Yellow, Colors.Orange, Colors.Red, Colors.DarkRed]
    );

    // Overlapping circles with radial gradients
    Ctx.Circle(8, 8, 6).Fill.RadialGradient(
        new SKPoint(8, 8),
        6,
        [Colors.White, Colors.Cyan, Colors.Blue]
    );

    Ctx.Circle(16, 16, 6).Fill.RadialGradient(
        new SKPoint(16, 16),
        6,
        [Colors.White, Colors.Lime, Colors.Green]
    );
};

// Scene 8: Sweep Gradient demonstration
var sweepGradientScene = () =>
{
    // Rainbow sweep gradient circle
    Ctx.Circle(12, 12, 10).Fill.SweepGradient(
        new SKPoint(12, 12),
        [Colors.Red, Colors.Yellow, Colors.Green, Colors.Cyan, Colors.Blue, Colors.Magenta, Colors.Red]
    );

    // Small center circle
    Ctx.Circle(12, 12, 3).Fill.Solid(Colors.White);
};

// Scene 9: Mixed gradients
var mixedGradientsScene = () =>
{
    // Background with vertical gradient
    Ctx.RectXyWh(0, 0, 24, 24).Fill.VerticalGradient(24, Colors.DarkBlue, Colors.Blue, Colors.Cyan);

    // Sun with radial gradient
    Ctx.Circle(6, 6, 4).Fill.RadialGradient(
        new SKPoint(6, 6),
        4,
        [Colors.Yellow, Colors.Orange]
    );

    // Bottom rectangle with horizontal gradient
    Ctx.RectXyWh(0, 18, 24, 6).Fill.HorizontalGradient(24, Colors.Green, Colors.DarkGreen, Colors.Green);
};

// Scene 10: Gradient Smiley Face
var gradientSmileyScene = () =>
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
};

// Scene 11: PacMan Game Scene
var pacManScene = () =>
{
    // PacMan - yellow arc using center and radius (270 degrees, mouth open 45 degrees on each side)
    Ctx.ArcCenter(12, 12, 6, 45, 270).Fill.Solid(Colors.Yellow);
};

// Scene 12: Text demonstration
var textScene = () =>
{
    // Title with different fonts
    Ctx.Text().Mono6x6("FONTS", 0, 0).Brush.Solid(Colors.White);
    
    // Small fonts
    Ctx.Text().Var3x5("3x5", 0, 7).Brush.Solid(Colors.Cyan);
    Ctx.Text().Mono4x5("4x5", 0, 13).Brush.Solid(Colors.Yellow);
    
    // Medium font with gradient
    Ctx.Text().Mono6x6("Hi!", 0, 18).Brush.HorizontalGradient(18, Colors.Red, Colors.Orange, Colors.Yellow);
};

// Scene 13: Text with graphics combined
var textGraphicsScene = () =>
{
    // Score display
    Ctx.Text().Mono4x5("SCORE", 1, 1).Brush.Solid(Colors.White);
    Ctx.Text().Mono6x6("PXL", 1, 7)
        .SkewX(-0.2f)
        .Brush.Solid(Colors.Yellow)
        ;
    
    Ctx.Text().Var3x5("PAC", 12, 16).Brush.Solid(Colors.White);
    Ctx.Text().Var3x5("MAN", 12, 20).Brush.Solid(Colors.White);
};



// Try: 
// lineScene, rectScene, circleScene, combinedScene
// gridScene, linearGradientScene, radialGradientScene, sweepGradientScene, mixedGradientsScene
// gradientSmileyScene, pacManScene, textScene, textGraphicsScene
var currentScene = textGraphicsScene;


var host = "localhost";
// var host = "192.168.178.52";

Simulator.Start(host, currentScene);
