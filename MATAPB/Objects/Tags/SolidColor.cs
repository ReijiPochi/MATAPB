using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;

namespace MATAPB.Objects.Tags
{
    public enum SolidColorOverwriteMode
    {
        Color,
        ColorAndAlpha
    }

    public class SolidColor : Tag
    {
        public SolidColor(SolidColorOverwriteMode mode, MatColor color)
        {
            Mode = mode;
            Color = color;
        }

        public SolidColorOverwriteMode Mode { get; set; }

        public MatColor Color { get; set; }

        private EffectVectorVariable SolidColor_color;

        public override void Download(RenderingContext context)
        {
            if (SolidColor_color != null)
            {
                SolidColor_color.Set(new float[] { (float)Color.R, (float)Color.G, (float)Color.B, (float)Color.A });
            }
        }

        public override string GetShaderText()
        {
            Dictionary<string, string> data = Disassemble(LoadShaderText("SolidColor.fx"));

            switch (Mode)
            {
                case SolidColorOverwriteMode.Color:
                    return data["COLOR_OVERWRITE"];

                case SolidColorOverwriteMode.ColorAndAlpha:
                    return data["COLOR_ALPHA_OVERWRITE"];

                default:
                    return null;
            }
        }

        public override void SetVariables(Effect effect)
        {
            SolidColor_color = effect.GetVariableByName("SolidColor_color").AsVector();
        }
    }
}
