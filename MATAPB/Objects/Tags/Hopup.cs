using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;
using SlimDX.Direct3D11;

namespace MATAPB.Objects.Tags
{
    public enum HopupAnimations
    {
        None,
        PopLiner
    }

    public enum HoverAnimations
    {
        None,
        Wave
    }

    public enum CloseAnimations
    {
        None,
        DepopLiner
    }

    public class Hopup : Tag
    {
        public HopupAnimations HopupAnimation { get; set; }
        public HoverAnimations HoverAnimation { get; set; }
        public CloseAnimations CloseAnimation { get; set; }

        public double HopupTime { get; set; } = 1.0;
        public double CloseTime { get; set; } = 1.0;

        public double WaveHeight { get; set; } = 0.1;
        public double WaveRate { get; set; } = 1.0;

        public MatVector3 MinPosition { get; set; } = new MatVector3(0.0);
        public MatVector3 MinScale { get; set; } = new MatVector3(0.0);
        public MatVector3 MinRotation { get; set; } = new MatVector3(0.0);

        public MatVector3 MaxPosition { get; set; } = new MatVector3(0.0, 1.0, 0.0);
        public MatVector3 MaxScale { get; set; } = new MatVector3(1.0);
        public MatVector3 MaxRotation { get; set; } = new MatVector3(0.0);

        private MatVector3 Position { get; set; } = new MatVector3(0.0);
        private MatVector3 Scale { get; set; } = new MatVector3(0.0);
        private MatVector3 Rotation { get; set; } = new MatVector3(0.0);

        private Matrix _Hopup_world;
        private EffectMatrixVariable Hopup_world;

        private double hopupState = 0.0, closeStaate = 0.0, hoverState = 0.0;

        private enum HopState
        {
            None,
            Hop,
            Hover,
            Close
        }

        private HopState state = HopState.None;

        public void Hop()
        {
            state = HopState.Hop;
            hopupState = 0.0;
        }

        public void Close()
        {
            state = HopState.Close;
            closeStaate = 1.0;
        }

        public override void Download(RenderingContext context)
        {
            switch (state)
            {
                case HopState.Hop:
                    OnHop();
                    break;

                case HopState.Hover:
                    OnHover();
                    break;

                case HopState.Close:
                    OnClose();
                    break;

                default:
                    break;
            }

            Hopup_world.SetMatrix(_Hopup_world);
        }

        private void OnHop()
        {
            switch (HopupAnimation)
            {
                case HopupAnimations.None:
                    MatVector3.Copy(MaxPosition, Position);
                    MatVector3.Copy(MaxScale, Scale);
                    MatVector3.Copy(MaxRotation, Rotation);

                    state = HopState.Hover;
                    hoverState = 0.0;
                    break;

                case HopupAnimations.PopLiner:
                    hopupState += PresentationArea.TimelengthOfFrame / HopupTime;
                    if (hopupState < 1.0)
                    {
                        Position = MatVector3.InternalDivision(MinPosition, MaxPosition, hopupState, 1.0 - hopupState);
                        Scale = MatVector3.InternalDivision(MinScale, MaxScale, hopupState, 1.0 - hopupState);
                        Rotation = MatVector3.InternalDivision(MinRotation, MaxRotation, hopupState, 1.0 - hopupState);
                    }
                    else
                    {
                        MatVector3.Copy(MaxPosition, Position);
                        MatVector3.Copy(MaxScale, Scale);
                        MatVector3.Copy(MaxRotation, Rotation);

                        state = HopState.Hover;
                        hoverState = 0.0;
                    }
                    break;

                default:
                    break;
            }

            CalcValue();
        }

        private void OnHover()
        {
            switch(HoverAnimation)
            {
                case HoverAnimations.None:
                    return;

                case HoverAnimations.Wave:
                    hoverState += PresentationArea.TimelengthOfFrame * 2.0 * Math.PI * WaveRate;
                    Position.Y = MaxPosition.Y + WaveHeight * Math.Sin(hoverState);
                    break;
            }

            CalcValue();
        }

        private void OnClose()
        {
            switch(CloseAnimation)
            {
                case CloseAnimations.None:
                    MatVector3.Copy(MinPosition, Position);
                    MatVector3.Copy(MinScale, Scale);
                    MatVector3.Copy(MinRotation, Rotation);
                    break;

                case CloseAnimations.DepopLiner:
                    closeStaate -= PresentationArea.TimelengthOfFrame / CloseTime;
                    if (closeStaate > 0.0)
                    {
                        Position = MatVector3.InternalDivision(MinPosition, MaxPosition, closeStaate, 1.0 - closeStaate);
                        Scale = MatVector3.InternalDivision(MinScale, MaxScale, closeStaate, 1.0 - closeStaate);
                        Rotation = MatVector3.InternalDivision(MinRotation, MaxRotation, closeStaate, 1.0 - closeStaate);
                    }
                    else
                    {
                        MatVector3.Copy(MinPosition, Position);
                        MatVector3.Copy(MinScale, Scale);
                        MatVector3.Copy(MinRotation, Rotation);

                        state = HopState.None;
                    }
                    CalcValue();
                    break;

                default:
                    break;
            }

            CalcValue();
        }

        private void CalcValue()
        {
            _Hopup_world = Matrix.Scaling(MatVector3.ToSlimDXVector3(Scale));
            _Hopup_world *= Matrix.RotationX((float)Rotation.X) * Matrix.RotationY((float)Rotation.Y) * Matrix.RotationZ((float)Rotation.Z);
            _Hopup_world *= Matrix.Translation(MatVector3.ToSlimDXVector3(Position));
        }

        public override string GetShaderText()
        {
            return LoadShaderText("Hopup.fx");
        }

        public override void SetVariables(Effect effect)
        {
            Hopup_world = effect.GetVariableByName("Hopup_world").AsMatrix();
        }

        protected override void OnDispose()
        {
            
        }
    }
}
