using Newtonsoft.Json;
using System.Diagnostics;

namespace Launcher
{
    internal class FileItem : IItem
    {
        [JsonIgnore]
        public Icon Icon => WinAPI.GetIcon(this.Path);
        [JsonProperty]
        public string Path { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public Point Location { get; private set; }
        public FileItem(string vPath, Point vLocation)
        {
            if (string.IsNullOrEmpty(vPath)) throw new ArgumentException("ファイルパスが不正です");
            this.Path = vPath;
            this.Name = System.IO.Path.GetFileName(this.Path);
            Location = vLocation;
        }
        public FileItem() { }
        public bool Start()
        {
            try
            {
                Process.Start(new ProcessStartInfo() { FileName = this.Path, UseShellExecute = true });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
