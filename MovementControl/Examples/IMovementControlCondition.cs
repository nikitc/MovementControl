using System;
using System.Collections.Generic;

namespace MovementControl.Examples
{
    public interface IMovementControlCondition
    {
        List<Func<double, double>> Control { get; set; }
        Matrix StartVector { get; set; }
        Matrix ConstantCoefficientsMatrix { get; set; }
    }
}