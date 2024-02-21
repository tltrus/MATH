using System;
using System.Collections.Generic;


namespace Interpolation_WPF
{
    class Newton
    {
        public double dy(List<double> Y, List<double> X)
        {
            if (Y.Count > 2)
            {
                List<double> Yleft = new List<double>(Y);
                List<double> Xleft = new List<double>(X);
                Xleft.RemoveAt(0);
                Yleft.RemoveAt(0);
                List<double> Yright = new List<double>(Y);
                List<double> Xright = new List<double>(X);
                Xright.RemoveAt(Y.Count - 1);
                Yright.RemoveAt(Y.Count - 1);
                return (dy(Yleft, Xleft) - dy(Yright, Xright)) / (X[X.Count - 1] - X[0]);
            }
            else if (Y.Count == 2)
            {
                return (Y[1] - Y[0]) / (X[1] - X[0]);
            }
            else
            {
                throw new Exception("Not available parameter");
            }
        }

        public double GetValue(List<double> X, List<double> Y, double x)
        {
            double res = Y[0];
            double buf;
            List<double> Xlist;
            List<double> Ylist;
            for (int i = 1; i < Y.Count; i++)
            {
                Xlist = new List<double>();
                Ylist = new List<double>();
                buf = 1;
                for (int j = 0; j <= i; j++)
                {
                    Xlist.Add(X[j]);
                    Ylist.Add(Y[j]);
                    if (j < i)
                        buf *= x - X[j];
                }
                res += dy(Ylist, Xlist) * buf;
            }
            return res;
        }

        public double dy_h(List<double> Y, List<double> X, int number, int index)
        {
            if (number > 1)
            {
                return (dy_h(Y, X, number - 1, index + 1) - dy_h(Y, X, number - 1, index));
            }
            else if (number == 1)
            {
                return (Y[index + 1] - Y[index]);
            }
            else
            {
                throw new Exception("Not available parameter");
            }
        }
    }
}