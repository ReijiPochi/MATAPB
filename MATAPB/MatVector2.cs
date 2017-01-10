using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB
{
    public class MatVector2
    {
        public MatVector2()
        {

        }

        public MatVector2(double x,double y)
        {
            X = x;
            Y = y;
        }

        public double X;
        public double Y;

        public static double Dot(MatVector2 v1, MatVector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static MatVector2 Normalize(MatVector2 v)
        {
            double dist = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            return new MatVector2(v.X / dist, v.Y / dist);
        }
    }
}
