using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB
{
    public struct VertexData
    {
        public MatVector3Float position;
        public MatVector3Float normal;
        public MatVector2Float texCoord;

        public static int SizeInBytes
        {
            get { return System.Runtime.InteropServices.Marshal.SizeOf(typeof(VertexData)); }
        }
    }
}
