using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RPLidar4Net.Core;
using System.Windows.Shapes;

namespace RPLidar4Net.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private const string Path = @"F:\UserData\Amael\OneDrive\R&D\Lidar\RPLIDAR A1\Scan Data\RoboStudio\200212 2347.txt";
        private const string Path = @"F:\UserData\Amael\OneDrive\R&D\Lidar\RPLIDAR A1\Scan Data\SlamtecRobopeakLidar\200213 0033.txt";
        private System.Drawing.PointF _origin;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawOriginEllipse()
        {
            float originX = Convert.ToSingle(mainCanvas.ActualWidth / 2);
            float originY = Convert.ToSingle(mainCanvas.ActualHeight / 2);
            _origin = new System.Drawing.PointF(originX, originY);

            DrawEllipse(_origin, Colors.Red);
        }

        private async Task LoadDataAsync()
        {
            IEnumerable<Core.Point> points = await PointHelper.ReadPointsAsync(Path);
            foreach (Core.Point point in points)
            {
                System.Drawing.PointF pointF = PointHelper.ToPointF(_origin, point.Angle, point.Distance);
                DrawEllipse(pointF, Colors.Black);
            }
        }

        private void DrawEllipse(System.Drawing.PointF pointF, Color color)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 10;
            ellipse.Height = 10;
            ellipse.Fill = new SolidColorBrush(color);
 
            Canvas.SetLeft(ellipse, pointF.X);
            Canvas.SetTop(ellipse, pointF.Y);

            mainCanvas.Children.Add(ellipse);
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            mainCanvas.Children.Clear();
            DrawOriginEllipse();
            LoadDataAsync();
        }
    }
}
