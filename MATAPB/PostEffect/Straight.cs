using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects;
using SharpDX.Direct3D11;

namespace MATAPB.PostEffect
{
    public class Straight : PostEffect
    {
        public Straight()
        {
            CurrentEffect = TagCollection.Compile(LoadShaderText("Straight.fx"));
            InitInputLayout();

            renderTarget = CurrentEffect.GetVariableByName("renderTarget").AsShaderResource();
        }

        EffectShaderResourceVariable renderTarget;

        public override void Apply(RenderingCanvas target)
        {
            renderTarget.SetResource(target.renderView);

            base.Apply(target);
        }
    }
}
