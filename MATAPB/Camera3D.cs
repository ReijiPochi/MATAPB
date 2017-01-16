using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Matrix = System.Numerics.Matrix4x4;

using MATAPB.Objects;
using SharpDX.Direct3D11;
using SharpDX;

namespace MATAPB
{
    public class Camera3D : CameraPerspective
    {
        public Vector3 ActualEye { get; set; }
        public Vector3 ActualTarget { get; set; }
        public Vector3 ActualUp { get; set; }
        private int viewPortX = 0, viewPortY = 0;

        public void SetSide1()
        {
            Vector3 eyeVector = Vector3.Normalize(Vector3.Cross(Up, Target - Eye));
            eyeVector = eyeVector * 0.03f;

            ActualEye = Eye + eyeVector;
            ActualTarget = Target + eyeVector;

            viewPortX = 0;

            CameraUpdate();
        }

        public void SetSide2()
        {
            Vector3 eyeVector = Vector3.Normalize(Vector3.Cross(Target - Eye, Up));
            eyeVector = eyeVector * 0.03f;

            ActualEye = Eye + eyeVector;
            ActualTarget = Target + eyeVector;

            viewPortX = (int)(ViewPortWidth * 0.5);

            CameraUpdate();
        }

        public override void CameraUpdate()
        {
            //base.CameraUpdate();

            if (PresentationArea.GraphicsDevice == null) return;

            PresentationArea.GraphicsDevice.ImmediateContext.Rasterizer.SetViewport(viewPortX, viewPortY, (float)(ViewPortWidth * 0.5), ViewPortHeight);

            Matrix view = Matrix.CreateLookAt(ActualEye, ActualTarget, Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView((float)(Math.PI * (FieldOfView / 180.0)), (float)ViewPortWidth / ViewPortHeight, 0.1f, 1000.0f);
            CameraMatrix = view * projection;
        }
    }
}
