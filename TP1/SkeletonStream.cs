using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.ColorBasics
{


    class SkeletonStream : KinectStream
    {
        public override void Init(Microsoft.Kinect.KinectSensor sensor, System.Windows.Controls.Canvas canvas)
        {
            base.Init(sensor, canvas);
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }
    }
}
