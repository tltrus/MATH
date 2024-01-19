using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace LeastSquares
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();

        int n;
        List<double> x = new List<double>() { 0, 1, 2, 3, 4, 5 };
        List<double> y = new List<double>() { 2.1, 2.4, 2.6, 2.7, 2.8, 3};
        double sum_x;
        double sum_y;
        List<double> product_xy = new List<double>();
        double sum_product_xy;
        List<double> square_x = new List<double>();
        double sum_square_x;
        double a_1, b_1, a_2, b_2;


        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            Least_squares_1(); // Вариант через производные с сайта http://www.cleverstudents.ru/articles/mnk.html#beginning
            Least_squares_2(); // Вариант простой по упрощенной формуле для пары х и y

            Draw();
        }

        void Least_squares_1()
        {
            /*
             * http://www.cleverstudents.ru/articles/mnk.html#beginning
             */

            // Подготовка данных для расчетов
            n = x.Count;
            for (int i = 0; i < n; ++i)
            {
                sum_x += x[i];

                //y.Add(Math.Pow(x[i] + 1, 1.0 / 3.0) + 1);
                sum_y += y[i];
                
                product_xy.Add(x[i] * y[i]);
                sum_product_xy += x[i] * y[i];

                square_x.Add(x[i] * x[i]);
                sum_square_x += x[i] * x[i];
            }

            // 
            a_1 = (n * sum_product_xy - sum_x * sum_y) /
                        (n * sum_square_x - sum_x * sum_x);

            b_1 = (sum_y - a_1 * sum_x) / n;
        }
        void Least_squares_2()
        {
            // Подготовка данных для расчетов
            n = x.Count;

            var X = x.Sum() / n;
            var Y = y.Sum() / n;

            double numerator = 0;
            double denumerator = 0;
            for (int i = 0; i < n; ++i)
            {
                numerator += (x[i] - X) * (y[i] - Y);
                denumerator += (x[i] - X) * (x[i] - X);
            }

            // 
            a_2 = numerator / denumerator;
            b_2 = Y - a_2 * X;
        }

        void Draw()
        {
            ChartValues<double> chartvalue = new ChartValues<double>();

            for (int i = 0; i < n; ++i)
            {
                chartvalue.Add(y[i]);
            }
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = chartvalue,
                    LineSmoothness = 0
                }
            };
            Labels = new[] { "0", "1", "2", "3", "4", "5" };

            // Вариант 1. через производные с сайта http://www.cleverstudents.ru/articles/mnk.html#beginning
            Draw_Least_squares_1();

            // Вариант 2. простой по упрощенной формуле для пары х и y
            Draw_Least_squares_2();

            DataContext = this;
        }

        void Draw_Least_squares_1()
        {
            ChartValues<double> chartvalue = new ChartValues<double>();
            for (int i = 0; i < n; ++i)
            {
                var y = a_1 * x[i] + b_1;
                chartvalue.Add(y);
            }

            SeriesCollection.Add(new LineSeries
            {
                Title = "Least squares 1",
                Values = chartvalue,
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointForeground = Brushes.Red
            });
        }

        void Draw_Least_squares_2()
        {
            ChartValues<double> chartvalue = new ChartValues<double>();
            for (int i = 0; i < n; ++i)
            {
                var y = a_2 * x[i] + b_2;
                chartvalue.Add(y);
            }

            SeriesCollection.Add(new LineSeries
            {
                Title = "Least squares 2",
                Values = chartvalue,
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointForeground = Brushes.Green
            });
        }

    }
}
