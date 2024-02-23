using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fractals
{
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        WriteableBitmap wb;
        int width, height;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            width = (int)image.Width;
            height = (int)image.Height;

            // Для Битмапа
            wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            image.Source = wb;

            Fractal mandelbrot = new Fractal(wb, width, height, (byte)rnd.Next(0, 4));
        }

        // Алгебраические фракталы:
        private void Mandelbrot_Click(object sender, RoutedEventArgs e)
        {
            Fractal fractal = new Fractal(wb, width, height, 0);
        }
        private void Lambda_Click(object sender, RoutedEventArgs e)
        {
            Fractal fractal = new Fractal(wb, width, height, 1);
        }
        private void Julia1_Click(object sender, RoutedEventArgs e)
        {
            Fractal fractal = new Fractal(wb, width, height, 2);
        }
        private void Julia2_Click(object sender, RoutedEventArgs e)
        {
            Fractal fractal = new Fractal(wb, width, height, 3);
        }
        private void Newton1_Click(object sender, RoutedEventArgs e)
        {
            Fractal fractal = new Fractal(wb, width, height, 4);
        }

        // Геометрические: Дракон
        private void Dragon_Click(object sender, RoutedEventArgs e) => Fractal.FractalDragonHartera(wb);

        private void PifagorTree_Click(object sender, RoutedEventArgs e) => Fractal.FractalPifagorTree(wb);

        // Стохастические:
        private void Paporotnik_Click(object sender, RoutedEventArgs e)
        {
            double x = 0, y = 0;
            double a = 0, b = 0, c = 0, d = 0, _e = 0, f = 0, X1 = 0, Y1 = 0;

            wb.Clear(Colors.White);

            for (int i = 1; i < 90000; i++)
            {
                double r = rnd.NextDouble();

                if (r >= 0 && r < 0.01) { a = 0; b = 0; c = 0; d = 0.16; _e = 0; f = 0; }
                else
                if (r >= 0.01 && r < 0.8) { a = 0.85; b = 0.04; c = -0.04; d = 0.85; _e = 0; f = 1.6; }
                else
                if (r >= 0.8 && r < 0.9) { a = 0.2; b = -0.26; c = 0.23; d = 0.22; _e = 0; f = 1.6; }
                else
                if (r >= 0.9 && r < 1) { a = -0.15; b = 0.28; c = 0.26; d = 0.24; _e = 0; f = 0.44; }

                X1 = (a * x) + (b * y) + _e;
                Y1 = (c * x) + (d * y) + f;
                x = X1;
                y = Y1;

                wb.SetPixel((int)(X1 * 40 + image.Width / 2), (int)(Y1 * 40 + 50), 0, 100, 0);

                // 
                //if (r < 1 / 3)
                //{
                //    x = 0.5 * x;
                //    y = 0.5 * y;
                //}
                //else
                //if (r >= 1 / 3 && r < 2 / 3)
                //{
                //    x = 0.5 * x;
                //    y = 0.5 * y + 300;
                //}
                //else
                //if (r >= 2 / 3)
                //{
                //    x = 0.5 * x + 150;
                //    y = 0.5 * y + 150;
                //}
                //wb.SetPixel((int)(y + image.Width / 2), (int)(x + 50), 0, 100, 0);
                //wb.FillRectangle((int)(y), (int)(x), (int)x + 2, (int)y + 2, Colors.Black);
            }

        }

        // Biomorphs
        private void Bio1_Click(object sender, RoutedEventArgs e)
        {
            Biomorph fractal = new Biomorph(wb, width, height, 0);
        }

        private void Bio2_Click(object sender, RoutedEventArgs e)
        {
            Biomorph fractal = new Biomorph(wb, width, height, 1);
        }
    }
}
