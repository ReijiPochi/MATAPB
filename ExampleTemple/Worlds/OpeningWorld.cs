using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB;
using MATAPB.Objects;

namespace ExampleTemple.Worlds
{
    public class OpeningWorld : World
    {
        public OpeningWorld()
        {
            ActiveCamera = cam;
        }

        CameraOrthographic cam = new CameraOrthographic();

    }
}
