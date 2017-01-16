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
        }

        public ColorTexture(string path)
        {
            Texture = TextureLoader.GenTexture(path);
            valueChanged = true;
        }

        public ShaderResourceView Texture { get; set; }

        public EffectShaderResourceVariable ColorTexture_texture;

        public override void Download(RenderingContext context)
        {
            if(valueChanged && ColorTexture_texture != null && Texture != null)
            {
                ColorTexture_texture.SetResource(Texture);
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
        }

        protected override void OnDispose()
        {
            if (Texture != null) Texture.Dispose();
        }
    }
}
