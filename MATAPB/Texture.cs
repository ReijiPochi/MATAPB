using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D11;
using SharpDX.Direct3D;
using SharpDX.DXGI;

namespace MATAPB
{
    public class Texture : AutoDisposeObject
    {
        public Texture(int w, int h, int sample = 1)
        {
            Description = new Texture2DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Height = h,
                Width = w,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(sample, 0),
                Usage = ResourceUsage.Default
            };

            Tex = new Texture2D(PresentationBase.GraphicsDevice, Description);

            ShaderResourceViewDescription srvDesc = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture2DMultisampled
            };

            if (sample > 1)
                ShaderResource = new ShaderResourceView(PresentationBase.GraphicsDevice, Tex, srvDesc);
            else
                ShaderResource = new ShaderResourceView(PresentationBase.GraphicsDevice, Tex);
        }

        public ShaderResourceView ShaderResource { get; set; }
        public Texture2D Tex { get; set; }
        public Texture2DDescription Description { get; set; }

        protected override void OnDispose()
        {
            if (ShaderResource != null) ShaderResource.Dispose();
            if (Tex != null) Tex.Dispose();
        }
    }
}
