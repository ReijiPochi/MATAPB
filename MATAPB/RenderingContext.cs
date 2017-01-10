using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects;

namespace MATAPB
{
    public class RenderingContext
    {
        public MatVector2 viewArea;
        public SlimDX.Direct3D11.Buffer cbuffer;
        public Camera cam;
    }
}
