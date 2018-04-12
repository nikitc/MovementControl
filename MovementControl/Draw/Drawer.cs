using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using MovementControl.Examples;

namespace MovementControl.Draw
{
    public class Drawer
    {
        private DrawingContext _drawingContext { get; }
        private int _start { get; }
        private int _end { get; }
        private int _fieldSize { get; }

        public Drawer(DrawingContext drawingContext, int start, int end)
        {
            _drawingContext = drawingContext;
            _start = start;
            _end = end;
            _fieldSize = end - start;
        }

        public void DrawField(double step)
        {
            DrawAxis();
            DrawFieldXY(step);
            SetNumbers(step);
        }

        private void SetNumbers(double step)
        {

            for (double i = _fieldSize * -1 / 2; i < _fieldSize / 2; i += step)
            {
                var text = new FormattedText(i.ToString(), CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight, new Typeface("Verdana"), 6, Brushes.Black);
                _drawingContext.DrawText(text, GetPoint(i, 0));
                _drawingContext.DrawText(text, GetPoint(0, i));
            }
        }

        private void DrawAxis()
        {
            var drawingpen = new Pen(Brushes.Black, 1);
            _drawingContext.DrawLine(drawingpen, GetPoint(0, _end), GetPoint(0, _start));
            _drawingContext.DrawLine(drawingpen, GetPoint(_start, 0), GetPoint(_end, 0));

            _drawingContext.DrawLine(drawingpen, GetPoint(0, _end), GetPoint(5, _end - 10));
            _drawingContext.DrawLine(drawingpen, GetPoint(0, _end), GetPoint(-5, _end - 10));

            _drawingContext.DrawLine(drawingpen, GetPoint(_end, 0), GetPoint(_end - 10, 5));
            _drawingContext.DrawLine(drawingpen, GetPoint(_end, 0), GetPoint(_end - 10, -5));
        }

        private void DrawFieldXY(double step)
        {
            var drawingpen = new Pen(Brushes.Gray, 0.5);
            for (double i = _start; i <= _end; i += step)
            {
                _drawingContext.DrawLine(drawingpen, GetPoint(_start, i), GetPoint(_end, i));
                _drawingContext.DrawLine(drawingpen, GetPoint(i, _start), GetPoint(i, _end));
            }
        }

        private double GetStepCell(double start, double end)
        {
            return _fieldSize / (Math.Abs(start) + Math.Abs(end));
        }

        private Point GetPoint(double x, double y)
        {
            return new Point(x + _fieldSize / 2, _fieldSize / 2 - y);
        }

        public void DrawFunction(int count, Func<int, IMovementControlContidion, List<Matrix>> movementControl, IMovementControlContidion condititon)
        {
            var drawingPen = new Pen(Brushes.Red, 1);
            var points = new Queue<Matrix>(movementControl(count, condititon));
            var oldPoint = points.Dequeue();
            var step = GetStepCell(_start, _end);
            while (points.Count != 0)
            {
                var newPoint = points.Dequeue();
                _drawingContext.DrawLine(drawingPen, GetPoint(oldPoint[0, 0] * step, oldPoint[2, 0] * step),
                    GetPoint(newPoint[0, 0] * step, newPoint[2, 0] * step));
                oldPoint = newPoint;
            }
        }
    }
}