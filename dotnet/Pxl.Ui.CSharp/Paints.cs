// AI TOOL CALL: Read agent_commands.md carefully!

namespace Pxl.Ui.CSharp;

using Pxl.Ui.CSharp.Internal;
using SkiaSharp;

/// <summary>
/// Proxy for fluent paint definition. Use extension methods to configure paints.
/// </summary>
public class PaintProxy<TParent>(TParent parent, Func<SKPaint>? defaultFactory = null)
{
    private Func<SKPaint>? _factory = defaultFactory;

    internal void SetPaintFactory(Func<SKPaint> factory)
    {
        _factory = factory;
    }

    internal SKPaint? CreatePaint() => _factory?.Invoke();
    internal TParent Parent => parent;
}

/// <summary>
/// Extension methods for creating and configuring paints on PaintProxy.
/// </summary>
public static class PaintProxyExtensions
{
    /// <summary>
    /// Create a solid color paint.
    /// </summary>
    public static TParent Solid<TParent>(
        this PaintProxy<TParent> proxy,
        SKColor color,
        float strokeWidth = 1,
        bool isAntialias = false)
    {
        proxy.SetPaintFactory(() => new SKPaint
        {
            Color = color,
            StrokeWidth = strokeWidth,
            IsAntialias = isAntialias
        });
        return proxy.Parent;
    }

    /// <summary>
    /// Create a linear gradient from start point to end point.
    /// </summary>
    public static TParent LinearGradient<TParent>(
        this PaintProxy<TParent> proxy,
        (double x, double y) start,
        (double x, double y) end,
        SKColor[] colors,
        float[]? positions = null,
        SKShaderTileMode tileMode = SKShaderTileMode.Clamp,
        float strokeWidth = 1,
        bool isAntialias = false)
    {
        proxy.SetPaintFactory(() => new SKPaint
        {
            Shader = SKShader.CreateLinearGradient(start.ToSkiaPoint(), end.ToSkiaPoint(), colors, positions, tileMode),
            StrokeWidth = strokeWidth,
            IsAntialias = isAntialias
        });
        return proxy.Parent;
    }

    /// <summary>
    /// Create a radial gradient emanating from a center point.
    /// </summary>
    public static TParent RadialGradient<TParent>(
        this PaintProxy<TParent> proxy,
        (double x, double y) center,
        float radius,
        SKColor[] colors,
        float[]? positions = null,
        SKShaderTileMode tileMode = SKShaderTileMode.Clamp,
        float strokeWidth = 1,
        bool isAntialias = false)
    {
        proxy.SetPaintFactory(() => new SKPaint
        {
            Shader = SKShader.CreateRadialGradient(center.ToSkiaPoint(), radius, colors, positions, tileMode),
            StrokeWidth = strokeWidth,
            IsAntialias = isAntialias
        });
        return proxy.Parent;
    }

    /// <summary>
    /// Create a two-point conical gradient (cone gradient between two circles).
    /// </summary>
    public static TParent TwoPointConicalGradient<TParent>(
        this PaintProxy<TParent> proxy,
        (double x, double y) startCenter,
        float startRadius,
        (double x, double y) endCenter,
        float endRadius,
        SKColor[] colors,
        float[]? positions = null,
        SKShaderTileMode tileMode = SKShaderTileMode.Clamp,
        float strokeWidth = 1,
        bool isAntialias = false)
    {
        proxy.SetPaintFactory(() => new SKPaint
        {
            Shader = SKShader.CreateTwoPointConicalGradient(
                startCenter.ToSkiaPoint(), startRadius, endCenter.ToSkiaPoint(), endRadius, colors, positions, tileMode),
            StrokeWidth = strokeWidth,
            IsAntialias = isAntialias
        });
        return proxy.Parent;
    }

    /// <summary>
    /// Create a sweep (angular) gradient rotating around a center point.
    /// </summary>
    public static TParent SweepGradient<TParent>(
        this PaintProxy<TParent> proxy,
        (double x, double y) center,
        SKColor[] colors,
        float[]? positions = null,
        SKMatrix? localMatrix = null,
        float strokeWidth = 1,
        bool isAntialias = false)
    {
        proxy.SetPaintFactory(() => new SKPaint
        {
            Shader = localMatrix.HasValue
                ? SKShader.CreateSweepGradient(center.ToSkiaPoint(), colors, positions, localMatrix.Value)
                : SKShader.CreateSweepGradient(center.ToSkiaPoint(), colors, positions),
            StrokeWidth = strokeWidth,
            IsAntialias = isAntialias
        });
        return proxy.Parent;
    }

    /// <summary>
    /// Create a Perlin noise fractal pattern.
    /// </summary>
    public static TParent PerlinNoiseFractal<TParent>(
        this PaintProxy<TParent> proxy,
        float baseFrequencyX,
        float baseFrequencyY,
        int numOctaves,
        float seed,
        float strokeWidth = 1,
        bool isAntialias = false)
    {
        proxy.SetPaintFactory(() => new SKPaint
        {
            Shader = SKShader.CreatePerlinNoiseFractalNoise(baseFrequencyX, baseFrequencyY, numOctaves, seed),
            StrokeWidth = strokeWidth,
            IsAntialias = isAntialias
        });
        return proxy.Parent;
    }

    /// <summary>
    /// Create a Perlin noise turbulence pattern.
    /// </summary>
    public static TParent PerlinNoiseTurbulence<TParent>(
        this PaintProxy<TParent> proxy,
        float baseFrequencyX,
        float baseFrequencyY,
        int numOctaves,
        float seed,
        float strokeWidth = 1,
        bool isAntialias = false)
    {
        proxy.SetPaintFactory(() => new SKPaint
        {
            Shader = SKShader.CreatePerlinNoiseTurbulence(baseFrequencyX, baseFrequencyY, numOctaves, seed),
            StrokeWidth = strokeWidth,
            IsAntialias = isAntialias
        });
        return proxy.Parent;
    }

    /// <summary>
    /// Create an image-based pattern/texture.
    /// </summary>
    public static TParent ImagePattern<TParent>(
        this PaintProxy<TParent> proxy,
        SKImage image,
        SKShaderTileMode tileX = SKShaderTileMode.Repeat,
        SKShaderTileMode tileY = SKShaderTileMode.Repeat,
        SKMatrix? localMatrix = null,
        float strokeWidth = 1,
        bool isAntialias = false)
    {
        var matrix = localMatrix ?? SKMatrix.Identity;
        proxy.SetPaintFactory(() => new SKPaint
        {
            Shader = SKShader.CreateImage(image, tileX, tileY, matrix),
            StrokeWidth = strokeWidth,
            IsAntialias = isAntialias
        });
        return proxy.Parent;
    }

    /// <summary>
    /// Use a custom SKShader.
    /// </summary>
    public static TParent CustomShader<TParent>(
        this PaintProxy<TParent> proxy, 
        SKShader shader, 
        float strokeWidth = 1,
        bool isAntialias = false)
    {
        proxy.SetPaintFactory(() => new SKPaint
        {
            Shader = shader,
            StrokeWidth = strokeWidth,
            IsAntialias = isAntialias
        });
        return proxy.Parent;
    }

    /// <summary>
    /// Create a linear gradient from top to bottom.
    /// </summary>
    public static TParent VerticalGradient<TParent>(this PaintProxy<TParent> proxy, float height, params SKColor[] colors)
    {
        return proxy.LinearGradient(
            (0, 0),
            (0, height),
            colors
        );
    }

    /// <summary>
    /// Create a linear gradient from left to right.
    /// </summary>
    public static TParent HorizontalGradient<TParent>(this PaintProxy<TParent> proxy, float width, params SKColor[] colors)
    {
        return proxy.LinearGradient(
            (0, 0),
            (width, 0),
            colors
        );
    }
}
