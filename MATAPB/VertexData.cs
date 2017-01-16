using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Vector2 = System.Numerics.Vector2;

namespace MATAPB
{
    public struct VertexData
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 texCoord;

        public static int SizeInBytes
        {
            get { return System.Runtime.InteropServices.Marshal.SizeOf(typeof(VertexData)); }
        }
    }
}
