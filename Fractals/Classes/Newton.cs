using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    internal class Newton : Fractal
    {
        public Newton(WriteableBitmap wb, int width, int height) : base(wb, width, height)
        {

        }

        public void Draw()
        {
            double dx = 0, dy = 0; // для случая 4 [Бассейн Ньютона 1]

            wb.Clear(Colors.White);

            for (int y = -my; y < my; y++)
            {
                for (int x = -mx; x < mx; x++)
                {
                    n = 0;

                    z.x = x * 0.005 * zoom;
                    z.y = y * 0.005 * zoom;
                    dx = z.x; dy = z.y;

                    iter = 50;

                    double Min = 1e-6, Max = 1e+6;
                    double p;

                    while ((Math.Pow(z.x, 2) + Math.Pow(z.y, 2) < Max) && (Math.Pow(dx, 2) + Math.Pow(dy, 2) > Min) && (n < iter))
                    {
                        double tx = z.x;
                        double ty = z.y;
                        p = Math.Pow(Math.Pow(tx, 2) + Math.Pow(ty, 2), 2);
                        z.x = 2 / 3 * tx + (Math.Pow(tx, 2) - Math.Pow(ty, 2)) / (3 * p);
                        z.y = 2 / 3 * ty * (1 - tx / p);
                        dx = Math.Abs(tx - z.x);
                        dy = Math.Abs(ty - z.y);
                        n++;
                    }

                    r = (byte)((n * 9) % 255);
                    g = 0;
                    b = (byte)((n * 9) % 255);

                    //цвет выбирается по числу итераций
                    if (n < iter)
                        wb.SetPixel(mx + x, my + y, r, g, b); //перекрашиваем соответствующую точку на canvas'e в соответствующий цвет
                }
            }

        }

    }
}
