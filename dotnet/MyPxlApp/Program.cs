using System;
using PxlClock;
using static PxlClock.Drawing;

var myScene = () =>
{
    Ctx.DrawLine(0, 0, Ctx.width, Ctx.height);
};

Simulator.Start("localhost", myScene);
