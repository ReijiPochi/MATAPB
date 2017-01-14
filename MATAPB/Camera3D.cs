using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects;
using SlimDX.Direct3D11;
using SlimDX;

namespace MATAPB
{
    public class Camera3D : CameraPerspective
    {
        public MatVector3 ActualEye { get; set; }
        public MatVector3 ActualTarget { get; set; }
        public MatVector3 ActualUp { get; set; }
        private int viewPortX = 0, viewPortY = 0;

        public void SetSide1()
        {
            MatVector3 eyeVector = MatVector3.Normalize(MatVector3.Cross(Up, MatVector3.Direction(Eye, Target)));
            eyeVector = MatVector3.Multiply(eyeVector, 0.03);

            ActualEye = MatVector3.Add(Eye, eyeVector);
            ActualTarget = MatVector3.Add(Target, eyeVector);

            viewPortX = 0;

            CameraUpdate();
        }

        public void SetSide2()
        {
            MatVector3 eyeVector = MatVector3.Normalize(MatVector3.Cross(MatVector3.Direction(Eye, Target), Up));
            eyeVector = MatVector3.Multiply(eyeVector, 0.03);

            ActualEye = MatVector3.Add(Eye, eyeVector);
            ActualTarget = MatVector3.Add(Target, eyeVector);

            viewPortX = (int)(ViewPortWidth * 0.5);

            CameraUpdate();
        }

        public override void CameraUpdate()
        {
            //base.CameraUpdate();

            if (PresentationArea.GraphicsDevice == null) return;

            PresentationArea.GraphicsDevice.ImmediateContext.Rasterizer.SetViewports(new Viewport
            {
                X = viewPortX,
                Y = viewPortY,
                Width = (int)(ViewPortWidth * 0.5),
                Height = ViewPortHeight,
                MaxZ = 1.0f
            });

            Matrix view = Matrix.LookAtRH(MatVector3.ToSlimDXVector3(ActualEye), MatVector3.ToSlimDXVector3(ActualTarget), MatVector3.ToSlimDXVector3(Up));
            Matrix projection = Matrix.PerspectiveFovRH((float)(Math.PI * (FieldOfView / 180.0)), (float)ViewPortWidth / ViewPortHeight, 0.1f, 1000.0f);
            CameraMatrix = view * projection;
        }
    }
}
