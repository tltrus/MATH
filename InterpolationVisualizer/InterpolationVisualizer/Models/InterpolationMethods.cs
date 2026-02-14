using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpolationVisualizer.Models
{
    public static class InterpolationMethods
    {
        // 7.2 Linear Interpolation
        public static List<DataPoint> LinearInterpolation(List<DataPoint> points, double step = 0.01)
        {
            if (points.Count < 2) return new List<DataPoint>();

            var sortedPoints = points.OrderBy(p => p.X).ToList();
            var result = new List<DataPoint>();

            for (int i = 0; i < sortedPoints.Count - 1; i++)
            {
                double x0 = sortedPoints[i].X;
                double x1 = sortedPoints[i + 1].X;
                double y0 = sortedPoints[i].Y;
                double y1 = sortedPoints[i + 1].Y;

                for (double x = x0; x <= x1; x += step)
                {
                    double t = (x - x0) / (x1 - x0);
                    double y = y0 * (1 - t) + y1 * t;
                    result.Add(new DataPoint(x, y));
                }
            }

            return result;
        }

        // 7.3 Bilinear Interpolation (2D)
        public static double[,] BilinearInterpolation(double[,] grid, int newWidth, int newHeight)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            double[,] result = new double[newWidth, newHeight];

            for (int i = 0; i < newWidth; i++)
            {
                double x = (double)i / (newWidth - 1) * (width - 1);
                int x1 = (int)Math.Floor(x);
                int x2 = Math.Min(x1 + 1, width - 1);
                double tx = x - x1;

                for (int j = 0; j < newHeight; j++)
                {
                    double y = (double)j / (newHeight - 1) * (height - 1);
                    int y1 = (int)Math.Floor(y);
                    int y2 = Math.Min(y1 + 1, height - 1);
                    double ty = y - y1;

                    double v11 = grid[x1, y1];
                    double v12 = grid[x1, y2];
                    double v21 = grid[x2, y1];
                    double v22 = grid[x2, y2];

                    double v1 = v11 * (1 - ty) + v12 * ty;
                    double v2 = v21 * (1 - ty) + v22 * ty;
                    result[i, j] = v1 * (1 - tx) + v2 * tx;
                }
            }

            return result;
        }

        // 7.4.1 Lagrange Interpolation
        public static List<DataPoint> LagrangeInterpolation(List<DataPoint> points, double step = 0.01)
        {
            if (points.Count < 2) return new List<DataPoint>();

            var sortedPoints = points.OrderBy(p => p.X).ToList();
            double minX = sortedPoints.First().X;
            double maxX = sortedPoints.Last().X;
            var result = new List<DataPoint>();

            for (double x = minX; x <= maxX; x += step)
            {
                double y = 0;
                for (int i = 0; i < sortedPoints.Count; i++)
                {
                    double term = sortedPoints[i].Y;
                    for (int j = 0; j < sortedPoints.Count; j++)
                    {
                        if (j != i)
                        {
                            term *= (x - sortedPoints[j].X) / (sortedPoints[i].X - sortedPoints[j].X);
                        }
                    }
                    y += term;
                }
                result.Add(new DataPoint(x, y));
            }

            return result;
        }

        // 7.4.2 Barycentric Interpolation
        public static List<DataPoint> BarycentricInterpolation(List<DataPoint> points, double step = 0.01)
        {
            if (points.Count < 2) return new List<DataPoint>();

            var sortedPoints = points.OrderBy(p => p.X).ToList();
            int n = sortedPoints.Count;
            double[] w = new double[n];

            // Compute barycentric weights
            for (int i = 0; i < n; i++)
            {
                w[i] = 1.0;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        w[i] *= 1.0 / (sortedPoints[i].X - sortedPoints[j].X);
                    }
                }
            }

            double minX = sortedPoints.First().X;
            double maxX = sortedPoints.Last().X;
            var result = new List<DataPoint>();

            for (double x = minX; x <= maxX; x += step)
            {
                double numerator = 0;
                double denominator = 0;
                bool exact = false;
                double y = 0;

                for (int i = 0; i < n; i++)
                {
                    if (Math.Abs(x - sortedPoints[i].X) < 1e-10)
                    {
                        y = sortedPoints[i].Y;
                        exact = true;
                        break;
                    }

                    double temp = w[i] / (x - sortedPoints[i].X);
                    numerator += temp * sortedPoints[i].Y;
                    denominator += temp;
                }

                if (!exact)
                {
                    y = numerator / denominator;
                }

                result.Add(new DataPoint(x, y));
            }

            return result;
        }

        // 7.4.3 Newton's Divided Differences Interpolation
        public static List<DataPoint> NewtonInterpolation(List<DataPoint> points, double step = 0.01)
        {
            if (points.Count < 2) return new List<DataPoint>();

            var sortedPoints = points.OrderBy(p => p.X).ToList();
            int n = sortedPoints.Count;
            double[,] f = new double[n, n];

            // Initialize with y values
            for (int i = 0; i < n; i++)
            {
                f[i, 0] = sortedPoints[i].Y;
            }

            // Compute divided differences
            for (int j = 1; j < n; j++)
            {
                for (int i = 0; i < n - j; i++)
                {
                    f[i, j] = (f[i + 1, j - 1] - f[i, j - 1]) /
                              (sortedPoints[i + j].X - sortedPoints[i].X);
                }
            }

            double minX = sortedPoints.First().X;
            double maxX = sortedPoints.Last().X;
            var result = new List<DataPoint>();

            for (double x = minX; x <= maxX; x += step)
            {
                double y = f[0, 0];
                double product = 1.0;

                for (int i = 1; i < n; i++)
                {
                    product *= (x - sortedPoints[i - 1].X);
                    y += f[0, i] * product;
                }

                result.Add(new DataPoint(x, y));
            }

            return result;
        }

        // 7.5.1 Natural Cubic Splines
        public static List<DataPoint> NaturalCubicSpline(List<DataPoint> points, double step = 0.01)
        {
            if (points.Count < 2) return new List<DataPoint>();

            var sortedPoints = points.OrderBy(p => p.X).ToList();
            int n = sortedPoints.Count;

            double[] h = new double[n - 1];
            double[] alpha = new double[n - 1];
            double[] l = new double[n];
            double[] mu = new double[n];
            double[] z = new double[n];
            double[] c = new double[n];
            double[] b = new double[n - 1];
            double[] d = new double[n - 1];

            // Step 1
            for (int i = 0; i < n - 1; i++)
            {
                h[i] = sortedPoints[i + 1].X - sortedPoints[i].X;
            }

            // Step 2
            for (int i = 1; i < n - 1; i++)
            {
                alpha[i] = (3 / h[i]) * (sortedPoints[i + 1].Y - sortedPoints[i].Y) -
                          (3 / h[i - 1]) * (sortedPoints[i].Y - sortedPoints[i - 1].Y);
            }

            // Step 3
            l[0] = 1;
            mu[0] = 0;
            z[0] = 0;

            // Step 4
            for (int i = 1; i < n - 1; i++)
            {
                l[i] = 2 * (sortedPoints[i + 1].X - sortedPoints[i - 1].X) - h[i - 1] * mu[i - 1];
                mu[i] = h[i] / l[i];
                z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
            }

            // Step 5
            l[n - 1] = 1;
            z[n - 1] = 0;
            c[n - 1] = 0;

            // Step 6
            for (int j = n - 2; j >= 0; j--)
            {
                c[j] = z[j] - mu[j] * c[j + 1];
                b[j] = (sortedPoints[j + 1].Y - sortedPoints[j].Y) / h[j] -
                       h[j] * (c[j + 1] + 2 * c[j]) / 3;
                d[j] = (c[j + 1] - c[j]) / (3 * h[j]);
            }

            // Generate interpolated points
            var result = new List<DataPoint>();
            for (int i = 0; i < n - 1; i++)
            {
                double x0 = sortedPoints[i].X;
                double x1 = sortedPoints[i + 1].X;
                double a = sortedPoints[i].Y;

                for (double x = x0; x <= x1; x += step)
                {
                    double dx = x - x0;
                    double y = a + b[i] * dx + c[i] * dx * dx + d[i] * dx * dx * dx;
                    result.Add(new DataPoint(x, y));
                }
            }

            return result;
        }

        // 7.5.2 Clamped Cubic Splines
        public static List<DataPoint> ClampedCubicSpline(List<DataPoint> points,
            double startSlope, double endSlope, double step = 0.01)
        {
            if (points.Count < 2) return new List<DataPoint>();

            var sortedPoints = points.OrderBy(p => p.X).ToList();
            int n = sortedPoints.Count;

            double[] h = new double[n - 1];
            double[] alpha = new double[n];
            double[] l = new double[n];
            double[] mu = new double[n];
            double[] z = new double[n];
            double[] c = new double[n];
            double[] b = new double[n - 1];
            double[] d = new double[n - 1];

            // Step 1
            for (int i = 0; i < n - 1; i++)
            {
                h[i] = sortedPoints[i + 1].X - sortedPoints[i].X;
            }

            // Step 2
            alpha[0] = 3 * (sortedPoints[1].Y - sortedPoints[0].Y) / h[0] - 3 * startSlope;
            alpha[n - 1] = 3 * endSlope - 3 * (sortedPoints[n - 1].Y - sortedPoints[n - 2].Y) / h[n - 2];

            for (int i = 1; i < n - 1; i++)
            {
                alpha[i] = (3 / h[i]) * (sortedPoints[i + 1].Y - sortedPoints[i].Y) -
                          (3 / h[i - 1]) * (sortedPoints[i].Y - sortedPoints[i - 1].Y);
            }

            // Step 3
            l[0] = 2 * h[0];
            mu[0] = 0.5;
            z[0] = alpha[0] / l[0];

            // Step 4
            for (int i = 1; i < n - 1; i++)
            {
                l[i] = 2 * (sortedPoints[i + 1].X - sortedPoints[i - 1].X) - h[i - 1] * mu[i - 1];
                mu[i] = h[i] / l[i];
                z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
            }

            // Step 5
            l[n - 1] = h[n - 2] * (2 - mu[n - 2]);
            z[n - 1] = (alpha[n - 1] - h[n - 2] * z[n - 2]) / l[n - 1];
            c[n - 1] = z[n - 1];

            // Step 6
            for (int j = n - 2; j >= 0; j--)
            {
                c[j] = z[j] - mu[j] * c[j + 1];
                b[j] = (sortedPoints[j + 1].Y - sortedPoints[j].Y) / h[j] -
                       h[j] * (c[j + 1] + 2 * c[j]) / 3;
                d[j] = (c[j + 1] - c[j]) / (3 * h[j]);
            }

            // Generate interpolated points
            var result = new List<DataPoint>();
            for (int i = 0; i < n - 1; i++)
            {
                double x0 = sortedPoints[i].X;
                double x1 = sortedPoints[i + 1].X;
                double a = sortedPoints[i].Y;

                for (double x = x0; x <= x1; x += step)
                {
                    double dx = x - x0;
                    double y = a + b[i] * dx + c[i] * dx * dx + d[i] * dx * dx * dx;
                    result.Add(new DataPoint(x, y));
                }
            }

            return result;
        }
    }
}