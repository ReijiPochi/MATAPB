using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB
{
    public class Line
    {
        public MatVector3 a;
        public MatVector3 b;

        public double aX { get { return a.X; } }
        public double aZ { get { return a.Z; } }
        public double bX { get { return b.X; } }
        public double bZ { get { return b.Z; } }
    }
}
