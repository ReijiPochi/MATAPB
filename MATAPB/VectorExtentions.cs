using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;

namespace MATAPB.Vector3Extentions
{
    static class VectorExtentions
    {
        public static Vector3 InternalDivision(this Vector3 a, Vector3 b, double n, double m)
        {
            double nm = n + m;
            return new Vector3(
                (float)((a.X * m + b.X * n) / nm),
                (float)((a.Y * m + b.Y * n) / nm),
                (float)((a.Z * m + b.Z * n) / nm));
        }
    }
}
