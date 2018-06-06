using MovementControl.Examples;

namespace MovementControl
{
    public static class IntegralCalculator
    {
        public static Matrix RightRectanglesFormula(double start, double end, 
                                                    double step, IMovementControlCondition condition)
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
                    tempVector[j, 0] = condition.Control[j](start + i * step);
                }

                var different = end - (start + i * step); 
                var fundamentalMatrix = new FundamentalMatrix.FundamentalMatrix(condition.ConstantCoefficientsMatrix, different).GetFundamentalMatrix();
                result += step * fundamentalMatrix * tempVector;
            }

            return result;
        }
    }
}