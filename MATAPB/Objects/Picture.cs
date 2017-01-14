﻿using System;
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
        public Picture(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();

            Orientation = Orientations.plusZ;
            UpdateBuffer(image.PixelWidth * PresentationArea.ScreenZoom, image.PixelHeight * PresentationArea.ScreenZoom, Orientation);

            Tags.AddTag(new ColorTexture(path));
        }
    }
}