using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Matrix = System.Numerics.Matrix4x4;
using SharpDX;

namespace MATAPB
{
    public class CameraOrthographic : Camera
    {
        public CameraOrthographic()
        {
        }

        public double CameraWidth { get; set; } = 1.0;
        public double CameraHeight { get; set; } = 1.0;

        public override void CameraUpdate(RenderingContext context)
        {
            base.CameraUpdate(context);

            Matrix ViewMatrix = Matrix.CreateLookAt(Eye, Target, Up);
            Matrix ProjectionMatrix = Matrix.CreateOrthographic(
                (float)CameraWidth,
                (float)CameraHeight,
                0.01f,
                1000.0f);
            CameraMatrix = ViewMatrix * ProjectionMatrix;
        }
    }
}
