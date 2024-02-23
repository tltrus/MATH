using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    internal class Gradient
    {
        List<Point> data = new List<Point>();
        double a, b, learning_rate = 0.05;
        int width, height;

        public Gradient(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void GradientDescent(List<Point> data)
        {
            this.data = data;

            foreach (var d in data)
            {
                var x = d.X;
                var y = d.Y;

                var guess = a * x + b;
                var error = y - guess;

                a = a + error * x * learning_rate;
                b = b + error * learning_rate;
            }
        }

        public void Drawing(DrawingContext dc)
        {
            // Line drawing

            double x1 = 0;
            double y1 = a * x1 + b;
            double x2 = 1;
            double y2 = a * x2 + b;

            x1 = Numerics.Map(x1, 0, 1, 0, width);
            y1 = Numerics.Map(y1, 0, 1, height, 0);
            x2 = Numerics.Map(x2, 0, 1, 0, width);
            y2 = Numerics.Map(y2, 0, 1, height, 0);

            dc.DrawLine(new Pen(Brushes.LightGreen, 2), new Point(x1, y1), new Point(x2, y2));
        }
    }
}
