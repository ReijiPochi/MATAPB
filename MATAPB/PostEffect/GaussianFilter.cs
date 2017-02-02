using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects;
using SharpDX.Direct3D11;
using SharpDX;

namespace MATAPB.PostEffect
{
    public class GaussianFilter : PostEffect
    {
        public GaussianFilter(int w, int h)
        {
            tempTrg = new RenderingCanvas(w, h, 1);

            Width = w;
            Height = h;

            SetWeight(200.0);

            CurrentEffect = TagCollection.Compile(LoadShaderText("GaussianFilter.fx"));
            InitInputLayout();

            src = CurrentEffect.GetVariableByName("src").AsShaderResource();
            weights = CurrentEffect.GetVariableByName("weights").AsScalar();
            samplePos = CurrentEffect.GetVariableByName("samplePos").AsVector();
            offsetX = CurrentEffect.GetVariableByName("offsetX").AsVector();
            offsetY = CurrentEffect.GetVariableByName("offsetY").AsVector();

            weights.Set(weightsValue);
        }

        public double Width { get; }
        public double Height { get; }

        public int Detail { get; set; } = 2;

        RenderingCanvas tempTrg;

        EffectShaderResourceVariable src;
        EffectScalarVariable weights;
        EffectVectorVariable samplePos;
        EffectVectorVariable offsetX;
        EffectVectorVariable offsetY;

        float[] weightsValue = new float[16];
        Vector2[] samplePosValues = new Vector2[16];

        protected override void DoApply(ShaderResourceView source, RenderTargetView target, int w, int h)
        {
            SetSamplePos(Detail, Width, Height);

            double offset = Detail * 16;

            src.SetResource(source);
            samplePos.Set(samplePosValues);
            offsetX.Set(new Vector2((float)(offset / Width), 0.0f));
            offsetY.Set(new Vector2(0.0f, (float)(offset / Height)));


            RenderTargetView[] views = null;
            if (target == null)
                views = PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.GetRenderTargets(8);

            base.DoApply(source, tempTrg.colorTarget, w, h);

            PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.ResetTargets();

            if (target != null)
                PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.SetTargets(target);
            else
                PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.SetTargets(views);

            src.SetResource(tempTrg.colorResource);

            Render(1);
        }

        private void SetWeight(double dispertion)
        {
            float total = 0.0f;

            for (int i = 0; i < 16; i++)
            {
                double pos = 1.0 + 2.0 * i;
                weightsValue[i] = (float)Math.Exp(-0.5 * pos * pos / dispertion);
                total += 2.0f * weightsValue[i];
            }

            for (int i = 0; i < 16; i++)
            {
                weightsValue[i] /= total;
            }
        }

        private void SetSamplePos(int skip, double w, double h)
        {
            int pixelOffset = -1;

            for(int i = 0; i < 16; i++)
            {
                samplePosValues[i] = new Vector2((float)(pixelOffset / w), (float)(pixelOffset / h));
                pixelOffset -= skip;
            }
        }

        protected override void OnDispose()
        {
            if (src != null) src.Dispose();
        }
    }
}
