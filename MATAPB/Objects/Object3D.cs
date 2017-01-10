using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using MATAPB.Input;
using MATAPB.Objects.Tags;

namespace MATAPB.Objects
{
    public class Object3D : RenderableObject
    {
        public Object3D()
        {
            DrawMode = PrimitiveTopology.TriangleList;

            Tags = new TagCollection();
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

            Tags = new TagCollection();
            PSRTag = new PSR();
            CameraTag = new Tags.Camera();
            Tags.AddTag(new Tag[] { PSRTag, CameraTag });

            Visible = true;
        }

        public VertexData[] vertices;
        public int[] indices;
        public SlimDX.Direct3D11.Buffer VertexBuffer { get; protected set; }
        public SlimDX.Direct3D11.Buffer IndexBuffer { get; protected set; }
        public int VertexCount { get; protected set; }
        public int IndexCount { get; protected set; }


        public PrimitiveTopology DrawMode { get; protected set; }

        public TagCollection Tags { get; protected set; }

        public PSR PSRTag { get; protected set; }

        public Tags.Camera CameraTag { get; protected set; }

        public override void Draw(RenderingContext context)
        {
            if (!Visible || VertexBuffer == null) return;

            PresentationArea.GraphicsDevice.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, VertexData.SizeInBytes, 0));
            PresentationArea.GraphicsDevice.ImmediateContext.InputAssembler.PrimitiveTopology = DrawMode;

            if (Tags != null) Tags.PrepareToDraw(context);

            if (IndexBuffer == null)
            {
                PresentationArea.GraphicsDevice.ImmediateContext.Draw(VertexCount, 0);
            }
            else
            {
                PresentationArea.GraphicsDevice.ImmediateContext.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
                PresentationArea.GraphicsDevice.ImmediateContext.DrawIndexed(IndexCount, 0, 0);
            }
        }

        protected void InitVertexBuffer(VertexData[] vertices)
        {
            using (DataStream vertexStream = new DataStream(vertices, true, true))
            {
                VertexBuffer = new SlimDX.Direct3D11.Buffer(
                    PresentationArea.GraphicsDevice,
                    vertexStream,
                    new BufferDescription { SizeInBytes = (int)vertexStream.Length, BindFlags = BindFlags.VertexBuffer }
                    );
            }

            VertexCount = vertices.Length;
        }

        protected void InitIndexBuffer(int[] indices)
        {
            using (DataStream indexStream = new DataStream(indices, true, true))
            {
                IndexBuffer = new SlimDX.Direct3D11.Buffer(
                    PresentationArea.GraphicsDevice,
                    indexStream,
                    new BufferDescription { SizeInBytes = (int)indexStream.Length, BindFlags = BindFlags.IndexBuffer }
                    );
            }

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
