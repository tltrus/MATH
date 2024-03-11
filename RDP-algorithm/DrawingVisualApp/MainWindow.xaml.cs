using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    /// <summary>
    /// Based on Coding Challenge #152: RDP Line Simplification Algorithm https://www.youtube.com/watch?v=nSYw9GrakjY
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timerDraw;
        Random rnd = new Random();
        int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        PointCollection points = new PointCollection();
        double epsilon;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            // Формирование графика функции
            for (int x = 0; x < width; x++)
            {
                var xval = UsefulTools.Map(x, 0, width, 0, 5);
                var yval = Math.Exp(-xval) * Math.Cos(Math.PI * 2 * xval);
                var y = UsefulTools.Map(yval, -1, 1, height, 0);

                points.Add(new Point(x, y));
            }

            timerDraw = new System.Windows.Threading.DispatcherTimer();
            timerDraw.Tick += new EventHandler(timerDrawTick);
            timerDraw.Interval = new TimeSpan(0, 0, 0, 0, 30);

            timerDraw.Start();
        }

        private void timerDrawTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            // ОРИГИНАЛЬНЫЙ ГРАФИК ФУНКЦИИ
            StreamGeometry pointsGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = pointsGeometry.Open())
            {
                geometryContext.BeginFigure(points[0], false, false);
                geometryContext.PolyLineTo(points, true, false);
            }


            // RDP ГРАФИК ФУНКЦИИ
            List<Point> rdpPoints = new List<Point>();

            var total = points.Count;
            var start = points[0];
            var end = points[total - 1];
            rdpPoints.Add(start);
            RDP(0, total - 1, points, rdpPoints);
            rdpPoints.Add(end);

            epsilon += 0.01; // Эпсилон - величина расстояния, которое останавливает дробление
            if (epsilon > 100)
            {
                epsilon = 0;
            }


            StreamGeometry rdpGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = rdpGeometry.Open())
            {
                geometryContext.BeginFigure(rdpPoints[0], false, false);
                geometryContext.PolyLineTo(rdpPoints, true, false);
            }


            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                // Вывод оригинального графика
                dc.DrawGeometry(null, new Pen(Brushes.Green, 4), pointsGeometry);

                // Вывод RDP графика
                dc.DrawGeometry(null, new Pen(Brushes.White, 2), rdpGeometry);

                dc.Close();
                g.AddVisual(visual);
            }

            lbEpsilon.Content = "Epsilon: " + epsilon.ToString();
            lbN.Content = "RDP points: " + rdpPoints.Count().ToString();
        }

        /*
         * Алгоритм рекурсивно делит линию. 
         * Входом алгоритма служат координаты всех точек между первой и последней. 
         * Первая и последняя точка сохраняются неизменными. 
         * После чего алгоритм находит точку, наиболее удалённую от отрезка, соединяющего первую и последнюю.
         */
        void RDP(int startIndex, int endIndex, PointCollection allPoints, List<Point> rdpPoints)
        {
            var nextIndex = FindFurthest(allPoints, startIndex, endIndex);
            if (nextIndex > 0)
            {
                if (startIndex != nextIndex)
                    RDP(startIndex, nextIndex, allPoints, rdpPoints);
                
                rdpPoints.Add(allPoints[nextIndex]);

                if (endIndex != nextIndex)
                    RDP(nextIndex, endIndex, allPoints, rdpPoints);
            }
        }

        int FindFurthest(PointCollection points, int a, int b)
        {
            double recordDistance = -1;
            var start = new Vector2D(points[a].X, points[a].Y);
            var end = new Vector2D(points[b].X, points[b].Y);
            var furthestIndex = -1;
            for (int i = a + 1; i < b; i++)
            {
                var currentPoint = new Vector2D(points[i].X, points[i].Y);
                var d = LineDist(currentPoint, start, end);
                if (d > recordDistance)
                {
                    recordDistance = d;
                    furthestIndex = i;
                }
            }

            /*  Если же расстояние больше ε, то алгоритм рекурсивно вызывает себя на наборе 
                от начальной до данной и от данной до конечной точках (что означает, что данная точка будет отмечена к сохранению).
            */
            if (recordDistance > epsilon)
            {
                return furthestIndex;
            }
            else
            {
                /* Если точка находится на расстоянии, меньшем ε, то все точки, которые ещё не были отмечены к сохранению, 
                *  могут быть выброшены из набора и получившаяся прямая сглаживает кривую с точностью не ниже ε
                */
                return -1;
            }
        }

        double LineDist(Vector2D c, Vector2D a, Vector2D b)
        {
            Vector2D norm = ScalarProjection(c, a, b);
            return Vector2D.Dist(c, norm);
        }

        Vector2D ScalarProjection(Vector2D p, Vector2D a, Vector2D b)
        {
            var ap = Vector2D.Sub(p, a);
            var ab = Vector2D.Sub(b, a);
            ab.Normalize(); // Normalize the line
            ab.Mult(ap.Dot(ab));
            var normalPoint = Vector2D.Add(a, ab);
            return normalPoint;
        }
    }
}
