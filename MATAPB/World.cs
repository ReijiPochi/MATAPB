using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector4 = System.Numerics.Vector4;
using Matrix = System.Numerics.Matrix4x4;

using MATAPB.Objects;
using SharpDX;
using SharpDX.Direct3D11;

namespace MATAPB
{
    public abstract class World : AutoDisposeObject
    {
        public World()
        {
            Objects = new List<RenderableObject>();
            OverlayObjects = new List<RenderableObject>();
            InitConstantBuffer();
        }

        public List<RenderableObject> Objects { get; set; }

        public List<RenderableObject> OverlayObjects { get; set; }

        public Camera ActiveCamera { get; set; }

        private ConstantBuffer cbuffer;

        public SharpDX.Direct3D11.Buffer WorldConstantBuffer { get; protected set; }


        public virtual void Render(RenderingContext context)
        {
            if(ActiveCamera is Camera3D)
            {
                Camera3D cam3d = ActiveCamera as Camera3D;

                cam3d.SetSide1();
                UpdateConstantBuffer();
                context.cbuffer = WorldConstantBuffer;
                ActiveCamera.Take(Objects, context);
                context.canvas.ClearDepthStencil();
                ActiveCamera.Take(OverlayObjects, context);

                cam3d.SetSide2();
                UpdateConstantBuffer();
                context.cbuffer = WorldConstantBuffer;
                ActiveCamera.Take(Objects, context);
                context.canvas.ClearDepthStencil();
                ActiveCamera.Take(OverlayObjects, context);
            }
            else
            {
                if (ActiveCamera == null) return;

                ActiveCamera.CameraUpdate(context);
                UpdateConstantBuffer();
                context.cbuffer = WorldConstantBuffer;
                if (ActiveCamera != null)
                {
                    ActiveCamera.Take(Objects, context);
                    context.canvas.ClearDepthStencil();
                    ActiveCamera.Take(OverlayObjects, context);
                }
            }
        }

        protected void InitConstantBuffer()
        {
            cbuffer = new ConstantBuffer()
            {
                light1_color = new Vector4(1.1f, 1.1f, 1.1f, 0.0f),
                light1_direction = Vector4.Normalize(new Vector4(2.0f, -2.0f, -10.0f, 0)),
                light1_lambertConstant = new Vector4(0.3f, 0.7f, 0, 0),
                light1_ambient = new Vector4(0.0f, 0.03f, 0.1f, 0)
            };

            WorldConstantBuffer = new SharpDX.Direct3D11.Buffer(
                PresentationArea.GraphicsDevice,
                new BufferDescription
                {
                    SizeInBytes = ConstantBuffer.SizeInBytes,
                    BindFlags = BindFlags.ConstantBuffer
                }
                );
        }

        protected void UpdateConstantBuffer()
        {
            if (WorldConstantBuffer == null) return;

            PresentationArea.GraphicsDevice.ImmediateContext.UpdateSubresource(ref cbuffer, WorldConstantBuffer);
        }

        /// <summary>
        /// バイト数は16の倍数である必要あり？各要素は16バイトの必要あり
        /// </summary>
        public struct ConstantBuffer
        {
            public Vector4 light1_color;
            public Vector4 light1_direction;
            public Vector4 light1_lambertConstant;
            public Vector4 light1_ambient;

            public static int SizeInBytes
            {
                get { return System.Runtime.InteropServices.Marshal.SizeOf(typeof(ConstantBuffer)); }
            }
        }

        protected override void OnDispose()
        {
            if (WorldConstantBuffer != null) WorldConstantBuffer.Dispose();
        }
    }
}
