using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix = System.Numerics.Matrix4x4;

using MATAPB.Objects;
using SharpDX.Direct3D11;
using SharpDX;

namespace MATAPB
{
    public class CameraPerspective : Camera
    {
        public double FieldOfView { get; set; }

        public Matrix ViewMatrix { get; protected set; }
        public Matrix ProjectionMatrix { get; protected set; }
        

        public override void CameraUpdate()
        {
            base.CameraUpdate();

            ViewMatrix = Matrix.CreateLookAt(Eye, Target, Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView((float)(Math.PI * (FieldOfView / 180.0)), (float)ViewPortWidth / ViewPortHeight, 0.1f, 1000.0f);
            CameraMatrix = ViewMatrix * ProjectionMatrix;
        }
    }
}
