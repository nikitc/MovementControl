using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        public int GetTime(TextBox time)
        {
            time.BorderBrush = Brushes.DarkGray;
            if (!int.TryParse(time.Text, out var currentTime))
            {
                time.BorderBrush = Brushes.Red;
                throw new FormatException();
            }

            return currentTime;
        }

        private bool isCorrectInterval(int time1, int time2)
        {
            return time1 < time2;
        }

        private void BuildGraphic(object sender, RoutedEventArgs e)
        {
            errorLabel.Visibility = Visibility.Hidden;
            intervalError.Visibility = Visibility.Hidden;

            try
            {
                var time1 = GetTime(t1);
                var time2 = GetTime(t2);
                if (!isCorrectInterval(time1, time2))
                {
                    intervalError.Visibility = Visibility.Visible;
                    return;
                }

                var control = GetControl();
                var constantCoefficientsMatrix = GetConstantCoefficientsMatrix();
                var startVector = GetStartVector();

                var drawer = new Drawer(MovementGraph);
                var condition = new MovementControlCondition(control, startVector, constantCoefficientsMatrix);
                drawer.DrawFunction(time1, time2, BuildMovementControl, condition);
            }
            catch (Exception)
            {
                errorLabel.Visibility = Visibility.Visible;
            }
        }

        private List<Matrix> BuildMovementControl(int time1, int time2, IMovementControlCondition condition)
        {
            var result = new List<Matrix> { condition.StartVector };
            for (var i = time1; i < time2; i++)
            {
                var fundamentalMatrix = new FundamentalMatrix.FundamentalMatrix(condition.ConstantCoefficientsMatrix, 1).GetFundamentalMatrix();
                var current = fundamentalMatrix * result.Last()
                                    + CalcIntegral(i, i + 1, condition);
                result.Add(current);
            }

            return result;
        }

        private Matrix CalcIntegral(double time1, double time2, IMovementControlCondition condition)
        {
            return IntegralCalculator.RightRectanglesFormula(time1, time2, 0.1, condition);
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
            var expressions = new[] { systemX1.Text, systemX2.Text, systemX3.Text, systemX4.Text };
            var constantCoefficientMatrix = new Matrix(4, 4);
            var variables = new List<string> { "x1", "x2", "x3", "x4" };
            for (var i = 0; i < countVariables; i++)
            {
                systemX1.BorderBrush = Brushes.DarkGray;
                try
                {
                    var currentExpression = expressions[i];
                    constantCoefficientMatrix =
                        SetCurrentExpression(currentExpression, constantCoefficientMatrix, i, variables);
                }
                catch (FormatException)
                {
                    systemX1.BorderBrush = Brushes.Red;
                    throw;
                }
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
            var controlTextBoxes = new[] { controlU1, controlU2, controlU3, controlU4 };
            var parser = new MathParser();

            return controlTextBoxes
                .Select(currentControl =>
                {
                    currentControl.BorderBrush = Brushes.DarkGray;
                    Func<double, double> currenControl = t =>
                    {
                        if (string.IsNullOrEmpty(currentControl.Text))
                            return 0;

                        try
                        {
                            var calcControlValue = currentControl.Text.Replace("t", t.ToString());
                            return (double)parser.Parse(calcControlValue);
                        }
                        catch (FormatException)
                        {
                            currentControl.BorderBrush = Brushes.Red;
                            throw;
                        }
                    };

                    return currenControl;
                })
                .ToList();
        }

        private Matrix GetStartVector()
        {
            var vectorTextBoxes = new List<TextBox> { startVectorX1, startVectorX2, startVectorX3, startVectorX4 };
            var startVector = new double[4][];
            for (var i = 0; i < vectorTextBoxes.Count; i++)
            {
                try
                {
                    var currentCoordinate = double.Parse(vectorTextBoxes[i].Text);
                    startVector[i] = new[] { currentCoordinate };
                    vectorTextBoxes[i].BorderBrush = Brushes.DarkGray;
                }
                catch (FormatException)
                {
                    vectorTextBoxes[i].BorderBrush = Brushes.Red;
                    throw;
                }
            }

            return new Matrix(startVector);
        }
    }
}