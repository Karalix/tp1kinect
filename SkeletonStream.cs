using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Kinect;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Samples.Kinect.ColorBasics
{


    class SkeletonStream : KinectStream
    {
        public override void Init(Microsoft.Kinect.KinectSensor sensor, Canvas canvas)
        {
            base.Init(sensor, canvas);

            sensor.SkeletonStream.Enable();

            sensor.SkeletonFrameReady += onSkeletonFrameReady;
        }

        public override void Close()
        {
            sensor.SkeletonFrameReady -= onSkeletonFrameReady;
            sensor.SkeletonStream.Disable();

        }

        protected void onSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    Skeleton[] squelettes = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(squelettes);

                    if (canvas.Children.Count > 2)
                        canvas.Children.RemoveRange(1, canvas.Children.Count - 1);

                    foreach (Skeleton squelette in squelettes.Where(skel => skel.TrackingState == SkeletonTrackingState.Tracked))
                    {
                        drawBone(squelette, canvas, JointType.Head, JointType.ShoulderCenter, Brushes.AliceBlue);

                        drawBone(squelette, canvas, JointType.ShoulderCenter, JointType.ShoulderLeft, Brushes.AliceBlue);
                        drawBone(squelette, canvas, JointType.ShoulderCenter, JointType.ShoulderRight, Brushes.AliceBlue);

                        drawBone(squelette, canvas, JointType.ShoulderCenter, JointType.Spine, Brushes.AliceBlue);

                        drawBone(squelette, canvas, JointType.Spine, JointType.HipCenter, Brushes.AliceBlue);

                        drawBone(squelette, canvas, JointType.HipCenter, JointType.HipLeft, Brushes.AliceBlue);
                        drawBone(squelette, canvas, JointType.HipCenter, JointType.HipRight, Brushes.AliceBlue);

                        drawBone(squelette, canvas, JointType.ShoulderLeft, JointType.ElbowLeft, Brushes.AliceBlue);
                        drawBone(squelette, canvas, JointType.ElbowLeft, JointType.WristLeft, Brushes.AliceBlue);
                        drawBone(squelette, canvas, JointType.WristLeft, JointType.HandLeft, Brushes.AliceBlue);

                        drawBone(squelette, canvas, JointType.ShoulderRight, JointType.ElbowRight, Brushes.AliceBlue);
                        drawBone(squelette, canvas, JointType.ElbowRight, JointType.WristRight, Brushes.AliceBlue);
                        drawBone(squelette, canvas, JointType.WristRight, JointType.HandRight, Brushes.AliceBlue);

                        // Left Leg
                        this.drawBone(squelette, canvas, JointType.HipLeft, JointType.KneeLeft, Brushes.AliceBlue);
                        this.drawBone(squelette, canvas, JointType.KneeLeft, JointType.AnkleLeft, Brushes.AliceBlue);
                        this.drawBone(squelette, canvas, JointType.AnkleLeft, JointType.FootLeft, Brushes.AliceBlue);

                        // Right Leg
                        this.drawBone(squelette, canvas, JointType.HipRight, JointType.KneeRight, Brushes.AliceBlue);
                        this.drawBone(squelette, canvas, JointType.KneeRight, JointType.AnkleRight, Brushes.AliceBlue);
                        this.drawBone(squelette, canvas, JointType.AnkleRight, JointType.FootRight, Brushes.AliceBlue);
                    }
                }
            }
        }

        private void drawBone(Skeleton squelette, Canvas canvas, JointType articulation1, JointType articulation2, Brush b)
        {
            Joint art1 = squelette.Joints[articulation1];
            Joint art2 = squelette.Joints[articulation2];

            if (art1.TrackingState == JointTrackingState.NotTracked || art2.TrackingState == JointTrackingState.NotTracked)
                return;

            Point p1 = SkeletonPointToScreen(art1.Position);
            Point p2 = SkeletonPointToScreen(art2.Position);

            Line bone = new Line();

            bone.Stroke = b;
            bone.StrokeThickness = 2;

            bone.X1 = p1.X;
            bone.Y1 = p1.Y;

            bone.X2 = p2.X;
            bone.Y2 = p2.Y;

            canvas.Children.Add(bone);
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }
    }
}
