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
using MATAPB.PostEffect;

namespace MATAPB
{
    public abstract class World : AutoDisposeObject
    {
        public World()
        {
            InitConstantBuffer();
        }

        public List<RenderableObject> Objects { get; set; } = new List<RenderableObject>();

        public List<RenderableObject> OverlayObjects { get; set; } = new List<RenderableObject>();

        public Camera ActiveCamera { get; set; }

        public Light GlobalLight1 { get; set; } = new Light();

        private ConstantBuffer cbuffer;

        public SharpDX.Direct3D11.Buffer WorldConstantBuffer { get; protected set; }

        public PostEffect.PostEffect Effect { get; set; }


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
            WorldConstantBuffer = new SharpDX.Direct3D11.Buffer(
                PresentationBase.GraphicsDevice,
                new BufferDescription
                {
                    SizeInBytes = ConstantBuffer.SizeInBytes,
                    BindFlags = BindFlags.ConstantBuffer
                }
                );
        }

        protected void SwitchToBackbuffer()
        {
            PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.SetTargets(PresentationBase.BackBuffer);
            PresentationBase.GraphicsDevice.ImmediateContext.ClearRenderTargetView(PresentationBase.BackBuffer, Color.Black);

            if (Effect != null)
                Effect.Apply(PresentationBase.DefaultCanvas);
        }

        protected void UpdateConstantBuffer()
        {
            if (WorldConstantBuffer == null) return;

            cbuffer = new ConstantBuffer()
            {
                eye = new Vector4(ActiveCamera.Eye, 0),
                light1_color = GlobalLight1.Color,
                light1_direction = Vector4.Normalize(GlobalLight1.Direction),
                light1_lambertConstant = GlobalLight1.LambertConstant,
                light1_ambient = GlobalLight1.Ambitent
            };

            PresentationBase.GraphicsDevice.ImmediateContext.UpdateSubresource(ref cbuffer, WorldConstantBuffer);
        }

        /// <summary>
        /// バイト数は16の倍数である必要あり？各要素は16バイトの必要あり
        /// </summary>
        public struct ConstantBuffer
        {
            public Vector4 eye;
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
