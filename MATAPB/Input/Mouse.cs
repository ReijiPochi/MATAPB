using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace MATAPB.Input
{
    public class Mouse
    {
        public static bool CursorLock { get; set; }

        private static bool pause;

        public static bool _CursorVisibility;
        public static bool CursorVisibility
        {
            get { return _CursorVisibility; }
            set
            {
                if (value)
                    PresentationArea.Overlay.Cursor = Cursors.Arrow;
                else
                    PresentationArea.Overlay.Cursor = Cursors.None;

                _CursorVisibility = value;
            }
        }

        private static bool cursorLocked;
        private static POINT lockPosition;
        private static bool oneTimeCancel;

        private static POINT prePosition;
        private static double deltaX = 0, deltaY = 0;
        private static bool valueChanging;

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void SetCursorPos(int X, int Y);

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void GetCursorPos(ref POINT p);

        private struct POINT
        {
            public int X;
            public int Y;
        }

        public static void Initialize()
        {
            if (PresentationArea.Overlay == null)
                throw new Exception("PresentationArea.Overlay が null です。");

            PresentationArea.Overlay.PreviewMouseMove += Overlay_PreviewMouseMove;
            PresentationArea.Overlay.Deactivated += Overlay_Deactivated;
            PresentationArea.Overlay.Activated += Overlay_Activated;
        }

        public static Point GetDelta()
        {
            for(int c = 0; c < 1000000; c++)
            {
                if (!valueChanging)
                    break;
            }

            Point delta = new Point(deltaX, deltaY);

            deltaX = 0;
            deltaY = 0;

            return delta;
        }

        private static void Overlay_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (pause)
                return;

            if(prePosition.X == 0 && prePosition.Y == 0)
            {
                GetCursorPos(ref prePosition);
            }

            if (oneTimeCancel)
            {
                oneTimeCancel = false;
                return;
            }

            POINT position = new POINT();

            GetCursorPos(ref position);

            valueChanging = true;
            deltaX += position.X - prePosition.X;
            deltaY += position.Y - prePosition.Y;
            valueChanging = false;

            if (CursorLock)
            {
                if (!cursorLocked)
                {
                    GetCursorPos(ref lockPosition);
                    cursorLocked = true;
                    prePosition = lockPosition;
                }

                SetCursorPos(lockPosition.X, lockPosition.Y);
                oneTimeCancel = true;
            }
            else
            {
                cursorLocked = false;
                prePosition = position;
            }
        }

        private static void Overlay_Activated(object sender, EventArgs e)
        {
            pause = false;
            CursorVisibility = _CursorVisibility;
        }

        private static void Overlay_Deactivated(object sender, EventArgs e)
        {
            pause = true;
            cursorLocked = false;
            prePosition.X = 0;
            prePosition.Y = 0;
            PresentationArea.Overlay.Cursor = Cursors.Arrow;
        }

    }
}
