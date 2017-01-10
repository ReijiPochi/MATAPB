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

        Worlds.TempleWorld templeWorld;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PresentationArea.Initialize(60.0);

            templeWorld = new Worlds.TempleWorld();
            PresentationArea.World = templeWorld;

            MATAPB.Input.Keyboard.Initialize();
            MATAPB.Input.Mouse.Initialize();
            MATAPB.Input.Mouse.CursorLock = true;
            MATAPB.Input.Mouse.CursorVisibility = false;

            PresentationArea.Launch();
        }
    }
}
