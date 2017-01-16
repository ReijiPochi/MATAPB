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
    public class LookCamera : Tag
    {
        private EffectMatrixVariable LookCamera_matrix;

        public Vector3 Position { get; set; } = new Vector3(0.0f);
        public Vector3 Scale { get; set; } = new Vector3(1.0f);

        public double DistanceScalingFactor { get; set; } = 0.3;
        public double DistanceScalingMaximum { get; set; } = 0.7;

        public override void Download(RenderingContext context)
        {
            Matrix c = context.cam.CameraMatrix;

            SharpDX.Vector3 pos = new SharpDX.Vector3(Position.X, Position.Y, Position.Z);
            SharpDX.Matrix cam = new SharpDX.Matrix(
                c.M11, c.M12, c.M13, c.M14,
                c.M21, c.M22, c.M23, c.M24,
                c.M31, c.M32, c.M33, c.M34,
                c.M41, c.M42, c.M43, c.M44);

            SharpDX.Vector3 scr = SharpDX.Vector3.TransformCoordinate(pos, cam);

            double dist = Math.Log10(Vector3.Distance(Position, context.cam.Eye)) * DistanceScalingFactor;
            if (dist > DistanceScalingMaximum)
                dist = DistanceScalingMaximum;
            dist = 1.0 - dist;

            double aspect = context.viewArea.X / context.viewArea.Y;
            SharpDX.Matrix trans = SharpDX.Matrix.Scaling(new SharpDX.Vector3((float)((Scale.X / aspect) * dist), (float)(Scale.Y * dist), (float)(Scale.Z * dist)));
            trans *= SharpDX.Matrix.Translation(scr);
            LookCamera_matrix.SetMatrix(trans);
        }

        public override string GetShaderText()
        {
            return LoadShaderText("LookCamera.fx");
        }

        public override void SetVariables(Effect effect)
        {
            LookCamera_matrix = effect.GetVariableByName("LookCamera_matrix").AsMatrix();
        }

        protected override void OnDispose()
        {
            
        }
    }
}
