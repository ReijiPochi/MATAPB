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

            renderTexture = tex.Tex;
            renderTarget = new RenderTargetView(PresentationBase.GraphicsDevice, tex.Tex);
            renderView = new ShaderResourceView(PresentationBase.GraphicsDevice, tex.Tex);

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

            depthTexture = new Texture2D(PresentationBase.GraphicsDevice, depthBufferDesc);
            depthStencil = new DepthStencilView(PresentationBase.GraphicsDevice, depthTexture);

            Texture2DDescription zBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.R32_Float,
                Width = tex.Description.Width,
                Height = tex.Description.Height,
                MipLevels = 1,
                SampleDescription = tex.Description.SampleDescription
            };

            zTexture = new Texture2D(PresentationBase.GraphicsDevice, zBufferDesc);
            zTarget = new RenderTargetView(PresentationBase.GraphicsDevice, zTexture);
            zView = new ShaderResourceView(PresentationBase.GraphicsDevice, zTexture);

            Texture2DDescription gBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.R8G8B8A8_UNorm,
                Width = tex.Description.Width,
                Height = tex.Description.Height,
                MipLevels = 1,
                SampleDescription = tex.Description.SampleDescription
            };

            geometryTexture = new Texture2D(PresentationBase.GraphicsDevice, gBufferDesc);
            geometryTarget = new RenderTargetView(PresentationBase.GraphicsDevice, geometryTexture);
            geometryView = new ShaderResourceView(PresentationBase.GraphicsDevice, geometryTexture);

            width = tex.Description.Width;
            height = tex.Description.Height;
        }

        public static RenderingCanvas FromTexture2D(Texture2D tex2D)
        {
            RenderingCanvas result = new RenderingCanvas();

            result.renderTexture = tex2D;
            result.renderTarget = new RenderTargetView(PresentationBase.GraphicsDevice, tex2D);
            result.renderView = new ShaderResourceView(PresentationBase.GraphicsDevice, tex2D);

            Texture2DDescription depthBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                Format = Format.D32_Float,
                Width = tex2D.Description.Width,
                Height = tex2D.Description.Height,
                MipLevels = 1,
                SampleDescription = tex2D.Description.SampleDescription
            };

            result.depthTexture = new Texture2D(PresentationBase.GraphicsDevice, depthBufferDesc);
            result.depthStencil = new DepthStencilView(PresentationBase.GraphicsDevice, result.depthTexture);
            result.depthView = new ShaderResourceView(PresentationBase.GraphicsDevice, result.depthTexture);

            Texture2DDescription zBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.R32_Float,
                Width = tex2D.Description.Width,
                Height = tex2D.Description.Height,
                MipLevels = 1,
                SampleDescription = tex2D.Description.SampleDescription
            };

            result.zTexture = new Texture2D(PresentationBase.GraphicsDevice, zBufferDesc);
            result.zTarget = new RenderTargetView(PresentationBase.GraphicsDevice, result.zTexture);
            result.zView = new ShaderResourceView(PresentationBase.GraphicsDevice, result.zTexture);

            Texture2DDescription gBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.R8G8B8A8_UNorm,
                Width = tex2D.Description.Width,
                Height = tex2D.Description.Height,
                MipLevels = 1,
                SampleDescription = tex2D.Description.SampleDescription
            };

            result.geometryTexture = new Texture2D(PresentationBase.GraphicsDevice, gBufferDesc);
            result.geometryTarget = new RenderTargetView(PresentationBase.GraphicsDevice, result.geometryTexture);
            result.geometryView = new ShaderResourceView(PresentationBase.GraphicsDevice, result.geometryTexture);

            result.width = tex2D.Description.Width;
            result.height = tex2D.Description.Height;

            return result;
        }

        public Color4 color = new Color4(0.0f, 0.0f, 0.0f, 0.0f);

        public Texture2D renderTexture;
        public RenderTargetView renderTarget;
        public ShaderResourceView renderView;

        public Texture2D depthTexture;
        public DepthStencilView depthStencil;
        public ShaderResourceView depthView;

        public Texture2D zTexture;
        public RenderTargetView zTarget;
        public ShaderResourceView zView;

        public Texture2D geometryTexture;
        public RenderTargetView geometryTarget;
        public ShaderResourceView geometryView;

        public int width, height;

        public void SetCanvas()
        {
            if (geometryTarget != null && zTarget != null)
            {
                PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.SetTargets(depthStencil, renderTarget, geometryTarget, zTarget);
            }
            else
            {
                PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.SetTargets(depthStencil, renderTarget);
            }
        }

        public void ClearCanvas()
        {
            PresentationBase.GraphicsDevice.ImmediateContext.ClearRenderTargetView(renderTarget, color);
            PresentationBase.GraphicsDevice.ImmediateContext.ClearRenderTargetView(geometryTarget, Color.Black);
            PresentationBase.GraphicsDevice.ImmediateContext.ClearRenderTargetView(zTarget, Color.Transparent);
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
            if (geometryTarget != null) geometryTarget.Dispose();

            if (renderTexture != null) renderTexture.Dispose();
            if (depthTexture != null) depthTexture.Dispose();
            if (geometryTexture != null) geometryTexture.Dispose();

            if (renderView != null) renderView.Dispose();
            if (depthView != null) depthView.Dispose();
            if (geometryView != null) geometryView.Dispose();
        }
    }
}
