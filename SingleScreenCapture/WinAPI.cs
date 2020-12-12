using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SingleScreenCapture
{
    public sealed class WinAPI
    {
        /* Constants */
        // Clipboard
        public const int CF_TEXT = 1;
        public const int CF_BITMAP = 2;
        public const int CF_DIB = 8;
        public const int CF_UNICODETEXT = 13;

        // Graphics
        public const int SRCCOPY = 0xCC0020;

        /* External Functions */
        // Clipboard
        [DllImport("User32.dll")]
        public static extern bool OpenClipboard(IntPtr hNewOwner);

        [DllImport("User32.dll")]
        public static extern bool EmptyClipboard();

        [DllImport("User32.dll")]
        public static extern IntPtr SetClipboardData(uint format, IntPtr hMemory);

        [DllImport("User32.dll")]
        public static extern IntPtr GetClipboardData(uint format);

        [DllImport("User32.dll")]
        public static extern uint EnumClipboardFormats(uint format);

        [DllImport("User32.dll")]
        public static extern bool CloseClipboard();

        // Graphics
        [DllImport("Gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hGdiObject);

        [DllImport("Gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("Gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("Gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int width, int height);

        [DllImport("Gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("Gdi32.dll")]
        public static extern bool BitBlt(IntPtr hDestDC, int destX, int destY, int width, int height, IntPtr hSourceDC, int sourceX, int sourceY, int rasterOperationType);
    }
}
