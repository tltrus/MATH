using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    internal class Squares
    {
        public List<Point> data = new List<Point>();
        int width, height;
        double a = 1, b;

        public Squares(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void LinearRegression(List<Point> data)
        {
            this.data = data;
            var n = data.Count;

            double xsum = 0, ysum = 0;
            foreach (var d in data)
            {
                xsum += d.X;
                ysum += d.Y;
            }
            var xmean = xsum / n;
            var ymean = ysum / n;

            double numerator = 0;
            double denumerator = 0;
            for (int i = 0; i < n; ++i)
            {
                numerator += (data[i].X - xmean) * (data[i].Y - ymean);
                denumerator += (data[i].X - xmean) * (data[i].X - xmean);
            }

            a = numerator / denumerator;
            b = ymean - a * xmean;
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

            dc.DrawLine(new Pen(Brushes.Purple, 2), new Point(x1, y1), new Point(x2, y2));
        }
    }
}
