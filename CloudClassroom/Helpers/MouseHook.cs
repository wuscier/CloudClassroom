using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CloudClassroom.Helpers
{
    public class MouseHook
    {
        public const int WH_MOUSE_LL = 14;
        private static int _mouseHookId = 0;
        private static HookProc _mouseHookProc;


        public static void Start(HookProc mouseHookProc)
        {
            if (_mouseHookId != 0)
            {
                return;
            }

            _mouseHookProc = mouseHookProc;
            _mouseHookId = Win32APIs.SetWindowsHookEx(WH_MOUSE_LL, _mouseHookProc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);

            if (_mouseHookId == 0)
            {
                Stop();
            }
        }

        public static void Stop()
        {
            if (_mouseHookId != 0)
            {
                Win32APIs.UnhookWindowsHookEx(_mouseHookId);

                _mouseHookId = 0;
            }
        }

        public static int CallNextHookEx(int code, Int32 wParam, IntPtr lParam)
        {
            return Win32APIs.CallNextHookEx(_mouseHookId, code, wParam, lParam);
        }
    }
}
