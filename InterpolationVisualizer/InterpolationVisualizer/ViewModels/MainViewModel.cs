using InterpolationVisualizer.Models;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using DataPoint = InterpolationVisualizer.Models.DataPoint;

namespace InterpolationVisualizer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DataPoint> _dataPoints;
        private ObservableCollection<InterpolationMethod> _methods;
        private double _startSlope = 0;
        private double _endSlope = 0;
        private double _interpolationStep = 0.01;
        private PlotModel _plotModel;

        public MainViewModel()
        {
            // Data initialization
            DataPoints = new ObservableCollection<DataPoint>
            {
                new DataPoint(0, 1),
                new DataPoint(1, 3),
                new DataPoint(2, 2),
                new DataPoint(3, 4),
                new DataPoint(4, 1)
            };

            // Interpolation methods initialization
            Methods = new ObservableCollection<InterpolationMethod>
            {
                new InterpolationMethod { Name = "Linear", IsSelected = true,
                    Color = OxyColor.FromRgb(0, 0, 255), LineStyle = LineStyle.Solid }, // Blue
                new InterpolationMethod { Name = "Lagrange", IsSelected = true,
                    Color = OxyColor.FromRgb(0, 128, 0), LineStyle = LineStyle.Dash }, // Green
                new InterpolationMethod { Name = "Barycentric", IsSelected = false,
                    Color = OxyColor.FromRgb(255, 165, 0), LineStyle = LineStyle.Dot }, // Orange
                new InterpolationMethod { Name = "Newton", IsSelected = false,
                    Color = OxyColor.FromRgb(128, 0, 128), LineStyle = LineStyle.DashDot }, // Purple
                new InterpolationMethod { Name = "Natural Cubic Spline", IsSelected = true,
                    Color = OxyColor.FromRgb(255, 0, 0), LineStyle = LineStyle.Solid }, // Red
                new InterpolationMethod { Name = "Clamped Cubic Spline", IsSelected = false,
                    Color = OxyColor.FromRgb(165, 42, 42), LineStyle = LineStyle.Dash } // Brown
            };

            // Commands
            AddPointCommand = new RelayCommand(AddRandomPoint);
            ClearPointsCommand = new RelayCommand(ClearPoints);
            LoadSampleDataCommand = new RelayCommand(LoadSampleData);

            // Plot initialization
            PlotModel = CreatePlotModel();
            UpdatePlot();

            // Create WeakEventManager to subscribe to all methods
            foreach (var method in Methods)
            {
                method.PropertyChanged += OnMethodPropertyChanged;
            }
        }

        public ObservableCollection<DataPoint> DataPoints
        {
            get => _dataPoints;
            set
            {
                _dataPoints = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<InterpolationMethod> Methods
        {
            get => _methods;
            set
            {
                _methods = value;
                OnPropertyChanged();
            }
        }

        public double StartSlope
        {
            get => _startSlope;
            set
            {
                _startSlope = value;
                OnPropertyChanged();
            }
        }

        public double EndSlope
        {
            get => _endSlope;
            set
            {
                _endSlope = value;
                OnPropertyChanged();
            }
        }

        public double InterpolationStep
        {
            get => _interpolationStep;
            set
            {
                _interpolationStep = value;
                OnPropertyChanged();
            }
        }

        public PlotModel PlotModel
        {
            get => _plotModel;
            set
            {
                _plotModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPointCommand { get; }
        public ICommand ClearPointsCommand { get; }
        public ICommand LoadSampleDataCommand { get; }

        private PlotModel CreatePlotModel()
        {
            var plotModel = new PlotModel
            {
                Title = "Interpolation Methods",
                Subtitle = "Chapter 7: Interpolation (from Book Numerical Methods, Algorithms and Tools in C# / Waldemar Dos Passos",
                PlotMargins = new OxyThickness(50, 20, 20, 40)
            };

            var legend = new Legend
            {
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical
            };
            plotModel.Legends.Add(legend);

            plotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "X",
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                MinorGridlineColor = OxyColor.FromRgb(240, 240, 240)
            });

            plotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "Y",
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                MinorGridlineColor = OxyColor.FromRgb(240, 240, 240)
            });

            return plotModel;
        }

        public void UpdatePlot()
        {
            if (DataPoints.Count < 2) return;

            // Clear previous series
            PlotModel.Series.Clear();

            // Add original points
            var scatterSeries = new ScatterSeries
            {
                Title = "Original Points",
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerFill = OxyColors.Black,
                MarkerStroke = OxyColors.Black,
                MarkerStrokeThickness = 1
            };

            foreach (var point in DataPoints)
            {
                scatterSeries.Points.Add(new ScatterPoint(point.X, point.Y));
            }
            PlotModel.Series.Add(scatterSeries);

            // Add selected interpolation methods
            foreach (var method in Methods.Where(m => m.IsSelected))
            {
                List<Models.DataPoint> interpolatedPoints = GetInterpolatedPoints(method.Name);

                if (interpolatedPoints.Any())
                {
                    var lineSeries = new LineSeries
                    {
                        Title = method.Name,
                        Color = method.Color,
                        StrokeThickness = 2,
                        LineStyle = method.LineStyle,
                        MarkerType = MarkerType.None
                    };

                    foreach (var point in interpolatedPoints)
                    {
                        lineSeries.Points.Add(new OxyPlot.DataPoint(point.X, point.Y));
                    }

                    PlotModel.Series.Add(lineSeries);
                }
            }

            // Update plot
            PlotModel.InvalidatePlot(true);
        }

        private List<DataPoint> GetInterpolatedPoints(string methodName)
        {
            var points = DataPoints.ToList();

            return methodName switch
            {
                "Linear" => InterpolationMethods.LinearInterpolation(points, InterpolationStep),
                "Lagrange" => InterpolationMethods.LagrangeInterpolation(points, InterpolationStep),
                "Barycentric" => InterpolationMethods.BarycentricInterpolation(points, InterpolationStep),
                "Newton" => InterpolationMethods.NewtonInterpolation(points, InterpolationStep),
                "Natural Cubic Spline" => InterpolationMethods.NaturalCubicSpline(points, InterpolationStep),
                "Clamped Cubic Spline" => InterpolationMethods.ClampedCubicSpline(points, StartSlope, EndSlope, InterpolationStep),
                _ => new List<Models.DataPoint>()
            };
        }

        private void AddRandomPoint()
        {
            Random rand = new Random();
            double x = Math.Round(rand.NextDouble() * 5, 2);
            double y = Math.Round(rand.NextDouble() * 5, 2);
            DataPoints.Add(new DataPoint(x, y));
            UpdatePlot();
        }

        private void ClearPoints()
        {
            DataPoints.Clear();
            PlotModel.Series.Clear();
            PlotModel.InvalidatePlot(true);
        }

        private void LoadSampleData()
        {
            DataPoints.Clear();
            var samplePoints = new List<DataPoint>
            {
                new DataPoint(0, 0),
                new DataPoint(1, 1),
                new DataPoint(2, 0.5),
                new DataPoint(3, 2),
                new DataPoint(4, 1.5),
                new DataPoint(5, 0.8)
            };

            foreach (var point in samplePoints)
            {
                DataPoints.Add(point);
            }

            UpdatePlot();
        }

        private void OnMethodPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InterpolationMethod.IsSelected))
            {
                // Small delay to group rapid changes
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    UpdatePlot();
                }), DispatcherPriority.Background);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class InterpolationMethod : INotifyPropertyChanged
    {
        private string _name;
        private bool _isSelected;
        private OxyColor _color;
        private LineStyle _lineStyle;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public OxyColor Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public LineStyle LineStyle
        {
            get => _lineStyle;
            set
            {
                _lineStyle = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();
    }
}