using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects;

namespace MATAPB.Gaming.FPS
{
    public class PlayerBase
    {
        public PlayerBase()
        {
            PlayerCam = new Camera()
            {
                Eye = new MatVector3(0.0, 1.0, 0.0),
                Up = new MatVector3(0.0, 1.0, 0.0)
            };
        }

        public PlayerBase(Camera cam)
        {
            PlayerCam = cam;
        }

        public Object3D Map { get; set; }

        public Camera PlayerCam { get; set; }

        double actualSpeedLR, actualSpeedFB, actualSpeedUD;
        double angleLR = 0.0, angleUD = 0.0;
        double targetHeight = 1.6;

        double fov = 70.0;

        public virtual void Move(MoveData data)
        {
            if (data != null)
            {
                angleLR += data.deltaAngleLR * PresentationArea.TimelengthOfFrame;
                angleUD += data.deltaAngleUD * PresentationArea.TimelengthOfFrame;

                if (angleUD > Math.PI * 0.45)
                    angleUD = Math.PI * 0.45;
                else if (angleUD < -Math.PI * 0.45)
                    angleUD = -Math.PI * 0.45;

                if (data.speedLR > actualSpeedLR) actualSpeedLR += 0.004;
                else if (data.speedLR < actualSpeedLR) actualSpeedLR -= 0.004;

                if (data.speedFB > actualSpeedFB) actualSpeedFB += 0.004;
                else if (data.speedFB < actualSpeedFB) actualSpeedFB -= 0.004;

                if (fov > PlayerCam.FieldOfView) PlayerCam.FieldOfView += 5.0;
                else if (fov < PlayerCam.FieldOfView) PlayerCam.FieldOfView -= 5.0;


                if (Math.Abs(actualSpeedLR) < 0.004) actualSpeedLR = 0.0;
                if (Math.Abs(actualSpeedFB) < 0.004) actualSpeedFB = 0.0;
                if (Math.Abs(actualSpeedUD) < 0.01) actualSpeedUD = 0.0;

                double preEyeX = PlayerCam.Eye.X, preEyeZ = PlayerCam.Eye.Z;


                MatVector3 speedVector = new MatVector3()
                {
                    X = -Math.Sin(angleLR) * actualSpeedFB + Math.Cos(angleLR) * actualSpeedLR,
                    Z = Math.Cos(angleLR) * actualSpeedFB + Math.Sin(angleLR) * actualSpeedLR,
                    Y = 0
                };

                MapJudgment2_5DInput inputData = new MapJudgment2_5DInput()
                {
                    speedVector = speedVector,
                    currentPosition = PlayerCam.Eye
                };

                MapJudgment2_5D.Map = Map;
                MapJudgment2_5DResult result = MapJudgment2_5D.Judge(inputData);

                PlayerCam.Eye.X += result.resultVector.X * PresentationArea.TimelengthOfFrame;
                PlayerCam.Eye.Z += result.resultVector.Z * PresentationArea.TimelengthOfFrame;

                if (result.mapOK)
                {
                    targetHeight = result.floorHeight + 1.6;
                }
                else
                {
                    targetHeight = PlayerCam.Eye.Y;
                }

                if (targetHeight > 5.0) targetHeight = 5.0;
                else if (targetHeight < 0.5) targetHeight = 0.5;

                double deltaEyeY = (targetHeight - PlayerCam.Eye.Y) * 0.12;
                if (Math.Abs(deltaEyeY) < 0.001) deltaEyeY = 0.0;

                PlayerCam.Eye.Y += deltaEyeY * PresentationArea.TimelengthOfFrame;

                PlayerCam.Target = new MatVector3()
                {
                    X = -Math.Sin(angleLR) * Math.Cos(angleUD) + PlayerCam.Eye.X,
                    Z = Math.Cos(angleLR) * Math.Cos(angleUD) + PlayerCam.Eye.Z,
                    Y = -Math.Sin(angleUD) + PlayerCam.Eye.Y
                };
            }
        }
    }
}
