using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    // Based on #148 — Gift Wrapping Algorithm https://thecodingtrain.com/challenges/148-gift-wrapping
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        Random rnd = new Random();
        int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        List<Point> points = new List<Point>();
        List<Point> hull = new List<Point>();
        int MARGIN = 100;
        Point leftMost = new Point();
        Point currentVertex = new Point();
        Point nextVertex = new Point();
        int index;
        int nextIndex = -1;
        bool stop;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            Init();
        }

        void Init()
        {
            // Создание случайных точек на экране
            for (int i = 0; i < 70; ++i)
            {
                var x = rnd.Next(MARGIN, width - MARGIN);
                var y = rnd.Next(MARGIN, height - MARGIN);
                points.Add(new Point(x, y));
            }

            // Сортировка и выбор самой левой точки по Х
            var sortedPoints =  points.OrderBy(p => p.X)
                                    .ThenBy(p => p.Y)
                                    .ToList();
            leftMost = sortedPoints[0];

            currentVertex = leftMost;
            hull.Add(currentVertex);
            nextVertex = sortedPoints[1];
            index = 2;
        }

        void Drawing()
        {
            if (stop)
            {
                timer.Stop();
                return;
            }

            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                // Отрисовка точек
                for (int i = 0; i < points.Count; i++)
                {
                    dc.DrawEllipse(Brushes.White, null, points[i], 3, 3);
                }

                // Отрисовка Вершин
                for (int i = 0; i < hull.Count; i++)
                {
                    dc.DrawEllipse(Brushes.Chartreuse, null, hull[i], 5, 5);
                }

                // Отрисовка обертки
                PointCollection dc_points = new PointCollection(hull);
                StreamGeometry streamGeometry = new StreamGeometry();
                using (StreamGeometryContext geometryContext = streamGeometry.Open())
                {
                    var beginPoint = new Point(dc_points[0].X, dc_points[0].Y);
                    geometryContext.BeginFigure(beginPoint, true, true);
                    geometryContext.PolyLineTo(dc_points, true, true);
                }
                dc.DrawGeometry(null, new Pen(Brushes.Chartreuse, 3), streamGeometry);

                Point checking = points[index];

                Point a = new Point(nextVertex.X - currentVertex.X, nextVertex.Y - currentVertex.Y);
                Point b = new Point(checking.X - currentVertex.X, checking.Y - currentVertex.Y);
                double cross = a.X * b.Y - a.Y * b.X;

                if (cross < 0)
                {
                    nextVertex = checking;
                    nextIndex = index;
                }

                index++;

                if (index == points.Count)
                {
                    if (nextVertex == leftMost)
                    {
                        stop = true;
                    }
                    else
                    {
                        hull.Add(nextVertex);
                        currentVertex = nextVertex;
                        index = 0;
                        nextVertex = leftMost;
                    }
                } 

                if (!stop)
                {
                    dc.DrawEllipse(Brushes.Blue, null, currentVertex, 3, 3);
                    dc.DrawEllipse(Brushes.Yellow, null, nextVertex, 3, 3);
                    dc.DrawLine(new Pen(Brushes.Yellow, 1), currentVertex, nextVertex);

                    dc.DrawLine(new Pen(Brushes.White, 1), currentVertex, checking);
                }


                dc.Close();
                g.AddVisual(visual);
            }
        }

        void TimerTick(object sender, EventArgs e) => Drawing();
        private void btnStart_Click(object sender, RoutedEventArgs e) => timer.Start();
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Init();
            g.RemoveVisual(visual);
        }
        private void btnStep_Click(object sender, RoutedEventArgs e) => Drawing();
    }
}
