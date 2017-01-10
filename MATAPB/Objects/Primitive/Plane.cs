using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            double w2 = w / 2.0, h2 = h / 2.0;
            switch(orientation)
            {
                case Orientations.plusX:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new MatVector3Float(0, h,-w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,0) },
                        new VertexData(){ position=new MatVector3Float(0, h, w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,0) },
                        new VertexData(){ position=new MatVector3Float(0,-h,-w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,1) },
                        new VertexData(){ position=new MatVector3Float(0,-h, w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,1) }
                    };
                    break;

                case Orientations.minusX:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new MatVector3Float(0, h, w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,0) },
                        new VertexData(){ position=new MatVector3Float(0, h,-w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,0) },
                        new VertexData(){ position=new MatVector3Float(0,-h, w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,1) },
                        new VertexData(){ position=new MatVector3Float(0,-h,-w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,1) }
                    };
                    break;

                case Orientations.plusY:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new MatVector3Float( h, 0,-w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,0) },
                        new VertexData(){ position=new MatVector3Float( h, 0, w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,0) },
                        new VertexData(){ position=new MatVector3Float(-h, 0,-w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,1) },
                        new VertexData(){ position=new MatVector3Float(-h, 0, w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,1) }
                    };
                    break;

                case Orientations.minusY:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new MatVector3Float( h, 0, w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,0) },
                        new VertexData(){ position=new MatVector3Float( h, 0,-w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,0) },
                        new VertexData(){ position=new MatVector3Float(-h, 0, w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,1) },
                        new VertexData(){ position=new MatVector3Float(-h, 0,-w), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,1) }
                    };
                    break;

                case Orientations.plusZ:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new MatVector3Float(-w, h, 0), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,0) },
                        new VertexData(){ position=new MatVector3Float( w, h, 0), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,0) },
                        new VertexData(){ position=new MatVector3Float(-w,-h, 0), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,1) },
                        new VertexData(){ position=new MatVector3Float( w,-h, 0), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,1) }
                    };
                    break;

                case Orientations.minusZ:
                    vertices = new VertexData[]
                    {
                        new VertexData(){ position=new MatVector3Float( w, h, 0), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,0) },
                        new VertexData(){ position=new MatVector3Float(-w, h, 0), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,0) },
                        new VertexData(){ position=new MatVector3Float( w,-h, 0), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(0,1) },
                        new VertexData(){ position=new MatVector3Float(-w,-h, 0), normal=new MatVector3Float(1,0,0), texCoord=new MatVector2Float(1,1) }
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
