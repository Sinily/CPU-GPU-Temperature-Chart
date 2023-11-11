using System; // basic functionality
using System.ComponentModel; // INotifyPropertyChanged
using System.Linq; // Where()
using System.Windows; // WPF functionality
using System.Windows.Controls; // Grids
using System.Windows.Input; // mouse button input events
using System.Windows.Threading; // timer
// https://github.com/Live-Charts/Live-Charts \\
    using LiveCharts; // SeriesCollection()
    using LiveCharts.Defaults; // ObservableValue()
    using LiveCharts.Wpf; // LineSeries()
// https://github.com/openhardwaremonitor/openhardwaremonitor \\
    using OpenHardwareMonitor.Hardware; // CPU/GPU Sensor Temperatures

namespace YourWPFProject
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Computer computer;

        // window data
        public SeriesCollection SeriesCollection { get; set; } // store chart data
        public Func<double, string> TimeFormatter { get; set; } // format time labels
        public string GraphTitle { get; set; } // chart window title
        private DispatcherTimer timer = new DispatcherTimer(); // chart runtime

        // grid dragging data
        private bool isDragging = false; // is grid being dragged?
        private Point offset; // current position
        private Grid currentGrid; // which grid is being dragged


        #region Chart Window

        // WINDOW OPENED
        public MainWindow()
        {
            InitializeComponent();

            // CPU and GPU chart lines
            SeriesCollection = new SeriesCollection{
                new LineSeries {
                    Title = "CPU",
                    Values = new ChartValues<ObservableValue>(),
                    PointGeometry = null,
                    LineSmoothness = 0.2,
                },
                new LineSeries {
                    Title = "GPU",
                    Values = new ChartValues<ObservableValue>(),
                    PointGeometry = null,
                    LineSmoothness = 0.2,
                }
            };

            // hide time values
            TimeFormatter = value => "";

            DataContext = this;

            // open hardware monitor
            computer = new Computer { GPUEnabled = true, CPUEnabled = true };
            computer.Open();

            // update window every second
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => UpdateChart();
            timer.Start();

            // chart title
            if (GraphTitle == null)
                GraphTitle = "[Project - Computer Peripherals] CPU & GPU Temperature Graph";

            // grid dragging
            AttachMouseHandlers(legendGrid); // legend
            AttachMouseHandlers(gpuTempGrid); // gpu temperatures
            AttachMouseHandlers(cpuTempGrid); // cpu temperatures
        }


        // WINDOW CLOSED
        protected override void OnClosed(EventArgs e)
        {
            timer.Stop();
            base.OnClosed(e);
        }


        // LOADING CHART
        private void chart_Loaded(object sender, RoutedEventArgs e) { }

        #endregion


        #region Chart Update

        // UPDATING TEMPERATURE VALUES
        private float? minCTemp = 120;
        private float? maxCTemp = 0;
        private float? minGTemp = 120;
        private float? maxGTemp = 0;
        private int count = 0;

        private void UpdateChart()
        {
            computer.Hardware[0].Update(); // cpu
            computer.Hardware[1].Update(); // gpu

            // getting hardware gpu and cpu
            var cpuSensors = computer.Hardware.Where(h => h.HardwareType == HardwareType.CPU).SelectMany(h => h.Sensors);
            var gpuSensors = computer.Hardware.Where(h => h.HardwareType == HardwareType.GpuNvidia || h.HardwareType == HardwareType.GpuAti).SelectMany(h => h.Sensors);
            var cpuSensor = default(ISensor);
            var gpuSensor = default(ISensor);

            // updating current cpu temperature
            foreach (var sensor in cpuSensors)
                if (sensor.SensorType == SensorType.Temperature && sensor.Name == "CPU Package")
                {
                    cpuSensor = sensor;

                    // updating min/max values
                    if (minCTemp > sensor.Value)
                        minCTemp = sensor.Value;
                    if (maxCTemp < sensor.Value)
                        maxCTemp = sensor.Value;
                }

            // getting current gpu temperature
            foreach (var sensor in gpuSensors)
                if (sensor.SensorType == SensorType.Temperature)
                {
                    gpuSensor = sensor;

                    // uodating min/max values
                    if (minGTemp > sensor.Value)
                        minGTemp = sensor.Value;
                    if (maxGTemp < sensor.Value)
                        maxGTemp = sensor.Value;
                }

            // adding current values to chart
            if (cpuSensor != null && gpuSensor != null)
            {
                ((ChartValues<ObservableValue>)SeriesCollection[0].Values).Add(new ObservableValue((double)cpuSensor.Value));
                ((ChartValues<ObservableValue>)SeriesCollection[1].Values).Add(new ObservableValue((double)gpuSensor.Value));

                // deleting old values
                if (count > 59)
                {
                    ((ChartValues<ObservableValue>)SeriesCollection[0].Values).RemoveAt(0);
                    ((ChartValues<ObservableValue>)SeriesCollection[1].Values).RemoveAt(0);
                }

                UpdateTempMinMax();

                // critical temperature warnings
                if (cpuSensor.Value > 100)
                    ShowCPUTemperatureWarning();
                else HideCPUTemperatureWarning();

                if (gpuSensor.Value > 80)
                    ShowGPUTemperatureWarning();
                else HideGPUTemperatureWarning();
            }

            // continuing to next chart values
            count++;
        }

        #endregion


        #region Min/Max Temperatures

        // CHECK FOR MIN/MAX VALUE UPDATES
        private string _cpuMinTemp;
        private string _cpuMaxTemp;
        private string _gpuMinTemp;
        private string _gpuMaxTemp;

        public string cpuMinTemp
        {
            get { return _cpuMinTemp; } // get current temp value
            set
            {
                if (_cpuMinTemp != value) // if current value != old value
                {
                    _cpuMinTemp = value;
                    OnPropertyChanged("cpuMinTemp");
                }
            }
        }
        public string cpuMaxTemp
        {
            get { return _cpuMaxTemp; }
            set {
                if (_cpuMaxTemp != value) 
                {
                    _cpuMaxTemp = value;
                    OnPropertyChanged("cpuMaxTemp");
                }
            }
        }
        public string gpuMinTemp
        {
            get { return _gpuMinTemp; }
            set {
                if (_gpuMinTemp != value) 
                {
                    _gpuMinTemp = value;
                    OnPropertyChanged("gpuMinTemp");
                }
            }
        }
        public string gpuMaxTemp
        {
            get { return _gpuMaxTemp; }
            set { if (_gpuMaxTemp != value) {
                    _gpuMaxTemp = value;
                    OnPropertyChanged("gpuMaxTemp");
                }
            }
        }


        // NOTIFY MIN/MAX VALUE CHANGES
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // UPDATE MIN/MAX TEMPERATURES
        private void UpdateTempMinMax()
        {
            var cpuSensors = computer.Hardware
                .Where(h => h.HardwareType == HardwareType.CPU)
                .SelectMany(h => h.Sensors)
                .Where(sensor => sensor.SensorType == SensorType.Temperature && sensor.Name == "CPU Package")
                .ToList();
            var gpuSensors = computer.Hardware
                .Where(h => h.HardwareType == HardwareType.GpuNvidia || h.HardwareType == HardwareType.GpuAti)
                .SelectMany(h => h.Sensors)
                .Where(sensor => sensor.SensorType == SensorType.Temperature)
                .ToList();

            // formatting
            if (cpuSensors.Count > 0 && gpuSensors.Count > 0)
            {
                cpuMinTemp = string.Format("{0:0}°C", minCTemp);
                cpuMaxTemp = string.Format("{0:0}°C", maxCTemp);
                gpuMinTemp = string.Format("{0:0}°C", minGTemp);
                gpuMaxTemp = string.Format("{0:0}°C", maxGTemp);
            }
        }

        #endregion


        #region Show/Hide Grids

        // MIN/MAX TEMPERATURES
        private void ShowCpuTemperatures(object sender, RoutedEventArgs e)
        {
            cpuTempGrid.Visibility = Visibility.Visible;
            UpdateTempMinMax();
        }
        private void HideCpuTemperatures(object sender, RoutedEventArgs e) { cpuTempGrid.Visibility = Visibility.Collapsed; }
        private void ShowGpuTemperatures(object sender, RoutedEventArgs e)
        {
            gpuTempGrid.Visibility = Visibility.Visible;
            UpdateTempMinMax();
        }
        private void HideGpuTemperatures(object sender, RoutedEventArgs e) { gpuTempGrid.Visibility = Visibility.Collapsed; }


        // TEMPERATURE WARNINGS
        private void ShowCPUTemperatureWarning()
        {
            warningCpuGrid.Visibility = Visibility.Visible;
            System.Media.SystemSounds.Exclamation.Play();
        }
        private void HideCPUTemperatureWarning() { warningCpuGrid.Visibility = Visibility.Collapsed; }
        private void ShowGPUTemperatureWarning()
        {
            warningGpuGrid.Visibility = Visibility.Visible;
            System.Media.SystemSounds.Exclamation.Play();
        }
        private void HideGPUTemperatureWarning() { warningGpuGrid.Visibility = Visibility.Collapsed; }

        #endregion


        #region Grid Draggability

        private void AttachMouseHandlers(Grid grid) // apply dragging functionality to a grid
        {
            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
            grid.MouseMove += Grid_MouseMove;
            grid.MouseLeftButtonUp += Grid_MouseLeftButtonUp;
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) // grid left click
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = true;
                offset = e.GetPosition((Grid)sender);
                currentGrid = (Grid)sender;
                currentGrid.CaptureMouse();
            }
        }
        private void Grid_MouseMove(object sender, MouseEventArgs e) // mouse movement
        {
            if (isDragging && currentGrid != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point newPoint = e.GetPosition(chart);
                    currentGrid.Margin = new Thickness(newPoint.X - offset.X, newPoint.Y - offset.Y, 0, 0);
                }
                else
                {
                    currentGrid.ReleaseMouseCapture();
                    isDragging = false;
                    currentGrid = null;
                }
            }
        }
        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) // mouse release left click
        {
            if (isDragging && currentGrid != null)
            {
                currentGrid.ReleaseMouseCapture();
                isDragging = false;
                currentGrid = null;
            }
        }

        #endregion
    }
}
