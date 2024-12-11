using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    internal class Julia_1 : Fractal
    {
        public Julia_1(WriteableBitmap wb, int width, int height) : base(wb, width, height)
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

                    z.x = x * 0.005 * zoom;
                    z.y = y * 0.005 * zoom;
                    c.x = 0.11;
                    c.y = -0.66;

                    while ((((z.x * z.x) + (z.y * z.y)) < max) && (n < iter))
                    {
                        double tx = z.x;
                        double ty = z.y;
                        z.x = (tx * tx) - (ty * ty) + c.x;
                        z.y = 2 * tx * ty + c.y;
                        n++;
                    }
                    r = 0;
                    g = (byte)((n * 6) % 255); // находим цвет;
                    b = 0;

                    //цвет выбирается по числу итераций
                    if (n < iter)
                        wb.SetPixel(mx + x, my + y, r, g, b); //перекрашиваем соответствующую точку на canvas'e в соответствующий цвет
                }
            }

        }

    }
}
