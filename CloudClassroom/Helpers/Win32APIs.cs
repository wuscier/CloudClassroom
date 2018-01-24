using System;
using System.Runtime.InteropServices;

namespace CloudClassroom.Helpers
{
    public class Win32APIs
    {
        public static int SW_HIDE = 0;
        public static int SW_SHOW = 5;
        public static uint WM_SHOWWINDOW = 0x0018;

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandleA(string name);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookExA(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", SetLastError = true, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

    }
}
