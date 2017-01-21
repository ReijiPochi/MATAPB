using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB;
using MATAPB.Timeline;
using ExampleTemple.Worlds;

namespace ExampleTemple.Timeline
{
    public class Gaming : TimelineObject
    {
        public TempleWorld templeWorld = new TempleWorld();

        public override void Start()
        {
            base.Start();

            PresentationBase.World = templeWorld;

            Host.Stop();
        }
    }
}
