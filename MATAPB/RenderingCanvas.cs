using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace MATAPB
{
    public class RenderingCanvas : AutoDisposeObject
    {
        public RenderingCanvas()
        {

        }

        public RenderingCanvas(Texture tex)
        {
            //RenderTargetViewDescription rtvDesc = new RenderTargetViewDescription()
            //{
            //    Format = Format.R8G8B8A8_UNorm,
            //    Dimension = RenderTargetViewDimension.Texture2D
            //};

            renderTarget = new RenderTargetView(PresentationBase.GraphicsDevice, tex.Tex);

            Texture2DDescription depthBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                Format = Format.D32_Float,
                Width = tex.Description.Width,
                Height = tex.Description.Height,
                MipLevels = 1,
                SampleDescription = tex.Description.SampleDescription
            };

            using (Texture2D depthBuffer = new Texture2D(PresentationBase.GraphicsDevice, depthBufferDesc))
            {
                depthStencil = new DepthStencilView(PresentationBase.GraphicsDevice, depthBuffer);
            }

            width = tex.Description.Width;
            height = tex.Description.Height;
        }

        public Color4 color = new Color4(0.0f, 0.0f, 0.0f, 0.0f);
        public RenderTargetView renderTarget;
        public DepthStencilView depthStencil;
        public double width, height;

        public void SetCanvas()
        {
            PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.SetTargets(depthStencil, renderTarget);
        }

        public void ClearCanvas()
        {
            PresentationBase.GraphicsDevice.ImmediateContext.ClearRenderTargetView(renderTarget, color);
            PresentationBase.GraphicsDevice.ImmediateContext.ClearDepthStencilView(depthStencil, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        public void ClearDepthStencil()
        {
            PresentationBase.GraphicsDevice.ImmediateContext.ClearDepthStencilView(depthStencil, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        protected override void OnDispose()
        {
            if (renderTarget != null) renderTarget.Dispose();
            if (depthStencil != null) depthStencil.Dispose();
        }
    }
}
