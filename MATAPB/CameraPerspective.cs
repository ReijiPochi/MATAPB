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
    public class CameraPerspective : Camera
    {
        public double FieldOfView { get; set; }

        public Matrix ViewMatrix { get; protected set; }
        public Matrix ProjectionMatrix { get; protected set; }
        

        public override void CameraUpdate()
        {
            base.CameraUpdate();

            ViewMatrix = Matrix.LookAtRH(MatVector3.ToSlimDXVector3(Eye), MatVector3.ToSlimDXVector3(Target), MatVector3.ToSlimDXVector3(Up));
            ProjectionMatrix = Matrix.PerspectiveFovRH((float)(Math.PI * (FieldOfView / 180.0)), (float)ViewPortWidth / ViewPortHeight, 0.1f, 1000.0f);
            CameraMatrix = ViewMatrix * ProjectionMatrix;
        }
    }
}
