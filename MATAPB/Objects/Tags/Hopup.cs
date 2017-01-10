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
        Raise,
        Pop
    }

    public enum HoverAnimations
    {
        None,
        UpDown
    }

    public class Hopup : Tag
    {
        public HopupAnimations HopupAnimation { get; set; }
        public HoverAnimations HoverAnimation { get; set; }

        public double HopupTime { get; set; }
        public double CloseTime { get; set; }

        private MatVector3 Position { get; set; }
        private MatVector3 Scale { get; set; }
        private MatVector3 Rotation { get; set; }

        private EffectMatrixVariable Hopup_world;
        private double hopupCount = 0, closeCount = 0;

        private enum HopState
        {
            None,
            Hop,
            Hover,
            Close
        }

        private HopState state;

        public void Hop()
        {
            state = HopState.Hop;
        }

        public void Close()
        {
            state = HopState.Close;
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
        }

        private void OnHop()
        {

        }

        private void OnHover()
        {

        }

        private void OnClose()
        {

        }

        private void ValueDownload()
        {
            Matrix world = Matrix.Scaling(MatVector3.ToSlimDXVector3(Scale));
            world *= Matrix.RotationX((float)Rotation.X) * Matrix.RotationY((float)Rotation.Y) * Matrix.RotationZ((float)Rotation.Z);
            world *= Matrix.Translation(MatVector3.ToSlimDXVector3(Position));

            Hopup_world.SetMatrix(world);
        }

        public override string GetShaderText()
        {
            return LoadShaderText("Hopup.fx");
        }

        public override void SetVariables(Effect effect)
        {
            Hopup_world = effect.GetVariableByName("PSR_world").AsMatrix();
        }

        protected override void OnDispose()
        {
            
        }
    }
}
