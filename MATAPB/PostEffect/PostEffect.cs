using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects.Primitive;
using MATAPB.Objects.Tags;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Reflection;
using System.IO;
using SharpDX.Direct3D;

namespace MATAPB.PostEffect
{
    public abstract class PostEffect : AutoDisposeObject
    {
        public PostEffect()
        {
            TargetPlane.Tags.ClearTag();
        }

        public Effect CurrentEffect { get; protected set; }
        public InputLayout VertexLayout { get; protected set; }
        protected Plane TargetPlane { get; } = new Plane(2, 2, Orientations.plusZ);

        public virtual void Apply(RenderingCanvas source, RenderingCanvas target = null)
        {
            if (target != null)
                DoApply(source.colorResource, target.colorTarget, target.Width, target.Height);
            else
                DoApply(source.colorResource, null, source.Width, source.Height);
        }

        public virtual void Apply(Texture source, RenderingCanvas target = null)
        {
            if (target != null)
                DoApply(source.ShaderResource, target.colorTarget, target.Width, target.Height);
            else
                DoApply(source.ShaderResource, null, source.Description.Width, source.Description.Height);
        }

        protected virtual void DoApply(ShaderResourceView source, RenderTargetView target, int w, int h)
        {
            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.InputLayout = VertexLayout;

            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(TargetPlane.VertexBuffer, VertexData.SizeInBytes, 0));
            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.SetIndexBuffer(TargetPlane.IndexBuffer, Format.R32_UInt, 0);

            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            PresentationBase.GraphicsDevice.ImmediateContext.Rasterizer.SetViewport(0, 0, w, h);

            if (target != null)
                PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.SetRenderTargets(target);

            Render(0);
        }

        protected virtual void Render(int pass)
        {
            CurrentEffect.GetTechniqueByIndex(0).GetPassByIndex(pass).Apply(PresentationBase.GraphicsDevice.ImmediateContext);
            PresentationBase.GraphicsDevice.ImmediateContext.DrawIndexed(TargetPlane.IndexCount, 0, 0);
        }

        public static string LoadShaderText(string name)
        {
            string shader;
            Assembly a = Assembly.GetExecutingAssembly();
            string[] resources = a.GetManifestResourceNames();
            using (StreamReader sr = new StreamReader(a.GetManifestResourceStream("MATAPB.PostEffect." + name)))
            {
                shader = sr.ReadToEnd();
            }

            return shader;
        }

        protected void InitInputLayout()
        {
            if (CurrentEffect == null) return;

            if (VertexLayout != null)
                VertexLayout.Dispose();

            VertexLayout = new InputLayout(
                PresentationBase.GraphicsDevice,
                CurrentEffect.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature,
                new[] {
                    new InputElement { SemanticName = "SV_Position", Format = Format.R32G32B32_Float },
                    new InputElement { SemanticName = "NORMAL", Format = Format.R32G32B32_Float, AlignedByteOffset = InputElement.AppendAligned },
                    new InputElement { SemanticName = "TEXCOORD", Format = Format.R32G32_Float, AlignedByteOffset = InputElement.AppendAligned }
                }
                );
        }
    }
}
