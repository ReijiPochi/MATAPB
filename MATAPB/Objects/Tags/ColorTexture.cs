using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using MATAPB.Input;

namespace MATAPB.Objects.Tags
{
    public class ColorTexture : Tag
    {
        public ColorTexture()
        {
            if (Sampler == null) FilterType = _FilterType;
            Opacity.Value = 1.0;
            Opacity.ActualValue = 1.0;
        }

        public ColorTexture(string path) : this()
        {
            Texture = TextureLoader.GenTexture(path);
            valueChanged = true;
        }

        public ColorTexture(Texture tex) : this()
        {
            Texture = tex.ShaderResource;
        }

        public ShaderResourceView Texture { get; set; }

        private Filter _FilterType = Filter.Anisotropic;
        public Filter FilterType
        {
            get { return _FilterType; }
            set
            {
                SamplerStateDescription desc = new SamplerStateDescription()
                {
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap,
                    MaximumAnisotropy = 16,
                    Filter = value
                };
                Sampler = new SamplerState(PresentationBase.GraphicsDevice, desc);
                _FilterType = value;
            }
        }

        public AnimationDouble Opacity { get; set; } = new AnimationDouble();

        private SamplerState _Sampler;
        public SamplerState Sampler
        {
            get { return _Sampler; }
            set
            {
                valueChanged = true;
                _Sampler = value;
            }
        }

        private EffectShaderResourceVariable ColorTexture_texture;
        private EffectScalarVariable ColorTexture_opacity;
        private EffectSamplerVariable ColorTexture_sampler;
        private bool valueChanged = true;

        public override void Download(RenderingContext context)
        {
            if (ColorTexture_texture != null && Texture != null)
            {
                ColorTexture_texture.SetResource(Texture);
            }

            ColorTexture_opacity.Set((float)Opacity.ActualValue);

            if(valueChanged)
            {
                ColorTexture_sampler.SetSampler(0, _Sampler);
                valueChanged = false;
            }
        }

        public override string GetShaderText()
        {
            return LoadShaderText("ColorTexture.fx");
        }

        public override void SetVariables(Effect effect)
        {
            ColorTexture_texture = effect.GetVariableByName("ColorTexture_texture").AsShaderResource();
            ColorTexture_opacity = effect.GetVariableByName("ColorTexture_opacity").AsScalar();
            ColorTexture_sampler = effect.GetVariableByName("ColorTexture_sampler").AsSampler();
        }

        protected override void OnDispose()
        {
            if (Texture != null) Texture.Dispose();
        }
    }
}
