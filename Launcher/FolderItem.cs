using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Launcher
{
    internal class FolderItem : IItem
    {
        [JsonIgnore]
        public Icon Icon => WinAPI.GetIcon(this.Path);
        [JsonProperty]
        public string Path { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public Point Location { get; private set; }
        public FolderItem(string vPath, Point vLocation)
        {
            if (string.IsNullOrEmpty(vPath)) throw new ArgumentException("ファイルパスが不正です");
            this.Path = vPath;
            this.Name = System.IO.Path.GetDirectoryName(this.Path);
            Location = vLocation;
        }
        public FolderItem() { }
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