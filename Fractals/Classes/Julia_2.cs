using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    internal class Julia_2 : Fractal
    {
        public Julia_2(WriteableBitmap wb, int width, int height) : base(wb, width, height)
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
                    c.x = -0.70176;
                    c.y = -0.3842;

                    iter = 300;

                    while ((((z.x * z.x) + (z.y * z.y)) < max) && (n < iter))
                    {
                        double tx = z.x;
                        double ty = z.y;
                        z.x = (tx * tx) - (ty * ty) + c.x;
                        z.y = 2 * tx * ty + c.y;
                        n++;
                    }
                    // для Жулиа 2
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
