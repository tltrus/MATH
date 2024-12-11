using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    internal class Harter_Dragon
    {
        public Harter_Dragon(WriteableBitmap wb) 
        {
            wb.Clear(Colors.White);

            int x1 = 200;
            int y1 = 300;
            int x2 = 500;
            int y2 = 300;
            int k = 20;

            Drawing(x1, y1, x2, y2, k, wb);
        }

        public void Drawing(int x1, int y1, int x2, int y2, int k, WriteableBitmap wb)
        {
            int tx, ty = 0;
            //Color color = Colors.Blue; // задаем цвет кривой
            byte r = (byte)(x1 + y1);
            byte g = (byte)x1;  // находим цвет;
            byte b = (byte)y1;
            Color color = Color.FromRgb(r, g, b);

            if (k > 0)
            {
                // формулы вычисления координат точки, находящейся на середине отрезка и удаленной от прямой на такое расстояние,
                // чтобы при соединении этой точки с концами отрезка получился угол в 90 градусов 
                tx = (x1 + x2) / 2 + (y2 - y1) / 2;
                ty = (y1 + y2) / 2 - (x2 - x1) / 2;
                // рекурсивный вызов функций, соединяющих концы отрезка с данной точкой 
                Drawing(x2, y2, tx, ty, k - 1, wb);
                Drawing(x1, y1, tx, ty, k - 1, wb);
            }
            else
            {
                wb.DrawLine(x1, y1, x2, y2, color);
            }
        }
    }
}
