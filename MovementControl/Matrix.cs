using System;
using System.Collections.Generic;
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

        public Matrix Transpose()
        {
            for (var layer = 0; layer < Length / 2; ++layer)
            {
                var first = layer;
                var last = Length - 1 - layer;
                for (var j = first; j < last; j++)
                {
                    var offset = j - first;
                    var top = this[first, j];
                    this[first, j] = this[last - offset, first];
                    this[last - offset, first] = this[last, last - offset];
                    this[last, last - offset] = this[j, last];
                    this[j, last] = top;
                }
            }

            for (var layer = 0; layer < Length / 2; layer++)
            {
                var first = layer;
                var last = Length - 1 - layer;
                for (var i = layer; i < Length; i++)
                {
                    var offset = layer - first;
                    var temp = this[i, offset];
                    this[i, offset] = this[i, last - offset];
                    this[i, last - offset] = temp;
                }
            }

            return this;
        }

        public Matrix Inverse()
        {
            var determinant = GetDeterminant();

            return 1 / determinant * MinorMatrix()
                   .ComplementsAlgebraicMatrix()
                   .Transpose();
        }

        public Matrix ComplementsAlgebraicMatrix()
        {
            for (var i = 0; i < Length; i++)
            {
                for (var j = 0; j < Length; j++)
                {
                    if ((j + i) % 2 == 1)
                        this[i, j] *= -1;
                }
            }

            return this;
        }

        public Matrix MinorMatrix()
        {
            var newArray = new double[Length][];
            for (var i = 0; i < Length; i++)
            {
                newArray[i] = new double[Length];
                for (var j = 0; j < Length; j++)
                {
                    newArray[i][j] = GetMinor(j, i);
                }
            }

            return new Matrix(newArray);
        }

        public double GetMinor(int k, int l)
        {
            var matrix = new double[Length - 1][];
            for (var i = 0; i < Length - 1; i++)
            {
                if (matrix[i] == null)
                    matrix[i] = new double[Length - 1];

                for (var j = 0; j < Length - 1; j++)
                {
                    var firstIndex = i >= l ? i + 1 : i;
                    var secondIndex = j >= k ? j + 1 : j;
                    matrix[i][j] = this[firstIndex, secondIndex];
                }
            }

            return new Matrix(matrix).GetDeterminant();
        }

        public double GetDeterminant()
        {
            var determinant = 0.0;
            var n = Length;
            var permutations = GetPermutations(n);
            foreach (var permutation in permutations)
            {
                var countInvrsion = GetCountInversion(permutation);
                determinant += (countInvrsion % 2 == 0 ? 1 : -1) * permutation
                                   .Select((elem, index) => this[index, elem - 1])
                                   .Aggregate((x, y) => x * y);
            }

            return determinant;
        }

        private int GetCountInversion(List<int> set)
        {
            var countInversion = 0;

            for (var i = 0; i < set.Count; i++)
                for (var j = i + 1; j < set.Count; j++)
                    if (set[i] > set[j])
                        countInversion++;

            return countInversion;
        }

        private List<List<int>> GetPermutations(int n)
        {
            var set = new List<int>();
            for (var i = 1; i <= n; i++)
                set.Add(i);

            return GetPermutations(set);
        }

        private List<List<int>> GetPermutations(List<int> set)
        {
            var permutations = new List<List<int>>();
            if (!set.Any())
            {
                permutations.Add(new List<int>());
                return permutations;
            }

            var firstElement = set.First();
            var newList = set.GetRange(1, set.Count - 1);
            var perms = GetPermutations(newList);
            foreach (var perm in perms)
            {
                for (var i = 0; i <= perm.Count; i++)
                {
                    var newList2 = perm.GetRange(0, perm.Count);
                    newList2.Insert(i, firstElement);
                    permutations.Add(newList2);
                }
            }

            return permutations;
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
                throw new NotImplementedException();
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
    }
}