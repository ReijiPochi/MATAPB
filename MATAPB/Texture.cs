﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D11;
using SharpDX;
using SharpDX.DXGI;

namespace MATAPB
{
    public class Texture : AutoDisposeObject
    {
        public Texture(int w, int h)
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
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };

            Tex = new Texture2D(PresentationArea.GraphicsDevice, Description);

            //ShaderResourceViewDescription srvDesc = new ShaderResourceViewDescription()
            //{
            //    Format = Format.R8G8B8A8_UNorm,
            //    Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2D,
            //};

            ShaderResource = new ShaderResourceView(PresentationArea.GraphicsDevice, Tex);
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