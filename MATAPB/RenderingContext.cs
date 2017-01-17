using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = System.Numerics.Vector2;

using MATAPB.Objects;

namespace MATAPB
{
    public class RenderingContext
    {
        public Vector2 viewArea;
        public SharpDX.Direct3D11.Buffer cbuffer;
        public Camera cam;
        public RenderingCanvas canvas;
    }
}
