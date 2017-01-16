using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace MATAPB
{
    public struct MatVector3Float
    {
        public MatVector3Float(double x, double y, double z)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
        }

        public float X;
        public float Y;
        public float Z;

        public static bool ValueEquals(MatVector3Float v1, MatVector3Float v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z);
        }

        public static Vector3 ToSlimDXVector3(MatVector3Float from)
        {
            return new Vector3(from.X, from.Y, from.Z);
        }

        public static MatVector3 ToMatVector3(MatVector3Float from)
        {
            return new MatVector3(from.X, from.Y, from.Z);
        }
    }
}
