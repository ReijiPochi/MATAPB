using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = System.Numerics.Vector2;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.Direct2D1;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

using MATAPB.Objects;
using MATAPB.Gaming;
using MATAPB.Objects.Tags;
using MATAPB.PostEffect;

namespace MATAPB
{
    public delegate void PreviewRenderEventHandler();

    public class PresentationBase
    {
        public PresentationBase()
        {
            throw new Exception("PresentationAreaはインスタンス化できません。");
        }

        public static void Initialize(double fps)
        {
            AutoDisposeObject.AllDisposing += AutoDisposeObject_AllDisposing;
            AnimationClock = new Clock(1000.0 / fps);
            FPS = fps;
            AnimationClock.Tick += AnimationClock_Tick;

            Overlay = new Window()
            {
                WindowStyle = WindowStyle.None,
                WindowState = System.Windows.WindowState.Maximized,
                AllowsTransparency = true,
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(1, 0, 0, 0)),
                ShowInTaskbar = false
            };
            Overlay.Closed += Overlay_Closed;
            Overlay.Show();

            ViewArea = new Border();

            WorldHost = new Window()
            {
                WindowStyle = WindowStyle.None,
                WindowState = System.Windows.WindowState.Maximized,
                Content = ViewArea,
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(10, 10, 10))
            };
            WorldHost.Activated += WorldHost_Activated;
            WorldHost.Deactivated += WorldHost_Deactivated;
            WorldHost.Closing += WorldHost_Closing;

            WorldHost.Show();

            PresentationSource s = PresentationSource.FromVisual(WorldHost);
            ScreenZoom = s.CompositionTarget.TransformToDevice.M11;

            CreateDeviceAndSwapChain(out _GraphicsDevice, out _SwapChain);

            RasterizerState state = new RasterizerState(
                GraphicsDevice,
                new RasterizerStateDescription()
                {
                    CullMode = CullMode.None,
                    FillMode = SharpDX.Direct3D11.FillMode.Solid
                });

            using (state)
            {
                GraphicsDevice.ImmediateContext.Rasterizer.State = state;
            }

            InitDefaultRenderTarget();
            DefaultCanvas.SetCanvas();

            InitD2d();

            Blend.SetUsual();

            using (Texture2D backBuffer = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(SwapChain, 0))
            {
                BackBuffer = new RenderTargetView(GraphicsDevice, backBuffer);
            }

            BackGround = new Objects.Primitive.Plane(2, 2, Orientations.plusZ);
            BackGround.Tags.ClearAndSet(new ColorTexture() { Texture = DefaultCanvas.renderView });
        }

        private static double _FPS = 60.0;
        public static double FPS
        {
            get { return _FPS; }
            set
            {
                _FPS = value;
                _TimelengthOfFrame = 1.0 / value;
            }
        }

        private static double _TimelengthOfFrame = 1.0 / 60.0;
        public static double TimelengthOfFrame
        {
            get { return _TimelengthOfFrame; }
        }

        public static World World { get; set; }

        public static Window WorldHost { get; private set; }

        public static Window Overlay { get; private set; }

        public static Border ViewArea { get; set; }

        public static double ScreenZoom { get; private set; }

        private static SharpDX.Direct3D11.Device _GraphicsDevice;
        public static SharpDX.Direct3D11.Device GraphicsDevice
        {
            get { return _GraphicsDevice; }
        }

        public static SharpDX.Direct2D1.Factory D2dFactory { get; set; }

        public static SharpDX.Direct2D1.Device D2dDevice { get; set; }

        public static SharpDX.DirectWrite.Factory DwFactory { get; set; }

        public static SharpDX.WIC.ImagingFactory ImagingFactory { get; set; }

        private static SwapChain _SwapChain;
        public static SwapChain SwapChain
        {
            get { return _SwapChain; }
        }

        public static RenderingCanvas DefaultCanvas { get; private set; }

        public static RenderTargetView BackBuffer { get; private set; }

        public static MATAPB.Objects.Primitive.Plane BackGround { get; private set; }

        public static Clock AnimationClock { get; private set; }

        public static event PreviewRenderEventHandler PreviewRender;

        public static void Launch()
        {
            AnimationClock.Start();
        }

        public static void LockObjects()
        {
            AnimationObject.Lock();
        }

        public static void UnlockObjects()
        {
            AnimationObject.Unlock();
        }

        private static void AnimationClock_Tick(long time)
        {
            PreviewRender?.Invoke();
            AnimationObject.DoAnimation();
            ActionObject.DoAction();
            Render();
        }

        public static void Render()
        {
            if (World != null)
            {
                RenderingContext context = new RenderingContext()
                {
                    viewArea = new Vector2((float)ViewArea.ActualWidth, (float)ViewArea.ActualHeight),
                    canvas = DefaultCanvas
                };

                DefaultCanvas.ClearCanvas();

                World.Render(context);

                SwapChain.Present(0, PresentFlags.None);
            }
        }

        private static void WorldHost_Deactivated(object sender, EventArgs e)
        {
            
        }

        private static void WorldHost_Activated(object sender, EventArgs e)
        {
            if (Overlay != null)
                Overlay.Activate();
        }

        private static void CreateDeviceAndSwapChain(out SharpDX.Direct3D11.Device device, out SwapChain swapChain)
        {
            HwndSource source = (HwndSource)HwndSource.FromVisual(WorldHost);

            SharpDX.Direct3D11.Device.CreateWithSwapChain(
                DriverType.Hardware,
                DeviceCreationFlags.BgraSupport,
                new SwapChainDescription
                {
                    BufferCount = 1,
                    OutputHandle = source.Handle,
                    IsWindowed = false,
                    SampleDescription = new SampleDescription
                    {
                        Count = 1,
                        Quality = 0
                    },
                    ModeDescription = new ModeDescription
                    {
                        Width = (int)(ViewArea.ActualWidth),
                        Height = (int)(ViewArea.ActualHeight),
                        RefreshRate = new Rational(60, 1),
                        Format = Format.R8G8B8A8_UNorm,
                    },
                    Usage = Usage.RenderTargetOutput
                },
                out device,
                out swapChain);
        }

        private static void InitDefaultRenderTarget()
        {
            Texture tex = new Texture((int)(ViewArea.ActualWidth), (int)(ViewArea.ActualHeight), 1);
            DefaultCanvas = new RenderingCanvas(tex);
        }

        private static void InitD2d()
        {
            D2dFactory = new SharpDX.Direct2D1.Factory();
            DwFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);
        }

        private static void Overlay_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private static void WorldHost_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimationClock.Dispose();
            Overlay.Close();
        }

        private static void AutoDisposeObject_AllDisposing()
        {
            if (GraphicsDevice != null) GraphicsDevice.Dispose();
            if (SwapChain != null) SwapChain.Dispose();
            if (DefaultCanvas != null) DefaultCanvas.Dispose();
        }
    }
}
