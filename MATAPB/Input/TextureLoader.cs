using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using SharpDX.Direct3D11;
using SharpDX;
using SharpDX.DXGI;

namespace MATAPB.Input
{
    internal class TextureLoader
    {
        public static ShaderResourceView GenTexture(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();

            WriteableBitmap data = new WriteableBitmap(image);

            data.Lock();
            DataBox dataBox = new DataBox(data.BackBuffer, data.PixelWidth * 4, data.PixelHeight);
            Texture2DDescription desc = new Texture2DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Height = data.PixelHeight,
                Width = data.PixelWidth,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };

            Texture2D tex = new Texture2D(PresentationBase.GraphicsDevice, desc, new[] { dataBox });
            data.Unlock();

            return new ShaderResourceView(PresentationBase.GraphicsDevice, tex);
        }
    }
}
