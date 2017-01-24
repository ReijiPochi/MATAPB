using System;
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

            CurrentEffect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(PresentationBase.GraphicsDevice.ImmediateContext);
            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.InputLayout = VertexLayout;

            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(TargetPlane.VertexBuffer, VertexData.SizeInBytes, 0));
            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.SetIndexBuffer(TargetPlane.IndexBuffer, Format.R32_UInt, 0);

            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            PresentationBase.GraphicsDevice.ImmediateContext.DrawIndexed(TargetPlane.IndexCount, 0, 0);
        }
    }
}
