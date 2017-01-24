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

namespace MATAPB.PostEffect
{
    public abstract class PostEffect
    {
        public PostEffect()
        {
            TargetPlane.Tags.ClearTag();
        }

        public Effect CurrentEffect { get; protected set; }
        public InputLayout VertexLayout { get; protected set; }
        protected Plane TargetPlane { get; } = new Plane(2, 2, Orientations.plusZ);

        public abstract void Apply(RenderingCanvas target);

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
