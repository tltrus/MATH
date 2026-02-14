using System.Windows;
using InterpolationVisualizer.ViewModels;

namespace InterpolationVisualizer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadSinData_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                // Use method from ViewModel
                viewModel.DataPoints.Clear();
                for (int i = 0; i <= 5; i++)
                {
                    double x = i;
                    double y = System.Math.Sin(x);
                    viewModel.DataPoints.Add(new Models.DataPoint(x, y));
                }
                viewModel.UpdatePlot();
            }
        }

        private void LoadParabolaData_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                // Use method from ViewModel
                viewModel.DataPoints.Clear();
                for (int i = 0; i <= 5; i++)
                {
                    double x = i;
                    double y = x * x / 5.0;
                    viewModel.DataPoints.Add(new Models.DataPoint(x, y));
                }
                viewModel.UpdatePlot();
            }
        }
    }
}