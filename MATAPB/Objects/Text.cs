using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects.Tags;
using MATAPB._2D;

namespace MATAPB.Objects
{
    public class Text : Primitive.Plane
    {
        public Text(int pixelWidth, int pixelHeight, MatColor color, Orientations orientation = Orientations.plusZ) : base()
        {
            UpdateBuffer(pixelWidth / 1000.0, pixelHeight / 1000.0, orientation);
            Orientation = orientation;

            TextAndFont = new TextTexture(pixelWidth, pixelHeight);

            TextureTag = new ColorTexture(TextAndFont);
            SolidColorTag = new SolidColor(SolidColorOverwriteMode.Color, color);

            Tags.AddTag(new Tag[] { TextureTag, SolidColorTag });
        }

        public string TextValue
        {
            get { return TextAndFont.Text; }
            set { TextAndFont.Text = value; }
        }

        public double FontSize
        {
            get { return TextAndFont.FontSize; }
            set { TextAndFont.FontSize = value; }
        }
        
        public TextTexture TextAndFont { get; }
        public ColorTexture TextureTag { get; }
        public SolidColor SolidColorTag { get; }

        public override void Draw(RenderingContext context)
        {
            TextAndFont.Draw();

            base.Draw(context);
        }
    }
}
