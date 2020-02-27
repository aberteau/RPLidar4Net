using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RPLidar4Net.Core;
using System.Windows.Shapes;
using RPLidar4Net.Core.Api;
using RPLidarSerial;

namespace RPLidar4Net.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private const string Path = @"F:\UserData\Amael\OneDrive\Electronique\RPLIDAR A1\Scan Data\RoboStudio\200212 2347.txt";
        private const string Path = @"F:\UserData\Amael\OneDrive\Electronique\RPLIDAR A1\Scan Data\SlamtecRobopeakLidar\200223 222514 396.txt";
        private const int Rotation = -90;
        private System.Drawing.PointF _origin;

        static RPLidarSerialDevice _rpLidar;

        public MainWindow()
        {
            InitializeComponent();

            StartScan();
        }

        private void StartScan()
        {
            _rpLidar = new RPLidarSerialDevice("com6");
            //Connect RPLidar
            _rpLidar.Connect();
            //Reset - Not really sure how this is supposed to work, reconnecting USB works too
            //RPLidar.Reset();
            //Stop motor
            _rpLidar.StopMotor();
            //Get Device Information
            InfoDataResponse infoDataResponse = _rpLidar.GetInfo();
            //Get Device Health
            HealthDataResponse healthDataResponse = _rpLidar.GetHealth();
            //Get Data Event
            _rpLidar.NewScan += RPLidar_NewScan;
            //Start Scan Thread
            _rpLidar.StartScan();
        }

        private void RPLidar_NewScan(object sender, NewScanEventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() => UpdateCanvas(eventArgs));
        }

        private void UpdateCanvas(NewScanEventArgs eventArgs)
        {
            UpdateCanvas(eventArgs.Points);
        }

        private void UpdateCanvas(IEnumerable<Core.Point> points)
        {
            mainCanvas.Children.Clear();
            DrawOriginEllipse();
            DrawEllipses(points);
        }

        private void DrawEllipses(IEnumerable<Core.Point> points)
        {
            foreach (Core.Point point in points)
            {
                System.Drawing.PointF pointF = PointHelper.ToPointF(_origin, Rotation, point);
                DrawEllipse(pointF, Colors.Black);
            }
        }

        private void DrawOriginEllipse()
        {
            float originX = Convert.ToSingle(mainCanvas.ActualWidth / 2);
            float originY = Convert.ToSingle(mainCanvas.ActualHeight / 2);
            _origin = new System.Drawing.PointF(originX, originY);

            DrawEllipse(_origin, Colors.Red);
        }

        private void DrawEllipse(System.Drawing.PointF pointF, Color color)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 30;
            ellipse.Height = 30;
            ellipse.Fill = new SolidColorBrush(color);
 
            Canvas.SetLeft(ellipse, pointF.X);
            Canvas.SetTop(ellipse, pointF.Y);

            mainCanvas.Children.Add(ellipse);
        }

        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<Core.Point> points = await PointHelper.ReadPointsAsync(Path);
            UpdateCanvas(points);
        }
    }
}
