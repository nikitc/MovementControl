using System;
using System.Collections.Generic;

namespace MovementControl.Examples
{
    public class Example1 : IMovementControlContidion
    {
        public List<Func<double, double>> Control
        {
            get => new List<Func<double, double>>
            {
                x => Math.Cos(x) - 3,
                Math.Cos,
                x => Math.Sin(x) + 2,
                Math.Sin
            };
            set {  }
        }

        public Matrix StartVector
        {
            get => new Matrix(new[]
            {
                new double[] {-1},
                new double[] {2},
                new double[] {4},
                new double[] {5}
            });
            set { }
        }

        public Matrix GetFundamentalMatrix(double ta, double tb)
        {
            var fundamentalMatrix = new[]
            {
                new [] {1, tb - ta, 0, 0},
                new double[] {0, 1, 0, 0},
                new [] {0, 0, 1, tb - ta},
                new double[] {0, 0, 0, 1},
            };

            return new Matrix(fundamentalMatrix);
        }
    }
}