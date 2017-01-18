using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Matrix = System.Numerics.Matrix4x4;

using SharpDX.Direct3D11;
using SharpDX;

namespace MATAPB.Objects.Tags
{
    public enum PSROrder
    {
        SRP,
        SPR
    }

    public class PSR : Tag
    {
        public PSR()
        {
            Position = new Vector3(0.0f,0.0f,0.0f);
            Scale = new Vector3(1.0f);
            Rotation = new Vector3(0.0f);
        }

        public PSROrder Order { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Rotation { get; set; }

        private Vector3 prePosition = new Vector3(), preScale = new Vector3(), preRotation = new Vector3();

        private EffectMatrixVariable PSR_world;

        public override void Download(RenderingContext context)
        {
            if (PSR_world != null)
            {
                if (prePosition != Position || preScale != Scale || preRotation != Rotation)
                {
                    Matrix world = new Matrix();

                    switch (Order)
                    {
                        case PSROrder.SRP:
                            world = Matrix.CreateScale(Scale);
                            world *= Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z);
                            world *= Matrix.CreateTranslation(Position);
                            break;

                        case PSROrder.SPR:
                            world = Matrix.CreateScale(Scale);
                            world *= Matrix.CreateTranslation(Position);
                            world *= Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z);
                            break;

                        default:
                            break;
                    }

                    PSR_world.SetMatrix(world);

                    prePosition = Position;
                    preScale = Scale;
                    preRotation = Rotation;
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
