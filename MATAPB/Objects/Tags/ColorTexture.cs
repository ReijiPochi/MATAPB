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

        public AnimationDouble Opacity { get; set; } = new AnimationDouble();

        private EffectShaderResourceVariable ColorTexture_texture;
        private EffectScalarVariable ColorTexture_opacity;

        public override void Download(RenderingContext context)
        {
            if (ColorTexture_texture != null && Texture != null)
            {
                ColorTexture_texture.SetResource(Texture);
            }

            ColorTexture_opacity.Set((float)Opacity.ActualValue);
        }

        public override string GetShaderText()
        {
            return LoadShaderText("ColorTexture.fx");
        }

        public override void SetVariables(Effect effect)
        {
            ColorTexture_texture = effect.GetVariableByName("ColorTexture_texture").AsShaderResource();
            ColorTexture_opacity = effect.GetVariableByName("ColorTexture_opacity").AsScalar();
        }

        protected override void OnDispose()
        {
            if (Texture != null) Texture.Dispose();
        }
    }
}
