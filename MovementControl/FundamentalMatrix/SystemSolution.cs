namespace MovementControl.FundamentalMatrix
{
    public static class SystemSolution
    {
        public static double[] CalcSolution(double[][] systemEquations, double[] freeMembers)
        {
            var n = systemEquations.Length;
            var solution = new double[n];

            for (var k = 0; k < n - 1; k++)
            {
                for (var i = k + 1; i < n; i++)
                {
                    for (var j = k + 1; j < n; j++)
                    {
                        systemEquations[i][j] = systemEquations[i][j] - systemEquations[k][j]
                                                * (systemEquations[i][k] / systemEquations[k][k]);
                    }
                    freeMembers[i] = freeMembers[i] - freeMembers[k] * systemEquations[i][k] / systemEquations[k][k];
                }
            }

            for (var k = n - 1; k >= 0; k--)
            {
                var sum = 0.0;
                for (var j = k + 1; j < n; j++)
                    sum += systemEquations[k][j] * solution[j];
                solution[k] = (freeMembers[k] - sum) / systemEquations[k][k];
            }

            return solution;
        }
    }
}