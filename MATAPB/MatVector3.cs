using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

namespace MATAPB
{
    public class MatVector3
    {
        public MatVector3()
        {

        }

        public MatVector3(double value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public MatVector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }


        public double X;
        public double Y;
        public double Z;

        public static bool ValueEquals(MatVector3 v1, MatVector3 v2)
        {
            if (v1 == null || v2 == null) return false;

            return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z);
        }

        public static Vector3 ToSlimDXVector3(MatVector3 from)
        {
            if (from == null) return new Vector3(0.0f);

            return new Vector3((float)from.X, (float)from.Y, (float)from.Z);
        }

        public static void Copy(MatVector3 from, MatVector3 to)
        {
            to.X = from.X;
            to.Y = from.Y;
            to.Z = from.Z;
        }

        public static double Distance(MatVector3 v1, MatVector3 v2)
        {
            double d1 = v1.X - v2.X, d2 = v1.Y - v2.Y, d3 = v1.Z - v2.Z;
            return Math.Abs(d1 * d1 + d2 * d2 + d3 * d3);
        }

        public static MatVector3 Normalize(MatVector3 v)
        {
            double dist = Math.Abs(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            return new MatVector3(v.X / dist, v.Y / dist, v.Z / dist);
        }

        public static MatVector3 Cross(MatVector3 v1, MatVector3 v2)
        {
            return new MatVector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        public static MatVector3 Direction(MatVector3 from, MatVector3 to)
        {
            return new MatVector3(to.X - from.X, to.Y - from.Y, to.Z - from.Z);
        }

        public static MatVector3 Add(MatVector3 v1, MatVector3 v2)
        {
            return new MatVector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static MatVector3 Multiply(MatVector3 v, double constant)
        {
            return new MatVector3(v.X * constant, v.Y * constant, v.Z * constant);
        }

        public static MatVector3 InternalDivision(MatVector3 v1, MatVector3 v2, double ratio1, double ratio2)
        {
            double sum = ratio1 + ratio2;
            return new MatVector3(
                (v1.X * ratio2 + v2.X * ratio1) / sum,
                (v1.Y * ratio2 + v2.Y * ratio1) / sum,
                (v1.Z * ratio2 + v2.Z * ratio1) / sum);
        }
    }
}
