using System;
using System.Collections.Generic;

namespace MovementControl
{
    public static class IntegralCalculator
    {
        public static Matrix RightRectanglesFormula(double start, double end, 
                                                    double step, List<Func<double, double>> funcs)
        {
            var startv = new []
            {
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0}
            };
            var temp = new[]
            {
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0}
            };
            var tempVector = new Matrix(temp);
            var result = new Matrix(startv);
            var m = (end - start) / step;
            for (var i = 1; i < m + 1; i++)
            {
                tempVector.Clear();
                for (var j = 0; j < result.RowsCount; j++)
                {
                    var a = funcs[j](start + i * step);
                    tempVector[j, 0] = funcs[j](start + i * step);
                }

                var e = GetMatrix1(i, end) * tempVector;
                result += GetMatrix1(i, end) * tempVector;
            }

            return step * result;
        }

        private static Matrix GetMatrix1(double ta, double tb)
        {
            var X = new[]
            {
                new double[] {1, tb - ta, 0, 0},
                new double[] {0, 1, 0, 0},
                new double[] {0, 0, 1, tb - ta},
                new double[] {0, 0, 0, 1},
            };

            return new Matrix(X);
        }
    }
}