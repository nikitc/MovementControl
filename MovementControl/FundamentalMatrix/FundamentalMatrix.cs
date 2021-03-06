﻿using System;
using System.Collections.Generic;

namespace MovementControl.FundamentalMatrix
{
    public class FundamentalMatrix
    {
        public Matrix Matrix { get; set; }
        public double Time { get; set; }

        public FundamentalMatrix(Matrix matrix, double time)
        {
            Matrix = matrix;
            Time = time;
        }

        public Matrix GetFundamentalMatrix()
        {
            var eighers = new double[Matrix.Length];
            var wi = new double[Matrix.Length];
            var vl = new double[Matrix.Length, Matrix.Length];
            var vr = new double[Matrix.Length, Matrix.Length];
            alglib.evd.rmatrixevd(Matrix.GetTwoDemensionalMatrix(), Matrix.Length, 1, ref eighers, ref wi, ref vl, ref vr);
            var systemEquations = GetSystemEquation(eighers, Time);
            var coefficients = SystemSolution.CalcSolution(systemEquations.Equation, systemEquations.FreeMembers);

            return CalcFundamentalRow(coefficients);
        }

        private Matrix CalcFundamentalRow(double[] coefficients)
        {
            var fundamentalMatrix = coefficients[0] * Matrix.UnitMatrx(coefficients.Length);
            for (var i = 1; i < coefficients.Length; i++)
            {
                fundamentalMatrix += coefficients[i] * Matrix.Pow(i);
            }

            return fundamentalMatrix;
        }


        private Dictionary<double, int> GetMultiplicityValues(double[] eighbers)
        {
            var multiplicityDict = new Dictionary<double, int>();
            var isGetNumbers = new bool[eighbers.Length];
            for (var i = 0; i < eighbers.Length; i++)
            {
                if (isGetNumbers[i])
                    continue;

                var multiplicity = 1;
                for (var j = i + 1; j < eighbers.Length; j++)
                {
                    if (!isGetNumbers[j] && Math.Abs(eighbers[i] - eighbers[j]) < 0.00001)
                    {
                        multiplicity++;
                        isGetNumbers[j] = true;
                    }
                }
                multiplicityDict.Add(eighbers[i], multiplicity);
                isGetNumbers[i] = true;
            }

            return multiplicityDict;
        }

        public SystemEquation GetSystemEquation(double[] eighbers, double t)
        {
            var multiplicityDict = GetMultiplicityValues(eighbers);
            var systemEquation = new double[eighbers.Length][];
            var count = 0;
            var freeMembers = new List<double>();
            foreach (var eighNumber in multiplicityDict.Keys)
            {
                freeMembers.Add(Math.Pow(Math.E, eighNumber * t));
                var equationHead = new double[eighbers.Length];
                for (var i = 0; i < eighbers.Length; i++)
                    equationHead[i] = Math.Pow(eighNumber, i);

                systemEquation[count] = equationHead;
                count++;

                if (multiplicityDict[eighNumber] > 1)
                {
                    for (var i = 1; i < multiplicityDict[eighNumber]; i++)
                    {
                        freeMembers.Add(Math.Pow(t, i) * Math.Pow(Math.E, eighNumber * t));
                        var equationTail = new double[eighbers.Length];

                        for (var j = 0; j < i - 1; j++)
                            equationTail[j] = 0;

                        equationTail[i] = i;
                        for (var j = i + 1; j < eighbers.Length; j++)
                        {
                            equationTail[j] = j * Math.Pow(eighNumber, j - 1);
                        }

                        systemEquation[count] = equationTail;
                        count++;
                    }
                }
            }

            return new SystemEquation(eighbers.Length)
            {
                Equation = systemEquation,
                FreeMembers = freeMembers.ToArray()
            };
        }
    }   
}
