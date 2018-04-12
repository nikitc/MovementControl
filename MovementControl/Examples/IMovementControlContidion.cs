using System;
using System.Collections.Generic;

namespace MovementControl.Examples
{
    public interface IMovementControlContidion
    {
        List<Func<double, double>> Control { get; set; }
        Matrix StartVector { get; set; }
        Matrix GetFundamentalMatrix(double ta, double tb);
    }
}