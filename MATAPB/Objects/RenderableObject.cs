using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB.Objects
{
    public abstract class RenderableObject : AutoDisposeObject
    {
        public bool Visible { get; set; } = true;

        public abstract void Draw(RenderingContext context);
    }
}
