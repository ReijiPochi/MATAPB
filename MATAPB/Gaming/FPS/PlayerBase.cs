using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using MATAPB.Objects;

namespace MATAPB.Gaming.FPS
{
    public class PlayerBase
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

        double actualSpeedLR, actualSpeedFB, actualSpeedUD;
        double angleLR = 0.0, angleUD = 0.0;
        double heightOffset = 0.0, targetHeight = 0.0;

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

                if (data.speedLR > actualSpeedLR) actualSpeedLR += 0.4;
                else if (data.speedLR < actualSpeedLR) actualSpeedLR -= 0.4;

                if (data.speedFB > actualSpeedFB) actualSpeedFB += 0.4;
                else if (data.speedFB < actualSpeedFB) actualSpeedFB -= 0.4;

                if (fov > PlayerCam.FieldOfView) PlayerCam.FieldOfView += 5.0;
                else if (fov < PlayerCam.FieldOfView) PlayerCam.FieldOfView -= 5.0;


                if (Math.Abs(actualSpeedLR) < 0.4) actualSpeedLR = 0.0;
                if (Math.Abs(actualSpeedFB) < 0.4) actualSpeedFB = 0.0;
                if (Math.Abs(actualSpeedUD) < 0.01) actualSpeedUD = 0.0;

                double preEyeX = PlayerCam.Eye.X, preEyeZ = PlayerCam.Eye.Z;


                Vector3 speedVector = new Vector3()
                {
                    X = (float)((-Math.Sin(angleLR) * actualSpeedFB + Math.Cos(angleLR) * actualSpeedLR) * PresentationArea.TimelengthOfFrame),
                    Z = (float)((Math.Cos(angleLR) * actualSpeedFB + Math.Sin(angleLR) * actualSpeedLR) * PresentationArea.TimelengthOfFrame),
                    Y = 0.0f
                };

                MapJudgment2_5DInput inputData = new MapJudgment2_5DInput()
                {
                    speedVector = speedVector,
                    currentPosition = PlayerCam.Eye
                };

                MapJudgment2_5D.Map = Map;
                MapJudgment2_5DResult result = MapJudgment2_5D.Judge(inputData);
                

                if (result.mapOK)
                {
                    heightOffset = result.floorHeight;
                    targetHeight = result.floorHeight + data.height;
                }
                else
                {
                    targetHeight = heightOffset + data.height;
                }

                if (targetHeight > 5.0) targetHeight = 5.0;
                else if (targetHeight < 0.5) targetHeight = 0.5;

                double deltaEyeY = (targetHeight - PlayerCam.Eye.Y) * 10.0;
                if (Math.Abs(deltaEyeY) < 0.01) deltaEyeY = 0.0;

                
                PlayerCam.Eye = new Vector3(
                    result.result.X,
                    (float)(PlayerCam.Eye.Y + deltaEyeY * PresentationArea.TimelengthOfFrame),
                    result.result.Z);

                PlayerCam.Target = new Vector3()
                {
                    X = (float)(-Math.Sin(angleLR) * Math.Cos(angleUD) + PlayerCam.Eye.X),
                    Z = (float)(Math.Cos(angleLR) * Math.Cos(angleUD) + PlayerCam.Eye.Z),
                    Y = (float)(-Math.Sin(angleUD) + PlayerCam.Eye.Y)
                };
            }
        }
    }
}
