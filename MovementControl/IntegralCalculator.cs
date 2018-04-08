using System;

namespace MovementControl
{
    public static class IntegralCalculator
    {
        public static double RightRectanglesFormula(int start, int end, double step, Func<double, double> func)
        {
            double result = 0;
            var m = (end - start) / step;
            for (var i = 1; i < m + 1; i++)
            {
                result += func(start + i * step);
            }

            return result * step;
        }
    }
}