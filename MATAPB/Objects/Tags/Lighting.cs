using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D11;

namespace MATAPB.Objects.Tags
{
    public enum LightingMode
    {
        Default,
        SoftLambert,
        Lambert
    }

    public class Lighting : Tag
    {
        //private SlimDX.Direct3D11.Buffer lightingConstantBuffer;

        //private double _LambertConstant;
        //public double LambertConstant
        //{
        //    get { return _LambertConstant; }
        //}

        //private struct LightingConstantBuffer
        //{
        //    MatVector2Float Lighting_lambertConstant;
        //    MatVector2Float Lighting_reserved;

        //    public static int SizeInBytes
        //    {
        //        get { return System.Runtime.InteropServices.Marshal.SizeOf(typeof(LightingConstantBuffer)); }
        //    }
        //}

        public LightingMode Mode { get; set; } = LightingMode.SoftLambert;

        public override void Download(RenderingContext context)
        {
            
        }

        public override string GetShaderText()
        {
            Dictionary<string, string> data = Disassemble(LoadShaderText("Lighting.fx"));

            switch (Mode)
            {
                case LightingMode.SoftLambert:
                    return data["SOFT_LAMBERT"];

                case LightingMode.Lambert:
                    return data["LAMBERT"];

                default:
                    return null;
            }
        }

        public override void SetVariables(Effect effect)
        {
            //lightingConstantBuffer = new SlimDX.Direct3D11.Buffer(
            //    PresentationArea.GraphicsDevice,
            //    new BufferDescription
            //    {
            //        SizeInBytes = LightingConstantBuffer.SizeInBytes,
            //        BindFlags = BindFlags.ConstantBuffer
            //    }
            //    );
        }

        protected override void OnDispose()
        {
            
        }
    }
}
