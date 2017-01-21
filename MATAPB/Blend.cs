using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D11;

namespace MATAPB
{
    public class Blend
    {
        public static BlendStateDescription usual;
        public static BlendStateDescription add;

        private static bool disposeInit, usualInit, addInit;

        static BlendState usualState;
        static BlendState addState;

        public static void SetUsual()
        {
            SetDispose();

            if (!usualInit)
            {
                usual = new BlendStateDescription()
                {
                    AlphaToCoverageEnable = false,
                    IndependentBlendEnable = false
                };
                usual.RenderTarget[0].IsBlendEnabled = true;
                usual.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
                usual.RenderTarget[0].BlendOperation = BlendOperation.Add;
                usual.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
                usual.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
                usual.RenderTarget[0].DestinationAlphaBlend = BlendOption.DestinationAlpha;
                usual.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
                usual.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

                usualState = new BlendState(PresentationBase.GraphicsDevice, usual);

                usualInit = true;
            }

            PresentationBase.GraphicsDevice.ImmediateContext.OutputMerger.BlendState = usualState;
        }

        //public static void SetAdd()
        //{
        //    SetDispose();

        //    if (!addInit)
        //    {
        //        add = new BlendStateDescription();

        //        add.AlphaToCoverageEnable = false;
        //        add.IndependentBlendEnable = false;
        //        add.RenderTargets[0].BlendEnable = true;
        //        add.RenderTargets[0].SourceBlend = BlendOption.SourceAlpha;
        //        add.RenderTargets[0].BlendOperation = BlendOperation.Add;
        //        add.RenderTargets[0].DestinationBlend = BlendOption.DestinationAlpha;
        //        add.RenderTargets[0].SourceBlendAlpha = BlendOption.One;
        //        add.RenderTargets[0].DestinationBlendAlpha = BlendOption.Zero;
        //        add.RenderTargets[0].BlendOperationAlpha = BlendOperation.Add;
        //        add.RenderTargets[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

        //        addState = BlendState.FromDescription(
        //            PresentationArea.GraphicsDevice,
        //            add);

        //        addInit = true;
        //    }

        //    PresentationArea.GraphicsDevice.ImmediateContext.OutputMerger.BlendState = addState;
        //}

        private static void SetDispose()
        {
            if (!disposeInit)
            {
                AutoDisposeObject.AllDisposing += AutoDisposeObject_AllDisposing;
                disposeInit = true;
            }
        }

        private static void AutoDisposeObject_AllDisposing()
        {
            if (usualState != null) usualState.Dispose();
            if (addState != null) addState.Dispose();
        }
    }
}
