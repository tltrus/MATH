using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace Interpolation_WPF
{
    public partial class MainWindow : Window
    {
        DrawingVisual visual;
        DrawingContext dc;

        List<Point> pointsAxis, pointsScreen;
        List<Point> pointsLagrange, pointsNewton;
        
        int mpSize = 3;         // mouse point size
        int pSize = 1;          // point size

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            pointsAxis = new List<Point>();
            pointsScreen = new List<Point>();

            pointsLagrange = new List<Point>();
            pointsNewton = new List<Point>();
        }

        public Point AxisToDisplay(Point axisPoint)
        {
            Point point = new Point(axisPoint.X, axisPoint.Y * -1);
            return point;
        }

        public Point DisplayToAxis(Point dispPoint)
        {
            Point point = new Point(dispPoint.X, dispPoint.Y * -1);
            return point;
        }

        private void g_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(g);
            pointsAxis.Add(DisplayToAxis(mousePos));
            pointsScreen.Add(mousePos);

            Draw();
        }

        private void g_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            pointsAxis.Clear();
            pointsScreen.Clear();

            pointsLagrange.Clear();
            pointsNewton.Clear();

            Draw();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            pointsLagrange.Clear();
            pointsNewton.Clear();

            List<double> Y = new List<double>();
            List<double> X = new List<double>();
            double y;

            var SortedPointList = pointsAxis.OrderBy(u => u.X); // Сортируем точки во возрастанию Х, чтобы уйти от проблемы заполнения в разнобой по оси Х

            foreach (var elem in SortedPointList)
            {
                X.Add(elem.X);
                Y.Add(elem.Y);
            }

            Lagrange lagrange = new Lagrange();
            Newton newton = new Newton();

            if (SortedPointList.Count() == 0) return;

            for (var x = SortedPointList.First().X; x <= SortedPointList.Last().X; x += 1)
            {
                // Lagrange
                y = lagrange.GetValue(X, Y, x);
                y = y * -1;
                pointsLagrange.Add(new Point(x, y));
            }

            for (var x = SortedPointList.First().X; x <= SortedPointList.Last().X; x += 2)
            {
                // Newton
                y = newton.GetValue(X, Y, x);
                y = y * -1;
                pointsNewton.Add(new Point(x, y));  
            }

            cbLang.IsChecked = true;
            cbNewt.IsChecked = true;

            Draw();
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                foreach(var point in pointsScreen)
                {
                    dc.DrawEllipse(Brushes.White, null, point, mpSize, mpSize);
                }

                if (cbLang.IsChecked == true)
                {
                    foreach (var point in pointsLagrange)
                    {
                        dc.DrawEllipse(Brushes.Red, null, point, pSize, pSize);
                    }
                }

                if (cbNewt.IsChecked == true)
                {
                    foreach (var point in pointsNewton)
                    {
                        dc.DrawEllipse(Brushes.Blue, null, point, pSize, pSize);
                    }
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }
    }
}
