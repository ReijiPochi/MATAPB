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

        public override void Apply(RenderingCanvas source, RenderingCanvas target)
        {
            renderTarget.SetResource(source.renderView);
            zBuffer.SetResource(source.zView);

            base.Apply(source, target);
        }

        protected override void OnDispose()
        {
            if (renderTarget != null) renderTarget.Dispose();
            if (zBuffer != null) zBuffer.Dispose();
        }
    }
}
