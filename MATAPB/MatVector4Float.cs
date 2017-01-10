using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB
{
    public struct MatVector4Float
    {
        public MatVector4Float(double x, double y, double z, double w)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
            W = (float)w;
        }

        public float X;
        public float Y;
        public float Z;
        public float W;

        public static bool ValueEquals(MatVector4Float v1, MatVector4Float v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z) && (v1.W == v2.W);
        }

        public static MatVector4Float Normalize(MatVector4Float vector)
        {
            double dist = vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z + vector.W * vector.W;
            dist = Math.Sqrt(dist);

            return new MatVector4Float(vector.X / dist, vector.Y / dist, vector.Z / dist, vector.W / dist);
        }
    }
}
