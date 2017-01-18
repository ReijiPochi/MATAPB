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
            map.PSRTag.Order = PSROrder.SPR;
            bg.PSRTag.Position = new System.Numerics.Vector3(0, 0, -1);

            Objects.Add(map);
            Objects.Add(bg);
            OverlayObjects.Add(player);

            ActiveCamera = cam;
        }

        public Picture map = new Picture(@"Objects\MiniMap.png");
        Picture bg = new Picture(@"Objects\BG.png", 20);
        Picture player = new Picture(@"Objects\PlayerSymbol.png", 0.25);

        public CameraOrthographic cam = new CameraOrthographic()
        {
            CameraHeight = 0.2,
            CameraWidth = 0.2,
        };
    }
}
