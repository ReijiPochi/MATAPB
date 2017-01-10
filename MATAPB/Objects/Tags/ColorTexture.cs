using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D11;

namespace MATAPB.Objects.Tags
{
    public class ColorTexture : Tag
    {
        public ColorTexture()
        {
        }

        public ColorTexture(string path)
        {
            Texture = ShaderResourceView.FromFile(PresentationArea.GraphicsDevice, path);
            valueChanged = true;
        }

        public ShaderResourceView Texture { get; set; }

        public EffectResourceVariable ColorTexture_texture;

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
            ColorTexture_texture = effect.GetVariableByName("ColorTexture_texture").AsResource();
        }

        protected override void OnDispose()
        {
            if (Texture != null) Texture.Dispose();
        }
    }
}
