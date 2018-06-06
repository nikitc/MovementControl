using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Mathos.Parser;
using MovementControl.Draw;
using MovementControl.Examples;

namespace MovementControl
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += BuildGraphic;
        }

        private void BuildGraphic(object sender, RoutedEventArgs e)
        {
            var count = int.Parse(countPoints.Text);
            var control = GetControl();
            var constantCoefficientsMatrix = GetConstantCoefficientsMatrix();
            var startVector = GetStartVector();

            var drawer = new Drawer(MovementGraph);
            var condition = new MovementControlCondition(control, startVector, constantCoefficientsMatrix);
            drawer.DrawFunction(count, BuildMovementControl, condition);
        }

        private List<Matrix> BuildMovementControl(int count, IMovementControlCondition condition)
        {
            var result = new List<Matrix>{ condition.StartVector };
            for (var i = 0; i < count; i++)
            {
                var fundamentalMatrix = new FundamentalMatrix.FundamentalMatrix(condition.ConstantCoefficientsMatrix, 1).GetFundamentalMatrix();
                var current =  fundamentalMatrix * result.Last()
                                    + CalcIntegral(i, i + 1, condition);
                result.Add(current);
            }

            return result;
        }

        private Matrix CalcIntegral(double t1, double t2, IMovementControlCondition condition)
        {
            return IntegralCalculator.RightRectanglesFormula(t1, t2, 0.1, condition);
        }


        private double ParseCoefficient(string expression, string variable, List<string> variables)
        {
            var parser = new MathParser();
            var variablePosition = expression.IndexOf(variable, StringComparison.Ordinal);
            if (variablePosition == 0)
                return 1;

            var leftVariables = variables
                .Where(x => x != variable && expression.Contains(x) && variablePosition - expression.IndexOf(x, StringComparison.Ordinal) > 0)
                .OrderBy(x => variablePosition - expression.IndexOf(x, StringComparison.Ordinal));

            if (!leftVariables.Any())
            {
                return (double)parser.Parse(expression.Substring(0, expression.IndexOf(variable, StringComparison.Ordinal)));
            }

            var leftVariable = leftVariables.First();
            var leftVariablePosition = expression.IndexOf(leftVariable, StringComparison.Ordinal);

            var str = leftVariablePosition < 0
                ? expression.Substring(0, expression.IndexOf(variable, StringComparison.Ordinal))
                : expression.Substring(leftVariablePosition + 2, variablePosition - leftVariablePosition - 2);

            if (str.Length == 1)
            {
                return 1;
            }

            return (double)parser.Parse(str);
        }

        private Matrix SetCurrentExpression(string expression, Matrix constantCoefficientMatrix, int number, List<string> variables)
        {
            for (var i = 0; i < variables.Count; i++)
            {
                if (!expression.Contains(variables[i]))
                {
                    constantCoefficientMatrix[number, i] = 0;
                }
                else
                {
                    constantCoefficientMatrix[number, i] = ParseCoefficient(expression, variables[i], variables);
                }

            }

            return constantCoefficientMatrix;
        }

        private Matrix GetConstantCoefficientsMatrix()
        {
            var countVariables = 4;
            var expressions = new [] { systemX1.Text, systemX2.Text, systemX3.Text, systemX4.Text };
            var constantCoefficientMatrix = new Matrix(4, 4);
            var variables = new List<string>{"x1", "x2", "x3", "x4"};
            for (var i = 0; i < countVariables; i++)
            {
                var currentExpression = expressions[i];
                constantCoefficientMatrix =
                    SetCurrentExpression(currentExpression, constantCoefficientMatrix, i, variables);
            }

            return constantCoefficientMatrix;
            /*
            return new Matrix(new[]
            {
                new double[] {0, 1, 0, 0},
                new double[] {0, 0, 0, 0},
                new double[] {0, 0, 0, 1},
                new double[] {0, 0, 0, 0}
            });*/
        }

        private List<Func<double, double>> GetControl()
        {
            return new List<Func<double, double>>
            {
                t => 0,
                t => Math.Cos(t),
                t => 0,
                t => Math.Sin(t)
            };
        }

        private Matrix GetStartVector()
        {
            var x1 = double.Parse(startVectorX1.Text);
            var x2 = double.Parse(startVectorX2.Text);
            var x3 = double.Parse(startVectorX3.Text);
            var x4 = double.Parse(startVectorX4.Text);

            return new Matrix(new[]
            {
                new [] {x1},
                new [] {x2},
                new [] {x3},
                new [] {x4}
            });
        }
    }
}