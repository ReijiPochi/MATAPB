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
    public enum RenderingCanvasMode
    {
        Color = 0x0001,
        Depth = 0x0010,
        Zbuffer = 0x0100,
        Gbuffer = 0x1000,
        Full = 0x1111
    }

    public class RenderingCanvas : AutoDisposeObject
    {
        public RenderingCanvas()
        {

        }

        public RenderingCanvas(int w, int h, int sample, RenderingCanvasMode mode = RenderingCanvasMode.Full)
        {
            if((mode & RenderingCanvasMode.Color) != 0)
            {
                Color = new Texture(w, h, sample);

                colorTexture = Color.Tex;
                colorTarget = new RenderTargetView(PresentationBase.GraphicsDevice, Color.Tex);
                colorResource = new ShaderResourceView(PresentationBase.GraphicsDevice, Color.Tex);
            }

            if((mode & RenderingCanvasMode.Depth) != 0)
            {
                Texture2DDescription depthBufferDesc = new Texture2DDescription
                {
                    ArraySize = 1,
                    BindFlags = BindFlags.DepthStencil,
                    Format = Format.D32_Float,
                    Width = w,
                    Height = h,
                    MipLevels = 1,
                    SampleDescription = colorTexture.Description.SampleDescription
                };

                depthTexture = new Texture2D(PresentationBase.GraphicsDevice, depthBufferDesc);
                depthStencil = new DepthStencilView(PresentationBase.GraphicsDevice, depthTexture);
            }

            if((mode & RenderingCanvasMode.Zbuffer) != 0)
            {
                Texture2DDescription zBufferDesc = new Texture2DDescription
                {
                    ArraySize = 1,
                    BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                    Format = Format.R32_Float,
                    Width = w,
                    Height = h,
                    MipLevels = 1,
                    SampleDescription = colorTexture.Description.SampleDescription
                };

                zTexture = new Texture2D(PresentationBase.GraphicsDevice, zBufferDesc);
                zTarget = new RenderTargetView(PresentationBase.GraphicsDevice, zTexture);
                zResource = new ShaderResourceView(PresentationBase.GraphicsDevice, zTexture);
            }

            if((mode & RenderingCanvasMode.Gbuffer) != 0)
            {
                Texture2DDescription gBufferDesc = new Texture2DDescription
                {
                    ArraySize = 1,
                    BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                    Format = Format.R8G8B8A8_UNorm,
                    Width = w,
                    Height = h,
                    MipLevels = 1,
                    SampleDescription = colorTexture.Description.SampleDescription
                };

                geometryTexture = new Texture2D(PresentationBase.GraphicsDevice, gBufferDesc);
                geometryTarget = new RenderTargetView(PresentationBase.GraphicsDevice, geometryTexture);
                geometryView = new ShaderResourceView(PresentationBase.GraphicsDevice, geometryTexture);
            }

            Width = w;
            Height = h;
        }

        public RenderingCanvas(Texture tex)
        {
            //RenderTargetViewDescription rtvDesc = new RenderTargetViewDescription()
            //{
            //    Format = Format.R8G8B8A8_UNorm,
            //    Dimension = RenderTargetViewDimension.Texture2D
            //};

            colorTexture = tex.Tex;
            colorTarget = new RenderTargetView(PresentationBase.GraphicsDevice, tex.Tex);
            colorResource = new ShaderResourceView(PresentationBase.GraphicsDevice, tex.Tex);

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
            zResource = new ShaderResourceView(PresentationBase.GraphicsDevice, zTexture);

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

            Width = tex.Description.Width;
            Height = tex.Description.Height;
        }

        public static RenderingCanvas FromTexture2D(Texture2D tex2D)
        {
            RenderingCanvas result = new RenderingCanvas();

            result.colorTexture = tex2D;
            result.colorTarget = new RenderTargetView(PresentationBase.GraphicsDevice, tex2D);
            result.colorResource = new ShaderResourceView(PresentationBase.GraphicsDevice, tex2D);

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
            result.zResource = new ShaderResourceView(PresentationBase.GraphicsDevice, result.zTexture);

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

            result.Width = tex2D.Description.Width;
            result.Height = tex2D.Description.Height;

            return result;
        }

        public Color4 color = new Color4(0.0f, 0.0f, 0.0f, 0.0f);

        public Texture Color { get; }
        public Texture2D colorTexture;
        public RenderTargetView colorTarget;
        public ShaderResourceView colorResource;

        public Texture2D depthTexture;
        public DepthStencilView depthStencil;

        public Texture2D zTexture;
        public RenderTargetView zTarget;
        public ShaderResourceView zResource;

        public Texture2D geometryTexture;
        public RenderTargetView geometryTarget;
        public ShaderResourceView geometryView;

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public void SetCanvas()
        {
            PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.ResetTargets();

            if (geometryTarget != null && zTarget != null)
            {
                PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.SetTargets(depthStencil, colorTarget, geometryTarget, zTarget);
            }
            else
            {
                PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.SetTargets(depthStencil, colorTarget);
            }
        }

        public void ClearCanvas()
        {
            PresentationBase.GraphicsDevice.ImmediateContext.ClearRenderTargetView(colorTarget, color);
            PresentationBase.GraphicsDevice.ImmediateContext.ClearRenderTargetView(geometryTarget, SharpDX.Color.Black);
            PresentationBase.GraphicsDevice.ImmediateContext.ClearRenderTargetView(zTarget, SharpDX.Color.Transparent);
            PresentationBase.GraphicsDevice.ImmediateContext.ClearDepthStencilView(depthStencil, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        public void ClearDepthStencil()
        {
            PresentationBase.GraphicsDevice.ImmediateContext.ClearDepthStencilView(depthStencil, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        public void Resolve(Texture target)
        {
            PresentationBase.GraphicsDevice.ImmediateContext.ResolveSubresource(colorTexture, 0, target.Tex, 0, Format.B8G8R8A8_UNorm);
        }

        protected override void OnDispose()
        {
            if (colorTarget != null) colorTarget.Dispose();
            if (depthStencil != null) depthStencil.Dispose();
            if (geometryTarget != null) geometryTarget.Dispose();
            if (zTarget != null) zTarget.Dispose();

            if (colorTexture != null) colorTexture.Dispose();
            if (depthTexture != null) depthTexture.Dispose();
            if (geometryTexture != null) geometryTexture.Dispose();
            if (zTexture != null) zTexture.Dispose();

            if (colorResource != null) colorResource.Dispose();
            if (geometryView != null) geometryView.Dispose();
            if (zResource != null) zResource.Dispose();
        }
    }
}
