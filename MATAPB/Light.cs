using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

namespace MATAPB
{
    public class Light
    {
        public LightMode Mode { get; set; } = LightMode.Parallel;

        public Vector4 Color { get; set; } = new Vector4(1.0f, 1.0f, 1.0f, 0);

        public Vector4 Direction { get; set; } = Vector4.UnitZ;

        public Vector4 Ambitent { get; set; } = Vector4.Zero;

        public Vector4 LambertConstant { get; set; } = new Vector4(0.3f, 0.7f, 0, 0);
    }
}
