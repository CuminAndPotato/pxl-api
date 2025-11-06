using System;
using Pxl;
using Pxl.Ui;
using static Pxl.PxlLocalDev;
using static Pxl.Ui.PxlClock;

var myScene = () =>
{
    Ctx.DrawLine(0, 0, Ctx.width, Ctx.height);
};

Simulator.startWith("localhost", myScene);
