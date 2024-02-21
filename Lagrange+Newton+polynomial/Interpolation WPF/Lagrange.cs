using System.Collections.Generic;

namespace Interpolation_WPF
{
    class Lagrange
    {
        private double L(int index, List<double> X, double x)
        {
            double l = 1;
            for (int i = 0; i < X.Count; i++)
            {
                if (i != index)
                {
                    l *= (x - X[i]) / (X[index] - X[i]);
                }
            }
            return l;
        }

        public double GetValue(List<double> X, List<double> Y, double x)
        {
            double y = 0;
            for (int i = 0; i < X.Count; i++)
            {
                y += Y[i] * L(i, X, x);
            }

            return y;
        }
    }
}
