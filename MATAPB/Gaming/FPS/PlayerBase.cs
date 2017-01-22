using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using MATAPB.Objects;

namespace MATAPB.Gaming.FPS
{
    public class PlayerBase : RenderableObject
    {
        public PlayerBase()
        {
            PlayerCam = new CameraPerspective()
            {
                Eye = new Vector3(0.0f, 1.0f, 0.0f),
                Up = new Vector3(0.0f, 1.0f, 0.0f)
            };
        }

        public PlayerBase(CameraPerspective cam)
        {
            PlayerCam = cam;
        }

        public Object3D Map { get; set; }

        public CameraPerspective PlayerCam { get; set; }

        public double SpeedLRDelta { get; set; } = 20.0;
        public double SpeedFBDelta { get; set; } = 20.0;
        public double FOVDelta { get; set; } = 200.0;
        public double Inertia { get; set; } = 10.0;

        double actualSpeedLR, actualSpeedFB;
        double actualSpeedX, actualSpeedZ;
        public double angleLR = 0.0, angleUD = 0.0;
        double heightOffset = 0.0, targetHeight = 0.0;
        double inertiaX, inertiaZ;

        public virtual void Move(MoveData data)
        {
            if (data != null)
            {
                angleLR += data.deltaAngleLR * PresentationBase.TimelengthOfFrame;
                angleUD += data.deltaAngleUD * PresentationBase.TimelengthOfFrame;

                if (angleUD > Math.PI * 0.45)
                    angleUD = Math.PI * 0.45;
                else if (angleUD < -Math.PI * 0.45)
                    angleUD = -Math.PI * 0.45;

                if (data.speedLR > actualSpeedLR) actualSpeedLR += SpeedLRDelta * PresentationBase.TimelengthOfFrame;
                else if (data.speedLR < actualSpeedLR) actualSpeedLR -= SpeedLRDelta * PresentationBase.TimelengthOfFrame;

                if (data.speedFB > actualSpeedFB) actualSpeedFB += SpeedFBDelta * PresentationBase.TimelengthOfFrame;
                else if (data.speedFB < actualSpeedFB) actualSpeedFB -= SpeedFBDelta * PresentationBase.TimelengthOfFrame;

                if (data.fov > PlayerCam.FieldOfView + FOVDelta * PresentationBase.TimelengthOfFrame) PlayerCam.FieldOfView += FOVDelta * PresentationBase.TimelengthOfFrame;
                else if (data.fov < PlayerCam.FieldOfView - FOVDelta * PresentationBase.TimelengthOfFrame) PlayerCam.FieldOfView -= FOVDelta * PresentationBase.TimelengthOfFrame;
                else PlayerCam.FieldOfView = data.fov;


                if (Math.Abs(actualSpeedLR) < SpeedLRDelta * PresentationBase.TimelengthOfFrame) actualSpeedLR = 0.0;
                if (Math.Abs(actualSpeedFB) < SpeedFBDelta * PresentationBase.TimelengthOfFrame) actualSpeedFB = 0.0;

                double preEyeX = PlayerCam.Eye.X, preEyeZ = PlayerCam.Eye.Z;


                double X = (float)((-Math.Sin(angleLR) * actualSpeedFB + Math.Cos(angleLR) * actualSpeedLR) * PresentationBase.TimelengthOfFrame);
                double Z = (float)((Math.Cos(angleLR) * actualSpeedFB + Math.Sin(angleLR) * actualSpeedLR) * PresentationBase.TimelengthOfFrame);

                if (X > actualSpeedX) actualSpeedX += (X - actualSpeedX) * Inertia * PresentationBase.TimelengthOfFrame;
                else if (X < actualSpeedX) actualSpeedX += (X - actualSpeedX) * Inertia * PresentationBase.TimelengthOfFrame;

                if (Z > actualSpeedZ) actualSpeedZ += (Z - actualSpeedZ) * Inertia * PresentationBase.TimelengthOfFrame;
                else if (Z < actualSpeedZ) actualSpeedZ += (Z - actualSpeedZ) * Inertia * PresentationBase.TimelengthOfFrame;

                if (Math.Abs(actualSpeedX) < 0.0001) actualSpeedX = 0.0;
                if (Math.Abs(actualSpeedZ) < 0.0001) actualSpeedZ = 0.0;


                Vector3 speedVector = new Vector3((float)actualSpeedX, 0, (float)actualSpeedZ);

                MapJudgment2_5DResult result;

                if (Map != null)
                {
                    MapJudgment2_5DInput inputData = new MapJudgment2_5DInput()
                    {
                        speedVector = speedVector,
                        currentPosition = PlayerCam.Eye
                    };

                    MapJudgment2_5D.Map = Map;
                    result = MapJudgment2_5D.Judge(inputData);
                }
                else
                {
                    result = new MapJudgment2_5DResult()
                    {
                        mapOK = true,
                        result = PlayerCam.Eye + speedVector
                    };
                }

                if (result.mapOK)
                {
                    heightOffset = result.floorHeight;
                    targetHeight = result.floorHeight + data.height;
                }
                else
                {
                    targetHeight = heightOffset + data.height;
                }

                double deltaEyeY = (targetHeight - PlayerCam.Eye.Y) * 10.0;
                if (Math.Abs(deltaEyeY) < 0.0001) deltaEyeY = 0.0;

                
                PlayerCam.Eye = new Vector3(
                    result.result.X,
                    (float)(PlayerCam.Eye.Y + deltaEyeY * PresentationBase.TimelengthOfFrame),
                    result.result.Z);

                PlayerCam.Target = new Vector3()
                {
                    X = (float)(-Math.Sin(angleLR) * Math.Cos(angleUD) + PlayerCam.Eye.X),
                    Z = (float)(Math.Cos(angleLR) * Math.Cos(angleUD) + PlayerCam.Eye.Z),
                    Y = (float)(-Math.Sin(angleUD) + PlayerCam.Eye.Y)
                };
            }
        }

        public override void Draw(RenderingContext context)
        {
            
        }

        protected override void OnDispose()
        {
            
        }
    }
}
