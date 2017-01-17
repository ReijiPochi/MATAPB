using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using MATAPB.Objects.Tags;

namespace MATAPB.Objects
{
    public class Picture : Primitive.Plane
    {
        public Picture(double w, double h, Orientations orientation = Orientations.plusZ)
        {
            Orientation = orientation;
            UpdateBuffer(w, h, orientation);
        }

        public Picture(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();

            Orientation = Orientations.plusZ;
            UpdateBuffer(
                image.PixelWidth / 1000.0,
                image.PixelHeight / 1000.0,
                Orientation);

            ColorTextureTag = new ColorTexture(path);
            Tags.AddTag(ColorTextureTag);
        }

        private ColorTexture _ColorTextureTag;
        public ColorTexture ColorTextureTag
        {
            get { return _ColorTextureTag; }
            set
            {
                _ColorTextureTag = value;
            }
        }
    }
}
