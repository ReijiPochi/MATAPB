using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SharpDX;

namespace MATAPB.Objects.Tags
{
    public class LookCamera : Tag
    {
        private EffectMatrixVariable LookCamera_matrix;

        public MatVector3 Position { get; set; } = new MatVector3(0.0);
        public MatVector3 Scale { get; set; } = new MatVector3(1.0);

        public double DistanceScalingFactor { get; set; } = 0.15;
        public double DistanceScalingMaximum { get; set; } = 0.5;

        public override void Download(RenderingContext context)
        {
            Vector3 pos = MatVector3.ToSlimDXVector3(Position);
            Vector3 screenCoord = Vector3.TransformCoordinate(pos, context.cam.CameraMatrix);

            double dist = Math.Log10(MatVector3.Distance(Position, context.cam.Eye)) * DistanceScalingFactor;
            if (dist > DistanceScalingMaximum)
                dist = DistanceScalingMaximum;
            dist = 1.0 - dist;

            double aspect = context.viewArea.Y / context.viewArea.X;
            Matrix trans = Matrix.Scaling(new Vector3((float)(Scale.X * dist), (float)((Scale.Y / aspect) * dist), (float)(Scale.Z * dist)));
            trans *= Matrix.Translation(screenCoord);
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
