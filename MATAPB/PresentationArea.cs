using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

using MATAPB.Objects;
using MATAPB.Gaming;

namespace MATAPB
{
    public delegate void PreviewRenderEventHandler();

    public class PresentationArea
    {
        public PresentationArea()
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
                WindowState = WindowState.Maximized,
                AllowsTransparency = true,
                Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0)),
                ShowInTaskbar = false
            };
            Overlay.Closed += Overlay_Closed;
            Overlay.Show();

            ViewArea = new Border();

            WorldHost = new Window()
            {
                WindowStyle = WindowStyle.None,
                WindowState = WindowState.Maximized,
                Content = ViewArea,
                Background = new SolidColorBrush(Color.FromRgb(10, 10, 10))
            };
            WorldHost.Activated += WorldHost_Activated;
            WorldHost.Deactivated += WorldHost_Deactivated;
            WorldHost.Closing += WorldHost_Closing;

            WorldHost.Show();

            CreateDeviceAndSwapChain(out _GraphicsDevice, out _SwapChain);

            RasterizerState state = RasterizerState.FromDescription(
                GraphicsDevice,
                new RasterizerStateDescription()
                {
                    CullMode = CullMode.None,
                    FillMode = FillMode.Solid
                });

            using (state)
            {
                GraphicsDevice.ImmediateContext.Rasterizer.State = state;
            }

            DefaultCanvas = new RenderingCanvas() { color = new Color4(0.1f, 0.1f, 0.1f) };
            InitDefaultRenderTarget();
            InitDefaultDepthStencil();
            DefaultCanvas.SetCanvas();
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

        private static SlimDX.Direct3D11.Device _GraphicsDevice;
        public static SlimDX.Direct3D11.Device GraphicsDevice
        {
            get { return _GraphicsDevice; }
        }

        private static SwapChain _SwapChain;
        public static SwapChain SwapChain
        {
            get { return _SwapChain; }
        }

        public static RenderingCanvas DefaultCanvas { get; private set; }

        public static Clock AnimationClock { get; private set; }

        public static event PreviewRenderEventHandler PreviewRender;

        public static void Launch()
        {
            AnimationClock.Start();
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
            RenderingContext context = new RenderingContext()
            {
                viewArea = new MatVector2(ViewArea.ActualWidth, ViewArea.ActualHeight)
            };

            if (World != null)
            {
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

        private static void CreateDeviceAndSwapChain(out SlimDX.Direct3D11.Device device, out SwapChain swapChain)
        {
            HwndSource source = (HwndSource)HwndSource.FromVisual(WorldHost);

            SlimDX.Direct3D11.Device.CreateWithSwapChain(
                DriverType.Hardware,
                DeviceCreationFlags.None,
                new SwapChainDescription
                {
                    BufferCount = 1,
                    OutputHandle = source.Handle,
                    IsWindowed = false,
                    SampleDescription = new SampleDescription
                    {
                        Count = 4,
                        Quality = 0
                    },
                    ModeDescription = new ModeDescription
                    {
                        Width = (int)(ViewArea.ActualWidth),
                        Height = (int)(ViewArea.ActualHeight),
                        RefreshRate = new Rational(60, 1),
                        Format = Format.R8G8B8A8_UNorm
                    },
                    Usage = Usage.RenderTargetOutput
                },
                out device,
                out swapChain);
        }

        private static void InitDefaultRenderTarget()
        {
            using (Texture2D backBuffer = SlimDX.Direct3D11.Resource.FromSwapChain<Texture2D>(SwapChain, 0))
            {
                DefaultCanvas.renderTarget = new RenderTargetView(GraphicsDevice, backBuffer);
            }
        }

        private static void InitDefaultDepthStencil()
        {
            Texture2DDescription depthBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                Format = Format.D32_Float,
                Width = (int)(ViewArea.ActualWidth),
                Height = (int)(ViewArea.ActualHeight),
                MipLevels = 1,
                SampleDescription = new SampleDescription(4, 0)
            };

            using (Texture2D depthBuffer = new Texture2D(GraphicsDevice, depthBufferDesc))
            {
                DefaultCanvas.depthStencil = new DepthStencilView(GraphicsDevice, depthBuffer);
            }
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
