using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    class Fractal
    {
        class TComplex
        {
            public double x, y;
        }

        private int iter = 80;
        private int max = 16;

        TComplex z = new TComplex();
        TComplex c = new TComplex();
        byte n;
        private int mx, my;
        WriteableBitmap _wb;
        private byte _type; // 0 - Мандельброт, 1 - Лямбда, 2 - Жулиа1, 3 - Жулиа2, 

        byte r;
        byte g;
        byte b;

        double zoom = 0.9; // увеличение\уменьшение изображения [0 ... 1]


        public Fractal(WriteableBitmap wb, int width, int height, byte type)
        {
            _wb = wb;
            mx = (int)(width / 2);  // Деление пополам 
            my = (int)(height / 2);  // Деление пополам
            _type = type;
            main();
        }

        private void main()
        {
            double dx = 0, dy = 0; // для случая 4 [Бассейн Ньютона 1]

            _wb.Clear(Colors.White);

            for (int y = -my; y < my; y++)
            {
                for (int x = -mx; x < mx; x++)
                {
                    n = 0;

                    // Выбор типа фрактала
                    switch(_type)   // 0 - Мандельброт, 1 - Лямбда, 2 - Жулиа, 3 - Жулиа2,
                    {
                        case 0:
                            //устанавливаем   значения параметров для Мондельбота
                            c.x = x * 0.005 * zoom;
                            c.y = y * 0.005 * zoom;
                            z.x = z.y = 0;

                            break;
                        case 1:
                            //устанавливаем   значения параметров для Лямбда-фрактала
                            z.x = 0.5;
                            z.y = 0;
                            c.x = (x * 0.01 + 1) * zoom;
                            c.y = y * 0.01 * zoom;

                            break;
                        case 2:
                            //устанавливаем   значения параметров для Жулиа 1
                            z.x = x * 0.005 * zoom;
                            z.y = y * 0.005 * zoom;
                            c.x = 0.11;
                            c.y = -0.66;

                            break;

                        case 3:
                            //устанавливаем   значения параметров для Жулиа 2
                            z.x = x * 0.005 * zoom;
                            z.y = y * 0.005 * zoom;
                            c.x = -0.70176;
                            c.y = -0.3842;

                            iter = 300;

                            break;

                        case 4:
                            //устанавливаем   значения параметров для Бассейна Ньютона 1
                            z.x = x * 0.005 * zoom;
                            z.y = y * 0.005 * zoom;
                            dx = z.x; dy = z.y;

                            iter = 50;

                            break;
                    }
                     
                    // Логика взависимости от типа фрактала
                    if (_type == 0 || _type == 2)
                    {
                        // для Жулиа and Мондельбота
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

                    } else // Жулиа 1 and Мондельбота

                    if (_type == 1)
                    {
                        // для Лямбды
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
                    } else // Лямбда

                    if(_type == 3)
                    {
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
                    } else // Жулиа 2

                    if (_type == 4)
                    {
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
                    } // Бассейн Ньютона 1

                    //цвет выбирается по числу итераций
                    if (n < iter)
                        _wb.SetPixel(mx + x, my + y, r, g, b); //перекрашиваем соответствующую точку на canvas'e в соответствующий цвет
                }
            }
        }



        /// *************************************************
        /// АЛЬТЕРНАТИВНАЯ ФУНКЦИЯ СОЗДАНИЯ ФРАКТАЛА ЖУЛИА 2
        /// *************************************************
        public void FractalJulia(int w, int h, WriteableBitmap g)
        {
            // при каждой итерации, вычисляется znew = zold² + С

            // вещественная  и мнимая части постоянной C
            double cRe, cIm;
            // вещественная и мнимая части старой и новой
            double newRe, newIm, oldRe, oldIm;
            // Можно увеличивать и изменять положение
            double zoom = 1, moveX = 0, moveY = 0;
            //Определяем после какого числа итераций функция должна прекратить свою работу
            int maxIterations = 300;

            //выбираем несколько значений константы С, это определяет форму фрактала         Жюлиа
            cRe = -0.70176;
            cIm = -0.3842;


            //"перебираем" каждый пиксель
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    //вычисляется реальная и мнимая части числа z
                    //на основе расположения пикселей,масштабирования и значения позиции
                    newRe = 1.5 * (x - w / 2) / (0.5 * zoom * w) + moveX;
                    newIm = (y - h / 2) / (0.5 * zoom * h) + moveY;

                    //i представляет собой число итераций 
                    byte i;

                    //начинается процесс итерации
                    for (i = 0; i < maxIterations; i++)
                    {

                        //Запоминаем значение предыдущей итерации
                        oldRe = newRe;
                        oldIm = newIm;

                        // в текущей итерации вычисляются действительная и мнимая части 
                        newRe = oldRe * oldRe - oldIm * oldIm + cRe;
                        newIm = 2 * oldRe * oldIm + cIm;

                        // если точка находится вне круга с радиусом 2 - прерываемся
                        if ((newRe * newRe + newIm * newIm) > 4) break;
                    }

                    //определяем цвета
                    Color ass = Color.FromRgb((byte)((i * 9) % 255), 0, (byte)((i * 9) % 255));
                    //рисуем пиксель
                    g.FillRectangle(x, y, x + 1, y + 1, ass);
                }
        }

        /// *************************************************
        /// ФУНКЦИЯ СОЗДАНИЯ ГЕОМЕТРИЧЕСКОГО ФРАКТАЛА Дракон Хартера — Хейтуэя
        /// *************************************************
        #region ФРАКТАЛ Дракон Хартера — Хейтуэя

        public static void FractalDragonHartera(WriteableBitmap wb)
        {
            wb.Clear(Colors.White);

            int x1 = 200;
            int y1 = 300;
            int x2 = 500;
            int y2 = 300;
            int k = 20;

            DragonPaint(x1, y1, x2, y2, k, wb);
        }
        private static void DragonPaint(int x1, int y1, int x2, int y2, int k, WriteableBitmap wb)
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
                DragonPaint(x2, y2, tx, ty, k - 1, wb);
                DragonPaint(x1, y1, tx, ty, k - 1, wb);
            }
            else
            {
                wb.DrawLine(x1, y1, x2, y2, color);
            }
        }
        #endregion

        /// *************************************************
        /// ФУНКЦИЯ СОЗДАНИЯ ГЕОМЕТРИЧЕСКОГО ФРАКТАЛА Дерево Пифагора
        /// *************************************************
        #region ФРАКТАЛ Дерево Пифагора

        public static void FractalPifagorTree(WriteableBitmap wb)
        {
            wb.Clear(Colors.White);
            double angle = Math.PI / 2; //Угол поворота на 90 градусов

            PifagorTreePaint(300, 450, 200, angle, wb);
        }

        //Рекурсивная функция отрисовки дерева
        //x и y - координаты родительской вершины
        //a - параметр, который фиксирует количество итераций в рекурсии
        //angle - угол поворота на каждой итерации
        private static void PifagorTreePaint(double x, double y, double a, double angle, WriteableBitmap wb)
        {
            double ang1 = Math.PI / 4;  //Угол поворота на 45 градусов
            double ang2 = Math.PI / 6;  //Угол поворота на 30 градусов

            if (a > 2)
            {
                a *= 0.7; //Меняем параметр a

                //Считаем координаты для вершины-ребенка
                double xnew = Math.Round(x + a * Math.Cos(angle)),
                       ynew = Math.Round(y - a * Math.Sin(angle));

                //рисуем линию между вершинами
                wb.DrawLine((int)x, (int)y, (int)xnew, (int)ynew, Colors.Red);

                //Переприсваеваем координаты
                x = xnew;
                y = ynew;

                //Вызываем рекурсивную функцию для левого и правого ребенка
                PifagorTreePaint(x, y, a, angle + ang1, wb);
                PifagorTreePaint(x, y, a, angle - ang2, wb);
            }
        }

        #endregion
    }
}
