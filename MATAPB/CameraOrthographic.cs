using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

namespace MATAPB
{
    public class CameraOrthographic : Camera
    {
        public double CameraWidth { get; set; } = 1.0;
        public double CameraHeight { get; set; } = 1.0;

        public override void CameraUpdate()
        {
            base.CameraUpdate();

            Matrix ViewMatrix = Matrix.LookAtRH(MatVector3.ToSlimDXVector3(Eye), MatVector3.ToSlimDXVector3(Target), MatVector3.ToSlimDXVector3(Up));
            Matrix ProjectionMatrix = Matrix.OrthoRH(
                (float)CameraWidth,
                (float)CameraHeight,
                0.01f,
                1000.0f);
            CameraMatrix = ViewMatrix * ProjectionMatrix;
        }
    }
}
