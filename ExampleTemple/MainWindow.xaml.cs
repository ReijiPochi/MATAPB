using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MATAPB;
using MATAPB.Timeline;
using ExampleTemple.Timeline;

namespace ExampleTemple
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        MATAPB.Timeline.Timeline mainTL = new MATAPB.Timeline.Timeline();

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PresentationBase.Initialize(60.0);

            mainTL.Add(new OpeningTitle(), 0, 2);
            mainTL.Add(new Gaming(), 3, 7);

            MATAPB.Input.Keyboard.Initialize();
            MATAPB.Input.Keyboard.KeyInput += Keyboard_KeyInput;
            MATAPB.Input.Mouse.Initialize();
            MATAPB.Input.Mouse.CursorLock = true;
            MATAPB.Input.Mouse.CursorVisibility = false;

            PresentationBase.Launch();

            mainTL.Start();
        }

        private void Keyboard_KeyInput(Key key)
        {
            if(key == Key.Enter)
            {
                MATAPB.Input.Mouse.Active = !MATAPB.Input.Mouse.Active;
                MATAPB.Input.Mouse.CursorLock = !MATAPB.Input.Mouse.CursorLock;
                MATAPB.Input.Mouse.CursorVisibility = !MATAPB.Input.Mouse.CursorVisibility;
            }

            if (key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
