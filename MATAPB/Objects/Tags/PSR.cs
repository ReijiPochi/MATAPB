using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SharpDX;

namespace MATAPB.Objects.Tags
{
    public class PSR : Tag
    {
        public PSR()
        {
            Position = new MatVector3(0.0,0.0,0.0);
            Scale = new MatVector3(1.0);
            Rotation = new MatVector3(0.0);
        }

        public MatVector3 Position { get; set; }
        public MatVector3 Scale { get; set; }
        public MatVector3 Rotation { get; set; }

        private MatVector3 prePosition = new MatVector3(), preScale = new MatVector3(), preRotation = new MatVector3();

        private EffectMatrixVariable PSR_world;

        public override void Download(RenderingContext context)
        {
            if (PSR_world != null)
            {
                if (!MatVector3.ValueEquals(prePosition, Position) || !MatVector3.ValueEquals(preScale, Scale) || !MatVector3.ValueEquals(preRotation, Rotation))
                {
                    Matrix world = Matrix.Scaling(MatVector3.ToSlimDXVector3(Scale));
                    world *= Matrix.RotationX((float)Rotation.X) * Matrix.RotationY((float)Rotation.Y) * Matrix.RotationZ((float)Rotation.Z);
                    world *= Matrix.Translation(MatVector3.ToSlimDXVector3(Position));

                    PSR_world.SetMatrix(world);

                    MatVector3.Copy(Position, prePosition);
                    MatVector3.Copy(Scale, preScale);
                    MatVector3.Copy(Rotation, preRotation);
                }
            }
        }

        public override string GetShaderText()
        {
            return LoadShaderText("PSR.fx");
        }

        public override void SetVariables(Effect effect)
        {
            PSR_world = effect.GetVariableByName("PSR_world").AsMatrix();
        }

        protected override void OnDispose()
        {

        }
    }
}
