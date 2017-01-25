﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D11;
using SharpDX.D3DCompiler;
using MATAPB.Objects;
using SharpDX.DXGI;
using SharpDX.Direct3D;

namespace MATAPB.PostEffect
{
    public class SSAO : PostEffect
    {
        public SSAO()
        {
            CurrentEffect = TagCollection.Compile(LoadShaderText("SSAO.fx"));
            InitInputLayout();

            renderTarget = CurrentEffect.GetVariableByName("renderTarget").AsShaderResource();
            gBuffer = CurrentEffect.GetVariableByName("gBuffer").AsShaderResource();
            zBuffer = CurrentEffect.GetVariableByName("zBuffer").AsShaderResource();
        }

        EffectShaderResourceVariable renderTarget;
        EffectShaderResourceVariable gBuffer;
        EffectShaderResourceVariable zBuffer;

        public override void Apply(RenderingCanvas target)
        {
            renderTarget.SetResource(target.renderView);
            gBuffer.SetResource(target.geometryView);
            zBuffer.SetResource(target.zView);

            base.Apply(target);
        }
    }
}
