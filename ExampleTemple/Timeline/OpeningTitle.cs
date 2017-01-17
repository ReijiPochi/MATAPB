using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MATAPB;
using MATAPB.Timeline;

using MATAPB.Objects;
using ExampleTemple.Worlds;

namespace ExampleTemple.Timeline
{
    public class OpeningTitle : TimelineObject
    {
        public OpeningTitle()
        {
        }

        public OpeningWorld opWorld = new OpeningWorld();

        public override void Start()
        {
            base.Start();

            PresentationArea.World = opWorld;

            opWorld.logo.ColorTextureTag.Opacity.Value = 1.0;
        }

        public override void End()
        {
            base.End();

            opWorld.logo.ColorTextureTag.Opacity.Value = 0.0;
        }
    }
}
