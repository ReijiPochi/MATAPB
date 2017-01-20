using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using Matrix = System.Numerics.Matrix4x4;

namespace MATAPB.Objects.Tags
{
    public class Camera : Tag
    {
        public bool UseCustomCamera { get; set; }
        public MATAPB.Camera CustomCamera { get; set; }

        private EffectMatrixVariable Camera_matrix;

        public override void Download(RenderingContext context)
        {
            if (Camera_matrix != null)
            {
                if (UseCustomCamera)
                {
                    Camera_matrix.SetMatrix(CustomCamera.CameraMatrix);
                }
                else if(context.cam != null)
                {
                    Camera_matrix.SetMatrix(context.cam.CameraMatrix);
                }
            }
        }

        public override string GetShaderText()
        {
            return LoadShaderText("Camera.fx");
        }

        public override void SetVariables(Effect effect)
        {
            Camera_matrix = effect.GetVariableByName("Camera_matrix").AsMatrix();
        }

        protected override void OnDispose()
        {
            
        }
    }
}
