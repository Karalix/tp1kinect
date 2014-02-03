//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.ColorBasics
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        internal KinectStream mStream;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }


        private void ButtonColorClick(object sender, RoutedEventArgs e)
        {
            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.ConnectDeviceFirst;
                return;
            }
            if (this.mStream != null)
            {
                this.mStream.Close();
            }

            this.mStream = new ColorStream();


            this.mStream.Init(this.sensor, this.Image);

            this.statusBarText.Text = "Vue en couleur";

        }

        private void ButtonDepthClick(object sender, RoutedEventArgs e)
        {
            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.ConnectDeviceFirst;
                return;
            }
            if (this.mStream != null)
            {
                this.mStream.Close();
            }

            this.mStream = new DepthStream();


            this.mStream.Init(this.sensor, this.Image);

            this.statusBarText.Text = "Vue en profondeur";

        }

        private void ButtonInfraClick(object sender, RoutedEventArgs e)
        {
            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.ConnectDeviceFirst;
                return;
            }
            if (this.mStream != null)
            {
                this.mStream.Close();
            }

            this.mStream = new InfraStream();


            this.mStream.Init(this.sensor, this.Image);

            this.statusBarText.Text = "Vue infrarouge";

        }

        private void ButtonSkeletonClick(object sender, RoutedEventArgs e)
        {

        }

        private void NearModeCheckBoxChange(object sender, RoutedEventArgs e)
        {
            if (this.CheckBoxNearMode.IsChecked.GetValueOrDefault())
            {
                if (sensor != null)
                {
                    sensor.DepthStream.Range = DepthRange.Near;
                }
            }
            else
            {
                if (sensor != null)
                {
                    sensor.DepthStream.Range = DepthRange.Default;
                }
            }
        }
    }
}