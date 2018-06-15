using System;
using System.Linq;

namespace MovementControl
{
    public class Matrix
    {
        private double[][] Elements { get; }

        public int Length => Elements[0].Length;
        public int RowsCount => Elements.Length;

        public Matrix(double[][] matrix)
        {
            Elements = matrix;
        }

        public Matrix(int n, int k)
        {
            Elements = new double[n][];
            for (var i = 0; i < n; i++)
            {
                Elements[i] = new double[k];
            }
        }

        public double this[int i, int j]
        {
            get => Elements[i][j];
            set => Elements[i][j] = value;
        }
        
        public static Matrix operator *(double value, Matrix matrix)
        {
            for (var i = 0; i < matrix.Length; i++)
            {
                for (var j = 0; j < matrix.Length; j++)
                {
                    matrix[i, j] = matrix[i, j] * value;
                }
            }

            return matrix;
        }

        public static Matrix operator *(Matrix firstMatrix, Matrix secondMatrix)
        {
            var newMatrix = new Matrix(firstMatrix.RowsCount, secondMatrix.Length);

            for (var i = 0; i < newMatrix.RowsCount; i++)
            {
                for (var j = 0; j < newMatrix.Length; j++)
                {
                    for (var k = 0; k < firstMatrix.Length; k++)
                        newMatrix[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }

            return newMatrix;
        }

        public static Matrix operator +(Matrix firstMatrix, Matrix secondMatrix)
        {
            for (var i = 0; i < firstMatrix.RowsCount; i++)
            {
                for (var j = 0; j < firstMatrix.Length; j++)
                {
                    firstMatrix[i, j] += secondMatrix[i, j];
                }
            }

            return firstMatrix;
        }

        public Matrix Clear()
        {
            for (var i = 0; i < RowsCount; i++)
            {
                for (var j = 0; j < Length; j++)
                {
                    this[i, j] = 0;
                }
            }

            return this;
        }

        public static Matrix UnitMatrx(int n)
        {
            var unitMatrix = new Matrix(n, n);
            for (var i = 0; i < n; i++)
            {
                unitMatrix[i, i] = 1;
            }

            return unitMatrix;
        }

        public void CopyTo(Matrix destMatrix)
        {
            if (destMatrix.RowsCount != RowsCount || destMatrix.Length != Length)
            {
                throw new IndexOutOfRangeException();
            }

            for (var i = 0; i < RowsCount; i++)
            {
                for (var j = 0; j < Length; j++)
                {
                    destMatrix[i, j] = this[i, j];
                }
            }
        }

        public double[,] GetTwoDemensionalMatrix()
        {
            var twoDemensionalMatrix = new double[Length, Length];

            for (var i = 0; i < Length; i++)
            {
                for (var j = 0; j < Length; j++)
                {
                    twoDemensionalMatrix[i, j] = this[i, j];
                }
            }

            return twoDemensionalMatrix;
        }

        public override string ToString()
        {
            return string.Join("\n", Elements.Select(line => string.Join(" ", line)));
        }

        public Matrix Pow(int pow)
        {
            var result = new Matrix(Length, Length);
            CopyTo(result);
            for (var i = 1; i < pow; i++)
                result = result * this;

            return result;
        }
    }
}