using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using MATAPB.Objects.Tags;

namespace MATAPB.Objects.Primitive
{
    public abstract class Line : Object3D
    {
        public Line()
        {
            DrawMode = SharpDX.Direct3D.PrimitiveTopology.LineStrip;

            Tags = new TagCollection() { AssociatedObject = this };

            PSRTag = new PSR();
            CameraTag = new Tags.Camera();
            ColorTag = new SolidColor(SolidColorOverwriteMode.ColorAndAlpha, new MatColor(1, 0, 0, 0));
            LineTag = new LineThicknessGS();

            Tags.AddTag(new Tag[] { PSRTag, CameraTag, ColorTag, LineTag });
        }

        public int CountOfPoints { get; set; } = 2;

        public MatColor Color
        {
            get { return ColorTag.Color; }
            set { ColorTag.Color = value; }
        }

        public SolidColor ColorTag { get; }
        public LineThicknessGS LineTag { get; }
        
        public void UpdateBuffer()
        {
            double a = 0.0, deltaA = 1.0 / (CountOfPoints - 1);

            vertices = new VertexData[CountOfPoints];

            for(int c = 0; c < CountOfPoints; c++)
            {
                Vector3 point = LineFormula(a);
                a += deltaA;

                vertices[c].position = point;
                vertices[c].normal = Vector3.UnitZ;
            }

            if (VertexBuffer != null) VertexBuffer.Dispose();
            InitVertexBuffer(vertices);
        }

        public abstract Vector3 LineFormula(double a);
    }
}
