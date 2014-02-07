using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using System.Windows.Controls;

namespace Microsoft.Samples.Kinect.ColorBasics
{
    public abstract class KinectStream
    {
        protected KinectSensor sensor;

        protected System.Windows.Controls.Image image = new System.Windows.Controls.Image();

        protected byte[] colorPixels;

        protected Canvas canvas;

        public virtual void Init(KinectSensor sensor, System.Windows.Controls.Canvas canvas)
        {
            this.sensor = sensor;
            this.image = new System.Windows.Controls.Image();

            this.canvas = canvas;

            canvas.Children.Insert(0, image);
        }

        public abstract void Close();

    }
}
