using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB;
using MATAPB.Objects;
using MATAPB.Objects.Tags;
using MATAPB.Gaming.FPS;
using System.Windows;
using System.Windows.Input;
using MATAPB.Input;
using MATAPB.Gaming;

using Keyboard = MATAPB.Input.Keyboard;

namespace ExampleTemple.Worlds
{
    public class TempleWorld : World
    {
        public TempleWorld()
        {
            hero.Map = mapArea;
            hero.PlayerCam = new Camera3D();
            hero.PlayerCam.Eye = new MatVector3(0.0, 1.0, 0.0);
            hero.PlayerCam.Up = new MatVector3(0.0, 1.0, 0.0);
            ActiveCamera = hero.PlayerCam;

            map.Tags.AddTag(new Tag[] { new ColorTexture(@"Objects\Map.png"), new Lighting() });
            sky.Tags.AddTag(new ColorTexture(@"Objects\Skydome.png"));
            testImage.Position = new MatVector3(0.0, 2.5, -6.2);
            testImage.Rotation.X = -0.2;

            Objects.Add(map);
            Objects.Add(sky);
            Objects.Add(testImage);

            hitArea.MineHit += HitArea_MineHit;
            hitArea.MineLeave += HitArea_MineLeave;
            hitArea.MinePosition = testImage.Position;
            hitArea.TargetPosition = hero.PlayerCam.Eye;
            hitArea.MineRadius = 3.0;
            hitArea.Hysteresis = 1.0;
        }

        Object3D map = new Object3D(@"Objects\Map.obj");
        Object3D mapArea = new Object3D(@"Objects\MapArea.obj");
        Object3D sky = new Object3D(@"Objects\sky.obj");
        HopupImage testImage = new HopupImage(@"Objects/ほのぼの神社.png", Orientations.plusZ)
        {
            Scale = 0.0008
        };
        Mine hitArea = new Mine();


        private void HitArea_MineHit()
        {
            testImage.Hop();
        }

        private void HitArea_MineLeave()
        {
            testImage.Remove();
        }

        MATAPB.Camera cam1 = new MATAPB.Camera()
        {
            Eye = new MatVector3(0.0, 2.0, 5.0),
            Target = new MatVector3(0.0, 0.0, 0.0),
            Up = new MatVector3(0.0, 1.0, 0.0),
            FieldOfView = 70.0
        };

        Player hero = new Player()
        {
        };

        public override void Render(RenderingContext context)
        {
            MovePlayer();
            


            base.Render(context);
        }

        private void MovePlayer()
        {
            if (Keyboard.KeyStates[Key.Escape])
            {
                Application.Current.Dispatcher.Invoke(() => { Application.Current.Shutdown(); });
            }

            Point mouseDelta = MATAPB.Input.Mouse.GetDelta();

            MoveData data = new MoveData()
            {
                deltaAngleLR = mouseDelta.X,
                deltaAngleUD = mouseDelta.Y
            };

            if (Keyboard.KeyStates[Key.D])
            {
                if (Keyboard.KeyStates[Key.A])
                    data.speedLR = 0.00;
                else
                    data.speedLR = -0.03;
            }
            else if (Keyboard.KeyStates[Key.A]) data.speedLR = 0.03;
            else data.speedLR = 0.0;

            if (Keyboard.KeyStates[Key.W])
            {
                if (Keyboard.KeyStates[Key.S])
                    data.speedFB = 0.00;
                else
                    data.speedFB = 0.03;
            }
            else if (Keyboard.KeyStates[Key.S]) data.speedFB = -0.03;
            else data.speedFB = 0.0;

            if (Keyboard.KeyStates[Key.LeftShift])
            {
                data.speedFB *= 2.0;
                data.speedLR *= 2.0;
            }

            hero.Move(data);
        }
    }
}
