using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    internal class Pifagor_Tree
    {
        public Pifagor_Tree(WriteableBitmap wb)
        {
            wb.Clear(Colors.White);
            double angle = Math.PI / 2; //Угол поворота на 90 градусов

            Draw(300, 450, 200, angle, wb);
        }

        //Рекурсивная функция отрисовки дерева
        //x и y - координаты родительской вершины
        //a - параметр, который фиксирует количество итераций в рекурсии
        //angle - угол поворота на каждой итерации
        void Draw(double x, double y, double a, double angle, WriteableBitmap wb)
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
                Draw(x, y, a, angle + ang1, wb);
                Draw(x, y, a, angle - ang2, wb);
            }
        }
    }
}
