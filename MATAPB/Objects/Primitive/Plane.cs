using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Vector2 = System.Numerics.Vector2;

namespace MATAPB.Objects.Primitive
{
    public class Plane : Object3D
    {
        public Plane() : base()
        {

        }

        public Plane(double width, double height, Orientations orientation) : base()
        {
            UpdateBuffer(width, height, orientation);
            Orientation = orientation;
        }

        public Orientations Orientation { get; protected set; }

        public void UpdateBuffer(double w, double h, Orientations orientation)
        {
            float w2 = (float)(w / 2.0), h2 = (float)(h / 2.0);
            switch (orientation)
            {
                case Orientations.plusX:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new Vector3(0, h2,-w2), normal=new Vector3(1,0,0), texCoord=new Vector2(0,0) },
                        new VertexData(){ position=new Vector3(0, h2, w2), normal=new Vector3(1,0,0), texCoord=new Vector2(1,0) },
                        new VertexData(){ position=new Vector3(0,-h2,-w2), normal=new Vector3(1,0,0), texCoord=new Vector2(0,1) },
                        new VertexData(){ position=new Vector3(0,-h2, w2), normal=new Vector3(1,0,0), texCoord=new Vector2(1,1) }
                    };
                    break;

                case Orientations.minusX:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new Vector3(0, h2, w2), normal=new Vector3(1,0,0), texCoord=new Vector2(0,0) },
                        new VertexData(){ position=new Vector3(0, h2,-w2), normal=new Vector3(1,0,0), texCoord=new Vector2(1,0) },
                        new VertexData(){ position=new Vector3(0,-h2, w2), normal=new Vector3(1,0,0), texCoord=new Vector2(0,1) },
                        new VertexData(){ position=new Vector3(0,-h2,-w2), normal=new Vector3(1,0,0), texCoord=new Vector2(1,1) }
                    };
                    break;

                case Orientations.plusY:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new Vector3( h2, 0,-w2), normal=new Vector3(1,0,0), texCoord=new Vector2(0,0) },
                        new VertexData(){ position=new Vector3( h2, 0, w2), normal=new Vector3(1,0,0), texCoord=new Vector2(1,0) },
                        new VertexData(){ position=new Vector3(-h2, 0,-w2), normal=new Vector3(1,0,0), texCoord=new Vector2(0,1) },
                        new VertexData(){ position=new Vector3(-h2, 0, w2), normal=new Vector3(1,0,0), texCoord=new Vector2(1,1) }
                    };
                    break;

                case Orientations.minusY:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new Vector3( h2, 0, w2), normal=new Vector3(1,0,0), texCoord=new Vector2(0,0) },
                        new VertexData(){ position=new Vector3( h2, 0,-w2), normal=new Vector3(1,0,0), texCoord=new Vector2(1,0) },
                        new VertexData(){ position=new Vector3(-h2, 0, w2), normal=new Vector3(1,0,0), texCoord=new Vector2(0,1) },
                        new VertexData(){ position=new Vector3(-h2, 0,-w2), normal=new Vector3(1,0,0), texCoord=new Vector2(1,1) }
                    };
                    break;

                case Orientations.plusZ:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new Vector3(-w2, h2, 0), normal=new Vector3(1,0,0), texCoord=new Vector2(0,0) },
                        new VertexData(){ position=new Vector3( w2, h2, 0), normal=new Vector3(1,0,0), texCoord=new Vector2(1,0) },
                        new VertexData(){ position=new Vector3(-w2,-h2, 0), normal=new Vector3(1,0,0), texCoord=new Vector2(0,1) },
                        new VertexData(){ position=new Vector3( w2,-h2, 0), normal=new Vector3(1,0,0), texCoord=new Vector2(1,1) }
                    };
                    break;

                case Orientations.minusZ:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new Vector3( w2, h2, 0), normal=new Vector3(1,0,0), texCoord=new Vector2(0,0) },
                        new VertexData(){ position=new Vector3(-w2, h2, 0), normal=new Vector3(1,0,0), texCoord=new Vector2(1,0) },
                        new VertexData(){ position=new Vector3( w2,-h2, 0), normal=new Vector3(1,0,0), texCoord=new Vector2(0,1) },
                        new VertexData(){ position=new Vector3(-w2,-h2, 0), normal=new Vector3(1,0,0), texCoord=new Vector2(1,1) }
                    };
                    break;

                default:
                    break;
            }

            indices = new int[] { 0, 1, 2, 2, 1, 3 };

            if (VertexBuffer != null)
                VertexBuffer.Dispose();

            if (IndexBuffer != null)
                IndexBuffer.Dispose();

            InitVertexBuffer(vertices);
            InitIndexBuffer(indices);
        }
    }
}
