using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace MATAPB.Input
{
    public delegate void KeyInputEventHandler(Key key);

    public class Keyboard
    {
        public static Dictionary<Key, bool> KeyStates { get; private set; } = new Dictionary<Key, bool>();

        public static event KeyInputEventHandler KeyInput;

        public static void Initialize()
        {
            if (PresentationArea.Overlay == null)
                throw new Exception("PresentationArea.Overlay が null です。");

            foreach(Key key in Enum.GetValues(typeof(Key)))
            {
                KeyStates[key] = false;
            }

            PresentationArea.Overlay.PreviewKeyDown += Overlay_PreviewKeyDown;
            PresentationArea.Overlay.PreviewKeyUp += Overlay_PreviewKeyUp;
        }

        private static void Overlay_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyInput?.Invoke(e.Key);
            KeyStates[e.Key] = true;
        }

        private static void Overlay_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            KeyStates[e.Key] = false;
        }
    }
}
