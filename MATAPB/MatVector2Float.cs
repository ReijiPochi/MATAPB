using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB
{
    public struct MatVector2Float
    {
        public MatVector2Float(double x,double y)
        {
            X = (float)x;
            Y = (float)y;
        }

        public float X;
        public float Y;
    }
}
