using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using MovementControl.Examples;

namespace MovementControl.Draw
{
    public class Drawer
    {
        private CartesianChart _cartesianChart { get; }
        
        public Drawer(CartesianChart cartesianChart)
        {
            _cartesianChart = cartesianChart;
        }
        
        public void DrawFunction(int count, Func<int, IMovementControlCondition, List<Matrix>> movementControl, IMovementControlCondition condititon)
        {
            var points = new Queue<Matrix>(movementControl(count, condititon));
            var mapper = Mappers.Xy<Point>()
                .X(x => x.X)
                .Y(x => x.Y);

            var values = new ChartValues<Point>();
            foreach (var point in points)
            {
                values.Add(new Point(point[0, 0], point[2, 0]));
            }
            
            _cartesianChart.Series = new SeriesCollection(mapper)
            {
                new LineSeries
                {
                    Title = "Движение",
                    Values = values,
                    Fill = Brushes.Transparent,
                }
            };
        }
    }
}