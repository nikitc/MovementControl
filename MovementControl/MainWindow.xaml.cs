using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using MovementControl.Draw;

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
            var visual = new DrawingVisual();
            var countPoints = 20;
            var start = int.Parse(startField.Text);
            var end = int.Parse(endField.Text);
            
            using (var drawingContext = visual.RenderOpen())
            {
                var drawer = new Drawer(drawingContext, start, end);
                drawer.DrawField(30);
                drawer.DrawFunction(countPoints, Example1);
            }

            var myDrawingImage = new DrawingImage(visual.Drawing);
            image1.Source = myDrawingImage;
        }

        private List<Matrix> Example1(int count)
        {
            var start = new []
            {
                new double[] {-1},
                new double[] {2},
                new double[] {4},
                new double[] {5}
            };
            var startVector = new Matrix(start);
            var result = new List<Matrix>{ startVector };
            for (var i = 0; i < count; i++)
            {
                var funcs = new List<Func<double, double>>
                {
                    x => Math.Cos(x) - 3,
                    Math.Cos,
                    x => Math.Sin(x) + 2,   
                    Math.Sin

                };
                var current = GetMatrix1(i, i + 1) * startVector + CalcIntegral(i, i + 1, funcs);
                result.Add(current);
            }

            return result;
        }

        private Matrix CalcIntegral(double t1, double t2, List<Func<double, double>> funcs)
        {
            return IntegralCalculator.RightRectanglesFormula(t1, t2, 0.1, funcs);
        }

        private static Matrix GetMatrix1(double ta, double tb)
        {
            var X = new[]
            {
                new [] {1, tb - ta, 0, 0},
                new double[] {0, 1, 0, 0},
                new [] {0, 0, 1, tb - ta},
                new double[] {0, 0, 0, 1},
            };

            return new Matrix(X);
        }
    }
}