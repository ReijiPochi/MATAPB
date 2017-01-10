using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects;
using SlimDX;
using SlimDX.Direct3D11;

namespace MATAPB
{
    public abstract class World : AutoDisposeObject
    {
        public World()
        {
            Objects = new List<RenderableObject>();
            InitConstantBuffer();
        }

        public List<RenderableObject> Objects { get; set; }

        public Camera ActiveCamera { get; set; }

        private ConstantBuffer cbuffer;

        public SlimDX.Direct3D11.Buffer WorldConstantBuffer { get; protected set; }


        public virtual void Render(RenderingContext context)
        {
            PresentationArea.DefaultCanvas.ClearCanvas();

            if(ActiveCamera is Camera3D)
            {
                Camera3D cam3d = ActiveCamera as Camera3D;

                cam3d.SetSide1();
                UpdateConstantBuffer();
                context.cbuffer = WorldConstantBuffer;
                ActiveCamera.Take(Objects, context);

                cam3d.SetSide2();
                UpdateConstantBuffer();
                context.cbuffer = WorldConstantBuffer;
                ActiveCamera.Take(Objects, context);
            }
            else
            {
                if (ActiveCamera == null) return;

                ActiveCamera.CameraUpdate();
                UpdateConstantBuffer();
                context.cbuffer = WorldConstantBuffer;
                if (ActiveCamera != null) ActiveCamera.Take(Objects, context);
            }
        }

        protected void InitConstantBuffer()
        {
            cbuffer = new ConstantBuffer()
            {
                light1_color = new MatVector4Float(1.1, 1.1, 1.1, 0),
                light1_direction = MatVector4Float.Normalize(new MatVector4Float(2.0, -2.0, -10.0, 0)),
                light1_lambertConstant = new MatVector4Float(0.3, 0.7, 0, 0),
                light1_ambient = new MatVector4Float(0.0, 0.03, 0.1, 0)
            };

            WorldConstantBuffer = new SlimDX.Direct3D11.Buffer(
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

            PresentationArea.GraphicsDevice.ImmediateContext.UpdateSubresource(
                new DataBox(0, 0, new DataStream(new[] { cbuffer }, true, true)),
                WorldConstantBuffer,
                0
                );
        }

        /// <summary>
        /// バイト数は16の倍数である必要あり？各要素は16バイトの必要あり
        /// </summary>
        public struct ConstantBuffer
        {
            public MatVector4Float light1_color;
            public MatVector4Float light1_direction;
            public MatVector4Float light1_lambertConstant;
            public MatVector4Float light1_ambient;

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
