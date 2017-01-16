using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;

namespace MATAPB
{
    public class Line
    {
        public Vector3 a;
        public Vector3 b;

        public float aX { get { return a.X; } }
        public float aZ { get { return a.Z; } }
        public float bX { get { return b.X; } }
        public float bZ { get { return b.Z; } }
    }
}
