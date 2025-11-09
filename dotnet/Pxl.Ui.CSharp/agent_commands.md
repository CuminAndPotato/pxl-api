AGENT COMMAND: GEN_FLUENT

    - scan for extension classes (e.g. public static class LineDrawOperationExtensions)
    - remove this code
    - Scan all classes that inherit from IDirectDrawable
      (e.g. LineDrawOperation)
    - directly under their definition, create a new extension class
      (e.g. LineDrawOperationExtensions)
    - Search every property or field is decorated with [BuilderStyle],
      (e.g. public double X1 { get; set; } )
    - Generate an extension method for every found item
      (e.g. public static LineDrawOperation X1(this LineDrawOperation op, double x1) { op.X1 = x1; return op; } )

    ALSO:
    - When the class (e.g. LineDrawOperation) has a constructor with parameters,
      create a static method on the extension class with the same parameters that calls the constructor
      e.g.: public static LineDrawOperation DrawLine(this RenderCtx ctx, double x1, double y1, double x2, double y2) => ctx.BeginDirectDrawable(new(x1, y1, x2, y2));
    - The name of the method should be the same as the class name without the "DrawOperation" suffix.

    IMPORTANT:
    - You only change this file - no other files!
    - You only do exactly what is asked - no other things!
    - Stick to the examples as close as possible!
    - Don't be verbose - only a small summary of what you did.

