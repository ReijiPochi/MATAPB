using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using MATAPB.Input;
using MATAPB.Objects.Tags;

namespace MATAPB.Objects
{
    public class Object3D : RenderableObject
    {
        public Object3D()
        {
            DrawMode = PrimitiveTopology.TriangleList;

            Tags = new TagCollection() { AssociatedObject = this };
            PSRTag = new PSR();
            CameraTag = new Tags.Camera();
            Tags.AddTag(new Tag[] { PSRTag, CameraTag});

            Visible = true;
        }

        public Object3D(string path)
        {
            vertices = ObjLoader.FromFile(path, out indices);

            InitVertexBuffer(vertices);
            InitIndexBuffer(indices);

            DrawMode = PrimitiveTopology.TriangleList;

            Tags = new TagCollection() { AssociatedObject = this };
            PSRTag = new PSR();
            CameraTag = new Tags.Camera();
            Tags.AddTag(new Tag[] { PSRTag, CameraTag });

            Visible = true;
        }

        public VertexData[] vertices;
        public int[] indices;
        public SharpDX.Direct3D11.Buffer VertexBuffer { get; protected set; }
        public SharpDX.Direct3D11.Buffer IndexBuffer { get; protected set; }
        public int VertexCount { get; protected set; }
        public int IndexCount { get; protected set; }


        public PrimitiveTopology DrawMode { get; protected set; }

        public TagCollection Tags { get; protected set; }

        public PSR PSRTag { get; protected set; }

        public Tags.Camera CameraTag { get; protected set; }

        public override void Draw(RenderingContext context)
        {
            if (!Visible || VertexBuffer == null) return;

            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, VertexData.SizeInBytes, 0));
            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.PrimitiveTopology = DrawMode;

            if (Tags != null) Tags.PrepareToDraw(context);

            if (IndexBuffer == null)
            {
                PresentationBase.GraphicsDevice.ImmediateContext.Draw(VertexCount, 0);
            }
            else
            {
                PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
                PresentationBase.GraphicsDevice.ImmediateContext.DrawIndexed(IndexCount, 0, 0);
            }
        }

        protected void InitVertexBuffer(VertexData[] vertices)
        {
            VertexBuffer = SharpDX.Direct3D11.Buffer.Create(PresentationBase.GraphicsDevice, BindFlags.VertexBuffer, vertices);

            VertexCount = vertices.Length;
        }

        protected void InitIndexBuffer(int[] indices)
        {
            IndexBuffer = SharpDX.Direct3D11.Buffer.Create(PresentationBase.GraphicsDevice, BindFlags.IndexBuffer, indices);

            IndexCount = indices.Length;
        }

        protected override void OnDispose()
        {
            if (VertexBuffer != null) VertexBuffer.Dispose();
            if (IndexBuffer != null) IndexBuffer.Dispose();
            if (Tags != null) Tags.Dispose();
        }
    }
}
