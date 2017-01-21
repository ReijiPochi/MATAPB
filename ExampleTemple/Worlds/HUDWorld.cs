using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB;
using MATAPB.Objects;
using MATAPB.Objects.Primitive;
using MATAPB.Objects.Tags;
using System.Numerics;

namespace ExampleTemple.Worlds
{
    public class HUDWorld : World
    {
        public HUDWorld()
        {
            ActiveCamera = cam;

            miniMap.PSRTag.Position = new System.Numerics.Vector3((float)(-cam.CameraWidth / 2 + 0.2), (float)(-cam.CameraHeight / 2 + 0.2), 0);
            miniMap.PSRTag.Scale = new System.Numerics.Vector3(0.3f);

            miniMapCanvas = new RenderingCanvas(miniMapTex);
            ColorTexture miniMapTexTag = new ColorTexture(miniMapTex);
            miniMapTexTag.Opacity.Value = 0.85;
            miniMap.Tags.AddTag(miniMapTexTag);

            scoreBg.Tags.AddTag(new SolidColor(SolidColorOverwriteMode.ColorAndAlpha, new MatColor(0.4, 0.1, 0.1, 0.1)));
            scoreBg.PSRTag.Position = new Vector3((float)(cam.CameraWidth / 2 - 0.2), (float)(-cam.CameraHeight / 2 + 0.1), 0.3f);
            scoreBg.PSRTag.Rotation = new Vector3(0.0f, -0.2f, 0.0f);
            scoreBg.CameraTag.UseCustomCamera = true;
            scoreBg.CameraTag.CustomCamera = customCam;

            score.TextValue = "100/100";
            score.FontSize = 100;
            score.PSRTag.Position = scoreBg.PSRTag.Position;
            score.PSRTag.Rotation = scoreBg.PSRTag.Rotation;
            score.PSRTag.Scale = new Vector3(0.5f);
            score.CameraTag.UseCustomCamera = true;
            score.CameraTag.CustomCamera = customCam;

            Objects.Add(miniMap);
            Objects.Add(scoreBg);
            OverlayObjects.Add(score);
        }

        Texture miniMapTex = new Texture(1000, 1000);
        public RenderingCanvas miniMapCanvas;
        Picture miniMap = new Picture(1, 1);

        CameraPerspective customCam = new CameraPerspective()
        {
            Eye = Vector3.UnitZ,
            Target = Vector3.Zero,
            Up = Vector3.UnitY,
            FieldOfView = 70
        };

        Text score = new Text(500, 200, new MatColor(1, 1, 1, 1));
        MATAPB.Objects.Primitive.Plane scoreBg = new MATAPB.Objects.Primitive.Plane(0.25, 0.1, Orientations.plusZ);

        double count = 100.0;

        public CameraOrthographic cam = new CameraOrthographic()
        {
            CameraHeight = PresentationBase.ViewArea.ActualHeight / 1000.0,
            CameraWidth = PresentationBase.ViewArea.ActualWidth / 1000.0
        };

        public override void Render(RenderingContext context)
        {
            count -= 0.01;
            score.TextValue = count.ToString("000") + "/100";

            customCam.CameraUpdate(context);

            base.Render(context);
        }
    }
}
