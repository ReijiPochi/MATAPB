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
using System.Numerics;
using MATAPB._2D;

using Keyboard = MATAPB.Input.Keyboard;
using Vector3 = System.Numerics.Vector3;

namespace ExampleTemple.Worlds
{
    public class TempleWorld : World
    {
        public TempleWorld()
        {
            GlobalLight1.Color = new Vector4(1.1f, 1.1f, 1.1f, 0.0f);
            GlobalLight1.Direction = new Vector4(2.0f, -2.0f, -10.0f, 0);
            GlobalLight1.Ambitent = new Vector4(0.0f, 0.03f, 0.1f, 0);

            hero.Map = mapArea;
            //hero.PlayerCam = new Camera3D();
            hero.PlayerCam = new CameraPerspective();
            hero.PlayerCam.Eye = new Vector3(0.0f, 1.0f, 0.0f);
            hero.PlayerCam.Up = new Vector3(0.0f, 1.0f, 0.0f);
            hero.PlayerCam.FieldOfView = 100.0;
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

            text.TextValue = "Welcom";
            text.FontSize = 50;
            text.PSRTag.Position = Vector3.UnitY;

            //miniMapCanvas = new RenderingCanvas(miniMap);
            //miniMapObj.Tags.AddTag(new ColorTexture(miniMap));
            //miniMapObj.PSRTag.Position = new Vector3(0, 1, 0);

            Objects.Add(map);
            //Objects.Add(mapArea);
            Objects.Add(sky);
            Objects.Add(image);
            Objects.Add(text);
            OverlayObjects.Add(test);
            OverlayObjects.Add(hero);


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
            WaveRate = 0.4,
            WaveHeight = 0.02,
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
        Text text = new Text(400, 100, MatColor.Green);

        HUDWorld hudWorld = new HUDWorld();
        MiniMapWorld miniMapWorld = new MiniMapWorld();

        MATAPB.PostEffect.SSAO ssao = new MATAPB.PostEffect.SSAO();

        private void HitArea_MineHit()
        {
            hopup.Hop();
        }

        private void HitArea_MineLeave()
        {
            hopup.Close();
        }

        Player hero = new Player()
        {
        };

        public override void Render(RenderingContext context)
        {
            MovePlayer();

            miniMapWorld.map.PSRTag.Position = new Vector3((float)(hero.PlayerCam.Eye.X / 100.0), (float)(-hero.PlayerCam.Eye.Z / 100.0), 0.0f);
            miniMapWorld.map.PSRTag.Rotation = new Vector3(0, 0, (float)hero.angleLR);

            hudWorld.miniMapCanvas.SetCanvas();
            {
                hudWorld.miniMapCanvas.ClearCanvas();
                context.canvas = hudWorld.miniMapCanvas;
                miniMapWorld.Render(context);
            }

            PresentationBase.DefaultCanvas.SetCanvas();
            {
                context.canvas = PresentationBase.DefaultCanvas;
                base.Render(context);

                hudWorld.Render(context);
            }

            ssao.Apply(PresentationBase.DefaultCanvas);
        }

        private void MovePlayer()
        {


            Point mouseDelta = MATAPB.Input.Mouse.GetDelta();

            MoveData data = new MoveData()
            {
                deltaAngleLR = mouseDelta.X * 0.3,
                deltaAngleUD = mouseDelta.Y * 0.3
            };

            data.fov = 70.0;

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
