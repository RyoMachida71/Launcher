using System.Runtime.InteropServices;

namespace Launcher
{
    public static class WinAPI
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            internal IntPtr hIcon;
            internal IntPtr iIcon;
            internal uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            internal string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            internal string szTypeName;
        };
        const uint SHGFI_LARGEICON = 0x00000000;
        const uint SHGFI_SMALLICON = 0x00000001;
        const uint SHGFI_USEFILEATTRIBUTES = 0x00000010;
        const uint SHGFI_ICON = 0x00000100;
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyIcon(IntPtr hIcon);
        public static Icon GetIcon(string vPath)
        {
            var wFileInfo = new SHFILEINFO();
            try
            {
                SHGetFileInfo(vPath, 0, ref wFileInfo, (uint)Marshal.SizeOf(wFileInfo), WinAPI.SHGFI_ICON + WinAPI.SHGFI_SMALLICON);
                if (wFileInfo.hIcon == IntPtr.Zero) return null;
                else return (Icon)Icon.FromHandle(wFileInfo.hIcon).Clone();
            }
            finally
            {
                if (wFileInfo.hIcon != IntPtr.Zero) DestroyIcon(wFileInfo.hIcon);
            }
        }
    }
}
