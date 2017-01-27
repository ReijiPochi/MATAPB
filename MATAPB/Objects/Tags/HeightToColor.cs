using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;

namespace MATAPB.Objects.Tags
{
    public class HeightToColor : Tag
    {
        public override void Download(RenderingContext context)
        {
            
        }

        public override string GetShaderText()
        {
            return LoadShaderText("HeightToColor.fx");
        }

        public override void SetVariables(Effect effect)
        {
            
        }
    }
}
