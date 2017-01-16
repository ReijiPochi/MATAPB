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
using Vector3 = System.Numerics.Vector3;

namespace ExampleTemple.Worlds
{
    public class TempleWorld : World
    {
        public TempleWorld()
        {
            hero.Map = mapArea;
            //hero.PlayerCam = new Camera3D();
            hero.PlayerCam = new CameraPerspective();
            hero.PlayerCam.Eye = new Vector3(0.0f, 1.0f, 0.0f);
            hero.PlayerCam.Up = new Vector3(0.0f, 1.0f, 0.0f);
            ActiveCamera = hero.PlayerCam;

            //map.PSRTag.Scale = new MatVector3(0.1);
            map.Tags.AddTag(new Tag[] { new ColorTexture(@"Objects\Map.png"), new Lighting() });
            sky.Tags.AddTag(new ColorTexture(@"Objects\Skydome.png"));

            image.Tags.InsertToFirst(hopup);
            image.PSRTag.Position = new Vector3(0.0f, 2.0f, -6.2f);
            image.PSRTag.Rotation = new Vector3(-0.3f, 0, 0);

            test.Tags.ClearAndSet(new LookCamera() { Scale = new Vector3(0.05f), Position = new Vector3(0.0f, 1.5f, -6.2f) });
            test.Tags.AddTag(new ColorTexture(@"Objects\sankaku.png"));
            test.Tags.InsertToFirst(hopup2);
            hopup2.Hop();

            Objects.Add(map);
            //Objects.Add(mapArea);
            Objects.Add(sky);
            Objects.Add(image);
            OverlayObjects.Add(test);

            hitArea.MineHit += HitArea_MineHit;
            hitArea.MineLeave += HitArea_MineLeave;
            hitArea.MinePosition = image.PSRTag.Position;
            hitArea.MineRadius = 2.0;
            hitArea.Hysteresis = 0.5;
        }

        Object3D map = new Object3D(@"Objects\Map.obj");
        Object3D mapArea = new Object3D(@"Objects\MapArea.obj");
        Object3D sky = new Object3D(@"Objects\sky.obj");
        Picture image = new Picture(@"Objects/ほのぼの神社.png");
        Mine hitArea = new Mine();
        Hopup hopup = new Hopup()
        {
            HopupAnimation = HopupAnimations.PopLiner,
            HoverAnimation = HoverAnimations.Wave,
            WaveRate = 0.5,
            WaveHeight = 0.05,
            HopupTime = 0.3,
            MaxPosition = new Vector3(0, 0.5f, 0),
            CloseAnimation = CloseAnimations.DepopLiner
        };
        Hopup hopup2 = new Hopup()
        {
            HopupAnimation = HopupAnimations.None,
            HoverAnimation = HoverAnimations.Wave,
            WaveRate = 0.8,
            WaveHeight = 0.5,
            CloseAnimation = CloseAnimations.None
        };
        Object3D test = new MATAPB.Objects.Primitive.Plane(1, 1, Orientations.plusZ);


        private void HitArea_MineHit()
        {
            hopup.Hop();
        }

        private void HitArea_MineLeave()
        {
            hopup.Close();
        }

        MATAPB.CameraPerspective cam1 = new MATAPB.CameraPerspective()
        {
            Eye = new Vector3(0.0f, 2.0f, 5.0f),
            Target = new Vector3(0.0f, 0.0f, 0.0f),
            Up = new Vector3(0.0f, 1.0f, 0.0f),
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
                deltaAngleLR = mouseDelta.X * 0.3,
                deltaAngleUD = mouseDelta.Y * 0.3
            };

            if (Keyboard.KeyStates[Key.D])
            {
                if (Keyboard.KeyStates[Key.A])
                    data.speedLR = 0;
                else
                    data.speedLR = -3;
            }
            else if (Keyboard.KeyStates[Key.A]) data.speedLR = 3;
            else data.speedLR = 0;

            if (Keyboard.KeyStates[Key.W])
            {
                if (Keyboard.KeyStates[Key.S])
                    data.speedFB = 0;
                else
                    data.speedFB = 3;
            }
            else if (Keyboard.KeyStates[Key.S]) data.speedFB = -3;
            else data.speedFB = 0;

            if (Keyboard.KeyStates[Key.LeftShift])
            {
                data.speedFB *= 2.0;
                data.speedLR *= 2.0;
            }

            if (Keyboard.KeyStates[Key.Space])
            {
                data.height = 2.5;
            }
            else if(Keyboard.KeyStates[Key.LeftCtrl])
            {
                data.height = 0.9;
            }
            else
            {
                data.height = 1.6;
            }

            hitArea.TargetPosition = hero.PlayerCam.Eye;

            hero.Move(data);
        }
    }
}
