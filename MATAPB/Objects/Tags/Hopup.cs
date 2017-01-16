using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using MATAPB.Vector3Extentions;
using Matrix = System.Numerics.Matrix4x4;

using SharpDX;
using SharpDX.Direct3D11;

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

        public Vector3 MinPosition { get; set; } = new Vector3(0.0f);
        public Vector3 MinScale { get; set; } = new Vector3(0.0f);
        public Vector3 MinRotation { get; set; } = new Vector3(0.0f);

        public Vector3 MaxPosition { get; set; } = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 MaxScale { get; set; } = new Vector3(1.0f);
        public Vector3 MaxRotation { get; set; } = new Vector3(0.0f);

        private Vector3 Position { get; set; } = new Vector3(0.0f);
        private Vector3 Scale { get; set; } = new Vector3(0.0f);
        private Vector3 Rotation { get; set; } = new Vector3(0.0f);

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

        protected override void AnimationOneFrame()
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
        }

        public override void Download(RenderingContext context)
        {
            Hopup_world.SetMatrix(_Hopup_world);
        }

        private void OnHop()
        {
            switch (HopupAnimation)
            {
                case HopupAnimations.None:
                    Position = MaxPosition;
                    Scale = MaxScale;
                    Rotation = MaxRotation;

                    state = HopState.Hover;
                    hoverState = 0.0;
                    break;

                case HopupAnimations.PopLiner:
                    hopupState += PresentationArea.TimelengthOfFrame / HopupTime;
                    if (hopupState < 1.0)
                    {
                        Position = MinPosition.InternalDivision(MaxPosition, hopupState, 1.0 - hopupState);
                        Scale = MinScale.InternalDivision(MaxScale, hopupState, 1.0 - hopupState);
                        Rotation = MinRotation.InternalDivision(MaxRotation, hopupState, 1.0 - hopupState);
                    }
                    else
                    {
                        Position = MaxPosition;
                        Scale = MaxScale;
                        Rotation = MaxRotation;

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
                    Position = new Vector3(MaxPosition.X, (float)(MaxPosition.Y + WaveHeight * Math.Sin(hoverState)), MaxPosition.Z);
                    break;
            }

            CalcValue();
        }

        private void OnClose()
        {
            switch(CloseAnimation)
            {
                case CloseAnimations.None:
                    Position = MinPosition;
                    Scale = MinScale;
                    Rotation = MinRotation;
                    break;

                case CloseAnimations.DepopLiner:
                    closeStaate -= PresentationArea.TimelengthOfFrame / CloseTime;
                    if (closeStaate > 0.0)
                    {
                        Position = MinPosition.InternalDivision(MaxPosition, closeStaate, 1.0 - closeStaate);
                        Scale = MinScale.InternalDivision(MaxScale, closeStaate, 1.0 - closeStaate);
                        Rotation = MinRotation.InternalDivision(MaxRotation, closeStaate, 1.0 - closeStaate);
                    }
                    else
                    {
                        Position = MinPosition;
                        Scale = MinScale;
                        Rotation = MinRotation;

                        state = HopState.None;
                    }
                    break;

                default:
                    break;
            }

            CalcValue();
        }

        private void CalcValue()
        {
            _Hopup_world = Matrix.CreateScale(Scale);
            _Hopup_world *= Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z);
            _Hopup_world *= Matrix.CreateTranslation(Position);
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
