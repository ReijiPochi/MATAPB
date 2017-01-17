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

            logo.ColorTextureTag.Opacity.ActualValue = 0.0;
            logo.ColorTextureTag.Opacity.Mode = AnimationMode.Liner;
            logo.ColorTextureTag.Opacity.Delta = 0.02;
            logo.ColorTextureTag.Opacity.Threshold = 0.02;
            
            Objects.Add(logo);
        }

        public CameraOrthographic cam = new CameraOrthographic()
        {
            CameraHeight = PresentationArea.ViewArea.ActualHeight / 1000.0,
            CameraWidth = PresentationArea.ViewArea.ActualWidth / 1000.0
        };

        public Picture logo = new Picture(@"Objects\ロゴ.png");
        Object3D sky = new Object3D(@"Objects\sky.obj");
    }
}
