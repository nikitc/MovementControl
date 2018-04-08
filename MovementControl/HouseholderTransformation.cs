using System;

namespace MovementControl
{
    public static class HouseholderTransformation
    {
        public static void ReflectHouseHolderMatrix(this Matrix householderMatrix, double[] vector, double num)
        {
            for (var i = 0; i < householderMatrix.RowsCount; i++)
            {
                for (var j = 0; j < householderMatrix.Length; j++)
                {
                    householderMatrix[i, j] -= 2 * vector[i] * vector[j] / num;
                }
            }
        }

        public static void Transform()
        {
            var A = new Matrix(new[]
            {
                new double[] {5, 6, 3},
                new double[] {-1, 0, 1},
                new double[] {1, 2, -1}
            });
            var householderMatrix = Matrix.UnitMatrx(3);
            var Q = new Matrix(new[]
            {
                new double[] {1, 0, 0},
                new double[] {0, 1, 0},
                new double[] {0, 0, 1}
            });
            var V = new double[3];
            var eps = 0.0000001;
            var ex = true;

            while (ex)
            {
                V[0] = A[0, 0] + Sign(A[0, 0]) * Math.Pow(A[0, 0] * A[0, 0] + A[1, 0] * A[1, 0] + A[2, 0] * A[2, 0], 0.5);
                V[1] = A[1, 0];
                V[2] = A[2, 0];
                var num = V[0] * V[0] + V[1] * V[1] + V[2] * V[2];
                householderMatrix.ReflectHouseHolderMatrix(V, num);

                Q = householderMatrix;
                A = householderMatrix * A;
                householderMatrix = Matrix.UnitMatrx(3);

                // вторая итерация разложения
                V[0] = 0;
                V[1] = A[1, 1] + Sign(A[1, 1]) * Math.Pow(A[1, 1] * A[1, 1] + A[2, 1] * A[2, 1], 0.5);
                V[2] = A[2, 1];
                num = V[1] * V[1] + V[2] * V[2];
                householderMatrix.ReflectHouseHolderMatrix(V, num);

                Q = Q * householderMatrix;
                A = householderMatrix * A * Q;

                householderMatrix = Matrix.UnitMatrx(3);

                var a = Math.Pow(A[1, 0] * A[1, 0] + A[2, 0] * A[2, 0], 0.5);
                var b = Math.Abs(A[2, 1]);
                if (a < eps && b < eps)
                    ex = false;
            }
            Console.WriteLine(A[0, 0]);
            Console.WriteLine(A[1, 1]);
            Console.WriteLine(A[2, 2]);
        }

        public static int Sign(double value)
        {
            if (Math.Abs(value) < 0.00001)
                return 0;

            return value > 0 ? 1 : -1;
        }
    }
}