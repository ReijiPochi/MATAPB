using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Matrix = System.Numerics.Matrix4x4;

using MATAPB.Objects;
using SharpDX;
using SharpDX.Direct3D11;

namespace MATAPB
{
    public class Camera
    {
        public Vector3 Eye { get; set; }
        public Vector3 Target { get; set; }
        public Vector3 Up { get; set; }
        public int ViewPortWidth { get; protected set; }
        public int ViewPortHeight { get; protected set; }

        public Matrix CameraMatrix { get; protected set; }

        public virtual void Take(List<RenderableObject> objects, RenderingContext context)
        {
            if (objects == null || context == null) return;

            ViewPortWidth = (int)(context.viewArea.X * PresentationArea.ScreenZoom);
            ViewPortHeight = (int)(context.viewArea.Y * PresentationArea.ScreenZoom);

            context.cam = this;

            foreach (RenderableObject o in objects)
            {
                o.Draw(context);
            }
        }

        public virtual void CameraUpdate()
        {
            if (PresentationArea.GraphicsDevice == null) return;

            PresentationArea.GraphicsDevice.ImmediateContext.Rasterizer.SetViewport(0, 0, ViewPortWidth, ViewPortHeight);
        }
    }
}
