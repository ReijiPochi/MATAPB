using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX;
using SharpDX.DirectWrite;

namespace MATAPB._2D
{
    public class TextTexture : Texture
    {
        public TextTexture(int w, int h) : base(w, h)
        {
            using (Surface surface = Tex.QueryInterface<Surface>())
            {
                renderTarget2D = new RenderTarget(
                    PresentationArea.D2dFactory,
                    surface,
                    new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied))
                    );
            }

            renderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
            renderTarget2D.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Grayscale;

            brush = new SolidColorBrush(renderTarget2D, Color.Black, new BrushProperties() { Opacity = 1.0f });

            format = new TextFormat(PresentationArea.DwFactory, _Font, _FontSize)
            {
                TextAlignment = TextAlignment.Center,
                ParagraphAlignment = ParagraphAlignment.Center
            };
        }

        private string _Font = "メイリオ";
        public string Font
        {
            get { return _Font; }
            set
            {
                _Font = value;

                if (format != null) format.Dispose();

                format = new TextFormat(PresentationArea.DwFactory, Font, _FontSize)
                {
                    TextAlignment = TextAlignment.Center,
                    ParagraphAlignment = ParagraphAlignment.Center
                };
            }
        }

        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                valueChanged = true;
                _Text = value;
            }
        }

        private float _FontSize = 50.0f;
        public double FontSize
        {
            get { return _FontSize; }
            set
            {
                _FontSize = (float)value;

                if (format != null) format.Dispose();

                format = new TextFormat(PresentationArea.DwFactory, Font, _FontSize)
                {
                    TextAlignment = TextAlignment.Center,
                    ParagraphAlignment = ParagraphAlignment.Center
                };
            }
        }

        private bool valueChanged;

        public RenderTarget renderTarget2D;
        public SolidColorBrush brush;
        public TextFormat format;
        public TextLayout layout;

        public void Draw()
        {
            if (Text == null)
                return;

            if (valueChanged)
            {
                layout = new TextLayout(PresentationArea.DwFactory, Text, format, Description.Width, Description.Height);
                valueChanged = false;
            }

            renderTarget2D.BeginDraw();

            renderTarget2D.Clear(new SharpDX.Mathematics.Interop.RawColor4(0.0f, 0.0f, 0.0f, 0.0f));
            renderTarget2D.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2(0, 0), layout, brush, DrawTextOptions.None);

            renderTarget2D.EndDraw();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            if (renderTarget2D != null) renderTarget2D.Dispose();
            if (brush != null) brush.Dispose();
            if (format != null) format.Dispose();
            if (layout != null) layout.Dispose();
        }
    }
}
