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
    public class Camera
    {
        public MatVector3 Eye { get; set; }
        public MatVector3 Target { get; set; }
        public MatVector3 Up { get; set; }
        public double FieldOfView { get; set; }
        public int ViewPortWidth { get; set; }
        public int ViewPortHeight { get; set; }

        public Matrix CameraMatrix { get; protected set; }

        public virtual void Take(List<RenderableObject> objects, RenderingContext context)
        {
            if (objects == null || context == null) return;

            ViewPortWidth = (int)context.viewArea.X;
            ViewPortHeight = (int)context.viewArea.Y;

            context.cam = this;

            foreach(RenderableObject o in objects)
            {
                o.Draw(context);
            }
        }

        public virtual void CameraUpdate()
        {
            if (PresentationArea.GraphicsDevice == null) return;

            PresentationArea.GraphicsDevice.ImmediateContext.Rasterizer.SetViewports(new Viewport
            {
                Width = ViewPortWidth,
                Height = ViewPortHeight,
                MaxZ = 1.0f
            });
            
            Matrix view = Matrix.LookAtRH(MatVector3.ToSlimDXVector3(Eye), MatVector3.ToSlimDXVector3(Target), MatVector3.ToSlimDXVector3(Up));
            Matrix projection = Matrix.PerspectiveFovRH((float)(Math.PI * (FieldOfView / 180.0)), (float)ViewPortWidth / ViewPortHeight, 0.1f, 1000.0f);
            CameraMatrix = view * projection;
        }
    }
}
