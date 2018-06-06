using System;
using System.Collections.Generic;

namespace MovementControl.Examples
{
    public class MovementControlCondition : IMovementControlCondition
    {
        public List<Func<double, double>> Control { get; set; }
        public Matrix StartVector { get; set; }
        public Matrix ConstantCoefficientsMatrix { get; set; }

        public MovementControlCondition(List<Func<double, double>> control, Matrix startVector,
            Matrix constantCoefficientsMatrix)
        {
            Control = control;
            StartVector = startVector;
            ConstantCoefficientsMatrix = constantCoefficientsMatrix;
        }
    }
}