using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Gaming.FPS;
using MATAPB.Objects;
using MATAPB.Objects.Primitive;
using MATAPB;
using MATAPB.Objects.Tags;
using System.Numerics;

namespace ExampleTemple
{
    public class Player : PlayerBase
    {
        public Player()
        {
            //scoreBg.Tags.AddTag(new SolidColor(SolidColorOverwriteMode.ColorAndAlpha, new MatColor(0.4, 0.1, 0.1, 0.1)));
        }

        //Text score = new Text(500, 200, new MatColor(1, 1, 1, 1));
        //MATAPB.Objects.Primitive.Plane scoreBg = new MATAPB.Objects.Primitive.Plane(0.25, 0.1, Orientations.plusZ);

        //Vector3 offset = new Vector3(1.0f, -0.5f, 0);

        public override void Draw(RenderingContext context)
        {
            //scoreBg.PSRTag.Position = PlayerCam.Target + offset;
            //scoreBg.PSRTag.Rotation = new System.Numerics.Vector3((float)(angleUD), (float)(-angleLR), 0);
            //score.Draw(context);
            //scoreBg.Draw(context);
        }
    }
}
