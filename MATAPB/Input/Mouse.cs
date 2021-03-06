﻿using System;
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

        public static bool _Active = true;
        public static bool Active
        {
            get { return _Active; }
            set
            {
                if (value)
                {
                    Restart();
                }
                else
                {
                    Pause();
                }

                _Active = value;
            }
        }

        private static bool pause;

        public static bool _CursorVisibility;
        public static bool CursorVisibility
        {
            get { return _CursorVisibility; }
            set
            {
                if (value)
                    PresentationBase.Overlay.Cursor = Cursors.Arrow;
                else
                    PresentationBase.Overlay.Cursor = Cursors.None;

                _CursorVisibility = value;
            }
        }

        public static bool RightButtonDown { get; set; }

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
            if (PresentationBase.Overlay == null)
                throw new Exception("PresentationArea.Overlay が null です。");

            PresentationBase.Overlay.PreviewMouseMove += Overlay_PreviewMouseMove;
            PresentationBase.Overlay.MouseDown += Overlay_MouseDown;
            PresentationBase.Overlay.MouseUp += Overlay_MouseUp;
            PresentationBase.Overlay.Deactivated += Overlay_Deactivated;
            PresentationBase.Overlay.Activated += Overlay_Activated;
        }

        private static void Overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                RightButtonDown = true;
        }

        private static void Overlay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Released)
                RightButtonDown = false;
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
            Restart();
        }

        private static void Overlay_Deactivated(object sender, EventArgs e)
        {
            Pause();
        }

        public static void Pause()
        {
            pause = true;
            cursorLocked = false;
            prePosition.X = 0;
            prePosition.Y = 0;
            PresentationBase.Overlay.Cursor = Cursors.Arrow;
        }

        public static void Restart()
        {
            pause = false;
            CursorVisibility = _CursorVisibility;
        }
    }
}
