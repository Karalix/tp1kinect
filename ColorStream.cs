using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace Microsoft.Samples.Kinect.ColorBasics
{
    public class ColorStream : KinectStream
    {
        private WriteableBitmap colorBitmap;

        public override void Init(KinectSensor sensor, System.Windows.Controls.Canvas canvas)
        {
            base.Init(sensor, canvas);

            // Turn on the color stream to receive color frames
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            // Allocate space to put the pixels we'll receive
            colorPixels = new byte[sensor.ColorStream.FramePixelDataLength];

            // This is the bitmap we'll display on-screen
            colorBitmap = new WriteableBitmap(sensor.ColorStream.FrameWidth, sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

            // Set the image we display to point to the bitmap where we'll put the image data
            image.Source = colorBitmap;

            // Add an event handler to be called whenever there is new color frame data
            sensor.ColorFrameReady += SensorColorFrameReady;

            this.sensor = sensor;

        }

        public override void Close()
        {
            sensor.ColorFrameReady -= SensorColorFrameReady;
        }


        /// <summary>
        /// Event handler for Kinect sensor's ColorFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(this.colorPixels);

                    // Write the pixel data into our bitmap
                    this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels,
                        this.colorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }

    }
}
