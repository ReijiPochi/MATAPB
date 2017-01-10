using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using MATAPB.Objects.Tags;

namespace MATAPB.Objects
{
    public class HopupImage : RenderableObject
    {
        public HopupImage(string imagePath, Orientations orientation) : base()
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            image.EndInit();

            Scale = 0.001;
            hopup.UpdateBuffer(image.PixelWidth, image.PixelHeight, orientation);

            hopup.Tags.AddTag(new ColorTexture(imagePath));
        }

        public Primitive.Plane hopup = new Primitive.Plane() { Visible = false };

        private double _Scale;
        public double Scale
        {
            get { return _Scale; }
            set
            {
                _Scale = value;
            }
        }

        public MatVector3 Position { get; set; } = new MatVector3();

        public MatVector3 Rotation { get; set; } = new MatVector3();

        private bool hop;
        private double moveY = 0.0, scale = 0.0;

        public override void Draw(RenderingContext context)
        {
            if (hop)
            {
                hopup.PSRTag.Position.X = Position.X;
                hopup.PSRTag.Position.Y = Position.Y + moveY;
                hopup.PSRTag.Position.Z = Position.Z;

                hopup.PSRTag.Scale.X = Scale * scale;
                hopup.PSRTag.Scale.Y = Scale * scale;
                hopup.PSRTag.Scale.Z = Scale * scale;

                hopup.PSRTag.Rotation = Rotation;

                moveY += 0.1;
                scale += 0.1;

                if (Math.Abs(moveY) < 0.01)
                {
                    moveY = 0.0;
                    hop = false;
                }
            }

            hopup.Draw(context);
        }

        public void Hop()
        {
            hop = true;
            hopup.Visible = true;
            moveY = -1.0;
            scale = 0.0;
        }

        public void Remove()
        {
            hopup.Visible = false;
        }

        protected override void OnDispose()
        {
            
        }
    }
}
