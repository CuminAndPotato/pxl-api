using System;
using Pxl.Ui.CSharp;
using static Pxl.Ui.CSharp.Drawing;

var myScene = () =>
{
    Ctx.DrawLine(0, 0, Ctx.width, Ctx.height);
};

Simulator.Start("localhost", myScene);
