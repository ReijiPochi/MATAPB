using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;

namespace MATAPB.Objects.Tags
{
    public class LineThicknessGS : Tag
    {
        public double Thickness { get; set; } = 0.1;

        private EffectScalarVariable LineThicknessGS_halfThickness;

        public override void Download(RenderingContext context)
        {
            if (LineThicknessGS_halfThickness != null)
                LineThicknessGS_halfThickness.Set((float)(Thickness / 2));
        }

        public override string GetShaderText()
        {
            return LoadShaderText("LineThicknessGS.fx");
        }

        public override void SetVariables(Effect effect)
        {
            LineThicknessGS_halfThickness = effect.GetVariableByName("LineThicknessGS_halfThickness").AsScalar();
        }
    }
}
