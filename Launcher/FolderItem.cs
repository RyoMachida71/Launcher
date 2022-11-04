using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Launcher
{
    internal class FolderItem : IItem
    {
        // ------------Win API------------
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
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
        // -------------------------------
        [JsonIgnore]
        public Icon Icon => GetFolderIcon();
        [JsonProperty]
        public string Path { get; private set; }
        [JsonProperty]
        public Point Location { get; private set; }
        public FolderItem(string vPath, Point vLocation)
        {
            if (string.IsNullOrEmpty(vPath)) throw new ArgumentException("ファイルパスが不正です");
            this.Path = vPath;
            Location = vLocation;
        }
        public FolderItem() { }
        private Icon GetFolderIcon()
        {
            var wFileInfo = new SHFILEINFO();
            try
            {
                SHGetFileInfo(this.Path, 0, ref wFileInfo, (uint)Marshal.SizeOf(wFileInfo), SHGFI_ICON + SHGFI_LARGEICON);
                if (wFileInfo.hIcon == IntPtr.Zero) return null;
                else return (Icon)Icon.FromHandle(wFileInfo.hIcon).Clone();
            }
            finally
            {
                if (wFileInfo.hIcon != IntPtr.Zero) DestroyIcon(wFileInfo.hIcon);
            }
        }
        public bool Start()
        {
            try
            {
                Process.Start("explorer.exe", this.Path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}