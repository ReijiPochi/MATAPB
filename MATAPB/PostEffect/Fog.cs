using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects;
using SharpDX.Direct3D11;

namespace MATAPB.PostEffect
{
    public class Fog : PostEffect
    {
        public Fog()
        {
            CurrentEffect = TagCollection.Compile(LoadShaderText("Fog.fx"));
            InitInputLayout();

            renderTarget = CurrentEffect.GetVariableByName("renderTarget").AsShaderResource();
            zBuffer = CurrentEffect.GetVariableByName("zBuffer").AsShaderResource();
        }

        EffectShaderResourceVariable renderTarget;
        EffectShaderResourceVariable zBuffer;

        public override void Apply(RenderingCanvas target)
        {
            renderTarget.SetResource(target.renderView);
            zBuffer.SetResource(target.zView);

            base.Apply(target);
        }
    }
}
