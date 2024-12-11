using System;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    class Biomorph : Fractal
    {
        private int iter = 50;
        private int max = 70;
        int _type;

        public Biomorph(WriteableBitmap wb, int width, int height, int type) : base(wb, width, height)
        {
            _type = type;

            Draw();
        }

        private void Draw()
        {
            wb.Clear(Colors.White);

            for (int y = -my; y < my; y++)
            {
                for (int x = -mx; x < mx; x++)
                {
                    n = 0;

                    // Выбор типа биоморфа
                    switch (_type)   // 
                    {
                        case 0:
                            //выбираем значения константы с, которая определяет форму биоморфа
                            c.x = 1.00003 * zoom;
                            c.y = 1.01828 * zoom;

                            z.x = x * 0.01;
                            z.y = y * 0.01;

                            break;

                        case 1:
                            c.x = 0.5 * zoom;
                            c.y = 0;

                            z.x = x * 0.005;
                            z.y = y * 0.005;
                            max = 100;

                            break;
                    }

                    // Логика взависимости от типа фрактала
                    if (_type == 0)
                    {
                        // 
                        while ((z.x * z.x < max && z.y * z.y < max) && (n < iter))
                        {
                            //запоминаем предыдущее значение
                            double tx = z.x;
                            double ty = z.y;
                            //z^4 + c
                            //вычисление в текущем значении n < iterations
                            z.x = tx * tx * tx * tx + ty * ty * ty * ty -
                                     6 * tx * tx * ty * ty + c.x;
                            z.y = 4 * tx * tx * tx * ty - 4 * tx * ty * ty * ty + c.y;
                            n++;
                        }
                        r = (byte)(n % 255); // находим цвет;;
                        g = 0;
                        b = 0;

                        //цвет выбирается по числу итераций
                        if (Math.Abs(z.x) > 10 || Math.Abs(z.y) > 1000)
                            wb.SetPixel(mx + x, my + y, r, g, b); //перекрашиваем соответствующую точку на canvas'e в соответствующий цвет

                    }
                    else // 

                    if (_type == 1)
                    {
                        // 
                        while ((z.x * z.x < max && z.y * z.y < max) && (n < iter))
                        {
                            //запоминаем предыдущее значение
                            double tx = z.x;
                            double ty = z.y;
                            //z^3 + c
                            //вычисление в текущем значении n < iterations
                            z.x = tx * tx * tx - 3 * tx * ty * ty + c.x;
                            z.y = 3 * tx * tx * ty - ty * ty * ty + c.y;
                            n++;
                        }
                        r = (byte)(n % 255); // находим цвет;;
                        g = 0;
                        b = 0;

                        //цвет выбирается по числу итераций
                        if (Math.Abs(z.x) >= 50 || Math.Abs(z.y) > 1000)
                            wb.SetPixel(mx + x, my + y, 0, 0, 0); //перекрашиваем соответствующую точку на canvas'e в соответствующий цвет
                        else
                            wb.SetPixel(mx + x, my + y, 255, 82, 0); //перекрашиваем соответствующую точку на canvas'e в соответствующий цвет

                    }
                }
            }
        }
    }
}
