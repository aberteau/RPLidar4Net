﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RPLidar4Net.Core;
using System.Windows.Shapes;
using RPLidar4Net.Core.Api;
using RPLidarSerial;
using RPLidarSerial.RPLidar;

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

            //StartScan();
        }

        private void StartScan()
        {
            _rpLidar = new RPLidarSerialDevice("com3");
            //Connect RPLidar
            _rpLidar.Connect();
            //Reset - Not really sure how this is supposed to work, reconnecting USB works too
            //RPLidar.Reset();
            //Stop motor
            _rpLidar.StopMotor();
            //Get Device Information
            InformationResponse responseInformation = _rpLidar.GetDeviceInfo();
            //Get Device Health
            _rpLidar.GetDeviceHealth();
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
            MeasurementNode[] measurementNodes = eventArgs.Nodes.ToArray();
            if (measurementNodes.Any())
            {
                mainCanvas.Children.Clear();
                DrawOriginEllipse();
                DrawNodeEllipses(measurementNodes);
            }
        }

        private void DrawNodeEllipses(MeasurementNode[] measurementNodes)
        {
            foreach (MeasurementNode measurementNode in measurementNodes)
            {
                System.Drawing.PointF pointF = PointHelper.ToPointF(_origin, Rotation, measurementNode.Angle, measurementNode.Distance);
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

        private async Task LoadDataAsync()
        {
            IEnumerable<Core.Point> points = await PointHelper.ReadPointsAsync(Path);
            foreach (Core.Point point in points)
            {
                System.Drawing.PointF pointF = PointHelper.ToPointF(_origin, Rotation, point.Angle, point.Distance);
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
