using MovementControl.Examples;

namespace MovementControl
{
    public static class IntegralCalculator
    {
        public static Matrix RightRectanglesFormula(double start, double end, 
                                                    double step, IMovementControlCondition condition)
        {
            var result = new Matrix(new []
            {
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0}
            });
            var tempVector = new Matrix(new[]
            {
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0}
            });
            var stepsCount = (end - start) / step;
            for (var i = 1; i < stepsCount + 1; i++)
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