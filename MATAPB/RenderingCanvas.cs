using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;

namespace MATAPB
{
    public class RenderingCanvas : AutoDisposeObject
    {
        public RenderingCanvas()
        {

        }

        public Color4 color;
        public RenderTargetView renderTarget;
        public DepthStencilView depthStencil;

        public void SetCanvas()
        {
            PresentationArea.GraphicsDevice.ImmediateContext.OutputMerger.SetTargets(depthStencil, renderTarget);
        }

        public void ClearCanvas()
        {
            PresentationArea.GraphicsDevice.ImmediateContext.ClearRenderTargetView(renderTarget, color);
            PresentationArea.GraphicsDevice.ImmediateContext.ClearDepthStencilView(depthStencil, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        public void ClearDepthStencil()
        {
            PresentationArea.GraphicsDevice.ImmediateContext.ClearDepthStencilView(depthStencil, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        protected override void OnDispose()
        {
            if (renderTarget != null) renderTarget.Dispose();
            if (depthStencil != null) depthStencil.Dispose();
        }
    }
}
