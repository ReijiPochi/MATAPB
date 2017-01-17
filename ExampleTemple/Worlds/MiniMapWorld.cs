using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB;
using MATAPB.Objects;
using MATAPB.Objects.Tags;

namespace ExampleTemple.Worlds
{
    public class MiniMapWorld : World
    {
        public MiniMapWorld()
        {
            Objects.Add(map);
            ActiveCamera = cam;
        }

        Picture map = new Picture(@"Objects\Map.png");

        public CameraOrthographic cam = new CameraOrthographic()
        {
            CameraHeight = 2.0,
            CameraWidth = 2.0,
        };
    }
}
