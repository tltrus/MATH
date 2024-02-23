using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace DrawingVisualApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;

        DrawingVisual visual;
        DrawingContext dc;
        int width, height;

        Point mouse;
        List<Point> data = new List<Point>();

        Gradient Gradient;
        Squares Squares;


        public MainWindow()
        {
            InitializeComponent();

            width = (int)g.Width;
            height = (int)g.Height;

            Gradient = new Gradient(width, height);
            Squares = new Squares(width, height);

            visual = new DrawingVisual();

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();

            Drawing();
        }


        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                if (cbGradient.IsChecked == true)
                    Gradient.Drawing(dc);

                if (cbSquares.IsChecked == true)
                    Squares.Drawing(dc);

                // Points
                foreach (var point in data)
                {
                    var x = Numerics.Map(point.X, 0, 1, 0, width);
                    var y = Numerics.Map(point.Y, 0, 1, height, 0);

                    dc.DrawEllipse(Brushes.White, null, new Point(x, y), 3, 3);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void g_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouse = e.GetPosition(g);

            var x = Numerics.Map(mouse.X, 0, width, 0, 1);
            var y = Numerics.Map(mouse.Y, 0, height, 1, 0);

            data.Add(new Point(x, y));
        }

        private void timerTick(object sender, EventArgs e)
        {
            Gradient.GradientDescent(data);
            Squares.LinearRegression(data);
            
            Drawing();
        }
    }
}
