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
    public class HUDWorld : World
    {
        public HUDWorld()
        {
            ActiveCamera = cam;

            miniMap.PSRTag.Position = new System.Numerics.Vector3((float)(-cam.CameraWidth / 2 + 0.2), (float)(-cam.CameraHeight / 2 + 0.2), 0);
            miniMap.PSRTag.Scale = new System.Numerics.Vector3(0.3f);

            miniMapCanvas = new RenderingCanvas(miniMapTex);
            ColorTexture miniMapTexTag = new ColorTexture(miniMapTex);
            miniMapTexTag.Opacity.Value = 0.7;
            miniMap.Tags.AddTag(miniMapTexTag);

            Objects.Add(miniMap);
        }

        Texture miniMapTex = new Texture(1000, 1000);
        public RenderingCanvas miniMapCanvas;
        Picture miniMap = new Picture(1, 1);

        public CameraOrthographic cam = new CameraOrthographic()
        {
            CameraHeight = PresentationArea.ViewArea.ActualHeight / 1000.0,
            CameraWidth = PresentationArea.ViewArea.ActualWidth / 1000.0
        };

        public override void Render(RenderingContext context)
        {
            base.Render(context);
        }
    }
}
