using System.Windows;
using System.Windows.Media;

namespace MovementControl
{
    public partial class MainWindow
    {
        private const int _fieldSize = 600;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                DrawField(dc);
                var drawingpen = new Pen(Brushes.Red, 1);
                dc.DrawLine(drawingpen, new Point(300, 50), new Point(350, 0));
            }

            var myDrawingImage = new DrawingImage(visual.Drawing);
            image1.Source = myDrawingImage;
        }

        private void DrawField(DrawingContext dc)
        {
            DrawAxis(dc);
            DrawFieldX(dc);
            DrawFieldY(dc);
        }

        private void DrawAxis(DrawingContext dc)
        {
            var drawingpen = new Pen(Brushes.Black, 1);
            dc.DrawLine(drawingpen, new Point(300, 0), new Point(300, 600));
            dc.DrawLine(drawingpen, new Point(0, 300), new Point(600, 300));

            dc.DrawLine(drawingpen, new Point(300, 0), new Point(305, 10));
            dc.DrawLine(drawingpen, new Point(300, 0), new Point(295, 10));
            dc.DrawLine(drawingpen, new Point(600, 300), new Point(590, 295));
            dc.DrawLine(drawingpen, new Point(600, 300), new Point(590, 305));
        }

        private void DrawFieldX(DrawingContext dc)
        {
            var drawingpen = new Pen(Brushes.Gray, 0.5);
            for (var i = 0; i <= 600; i += 10)
            {
                dc.DrawLine(drawingpen, new Point(0, i), new Point(600, i));

            }
        }

        private void DrawFieldY(DrawingContext dc)
        {
            var drawingpen = new Pen(Brushes.Gray, 0.5);
            for (var i = 0; i <= 600; i += 10)
            {
                dc.DrawLine(drawingpen, new Point(i, 0), new Point(i, 600));

            }
        }
    }
}