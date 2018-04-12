using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
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
            var visual = new DrawingVisual();
            var countPoints = 20;
            var start = int.Parse(startField.Text);
            var end = int.Parse(endField.Text);
            
            using (var drawingContext = visual.RenderOpen())
            {
                var condition = new Example1();
                var drawer = new Drawer(drawingContext, start, end);
                drawer.DrawField(30);
                drawer.DrawFunction(countPoints, BuildMovementControl, condition);
            }

            var myDrawingImage = new DrawingImage(visual.Drawing);
            image1.Source = myDrawingImage;
        }

        private List<Matrix> BuildMovementControl(int count, IMovementControlContidion condition)
        {
            var result = new List<Matrix> { condition.StartVector };
            for (var i = 0; i < count; i++)
            {
                var current = condition.GetFundamentalMatrix(i, i + 1) * result.First()
                                    + CalcIntegral(i, i + 1, condition.Control);
                result.Add(current);
            }

            return result;
        }

        private Matrix CalcIntegral(double t1, double t2, List<Func<double, double>> funcs)
        {
            return IntegralCalculator.RightRectanglesFormula(t1, t2, 0.1, funcs);
        }
    }
}