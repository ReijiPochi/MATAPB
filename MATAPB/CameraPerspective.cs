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
    public enum CameraPerspectiveMode
    {
        UseFOV,
        UseWidthHeight
    }

    public class CameraPerspective : Camera
    {
        public CameraPerspectiveMode Mode { get; set; } = CameraPerspectiveMode.UseFOV;

        public double FieldOfView { get; set; }

        public double CameraWidth { get; set; }
        public double CameraHeight { get; set; }

        public Matrix ViewMatrix { get; protected set; }
        public Matrix ProjectionMatrix { get; protected set; }
        

        public override void CameraUpdate(RenderingContext context)
        {
            base.CameraUpdate(context);

            ViewMatrix = Matrix.CreateLookAt(Eye, Target, Up);

            switch (Mode)
            {
                case CameraPerspectiveMode.UseFOV:
                    ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView((float)(Math.PI * (FieldOfView / 180.0)), (float)ViewPortWidth / ViewPortHeight, 0.1f, 1000.0f);
                    break;

                case CameraPerspectiveMode.UseWidthHeight:
                    ProjectionMatrix = Matrix.CreatePerspective((float)CameraWidth, (float)CameraHeight, 1.0f, 1000.0f);
                    break;

                default:
                    break;
            }
            
            CameraMatrix = ViewMatrix * ProjectionMatrix;
        }
    }
}
