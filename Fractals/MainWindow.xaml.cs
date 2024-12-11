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

        Mandelbrot Mandelbrot_fractal;
        Lambda Lambda_fractal;
        Julia_1 Julia1_fractal;
        Julia_2 Julia2_fractal;
        Newton Newton_fractal;
        Harter_Dragon Harter_Dragon;
        Pifagor_Tree Pifagor_Tree;

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            width = (int)image.Width;
            height = (int)image.Height;

            // Для Битмапа
            wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            image.Source = wb;

            Mandelbrot_fractal = new Mandelbrot(wb, width, height);
            Mandelbrot_fractal.Draw();
        }

        // Алгебраические фракталы:
        private void Mandelbrot_Click(object sender, RoutedEventArgs e)
        {
            Mandelbrot_fractal = new Mandelbrot(wb, width, height);
            Mandelbrot_fractal.Draw();
        }
        private void Lambda_Click(object sender, RoutedEventArgs e)
        {
            Lambda_fractal = new Lambda(wb, width, height);
            Lambda_fractal.Draw();
        }
        private void Julia1_Click(object sender, RoutedEventArgs e)
        {
            Julia1_fractal = new Julia_1(wb, width, height);
            Julia1_fractal.Draw();
        }
        private void Julia2_Click(object sender, RoutedEventArgs e)
        {
            Julia2_fractal = new Julia_2(wb, width, height);
            Julia2_fractal.Draw();
        }
        private void Newton1_Click(object sender, RoutedEventArgs e)
        {
            Newton_fractal = new Newton(wb, width, height);
            Newton_fractal.Draw();
        }

        // Геометрические: Дракон
        private void Dragon_Click(object sender, RoutedEventArgs e)
        {
            Harter_Dragon = new Harter_Dragon(wb);
        }

        private void PifagorTree_Click(object sender, RoutedEventArgs e)
        {
            Pifagor_Tree = new Pifagor_Tree(wb);
        }

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
            }

        }

        // Biomorphs
        private void Bio1_Click(object sender, RoutedEventArgs e)
        {
            Biomorph bio = new Biomorph(wb, width, height, 0);
        }

        private void Bio2_Click(object sender, RoutedEventArgs e)
        {
            Biomorph bio = new Biomorph(wb, width, height, 1);
        }
    }
}
