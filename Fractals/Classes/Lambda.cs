using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    internal class Lambda : Fractal
    {
        public Lambda(WriteableBitmap wb, int width, int height) : base(wb, width, height)
        {

        }

        public void Draw()
        {
            wb.Clear(Colors.White);

            for (int y = -my; y < my; y++)
            {
                for (int x = -mx; x < mx; x++)
                {
                    n = 0;

                    z.x = 0.5;
                    z.y = 0;
                    c.x = (x * 0.01 + 1) * zoom;
                    c.y = y * 0.01 * zoom;

                    while ((((z.x * z.x) + (z.y * z.y)) < max) && (n < iter))
                    {
                        double p = z.x - z.x * z.x + z.y * z.y;
                        double q = z.y - 2 * z.x * z.y;
                        z.x = c.x * p - c.y * q;
                        z.y = c.x * q + c.y * p;
                        n++;
                    }

                    r = 0;
                    g = (byte)((n * 15) % 255); // находим цвет;
                    b = (byte)((n * 20) % 255);

                    //цвет выбирается по числу итераций
                    if (n < iter)
                        wb.SetPixel(mx + x, my + y, r, g, b); //перекрашиваем соответствующую точку на canvas'e в соответствующий цвет
                }
            }

        }

    }
}
