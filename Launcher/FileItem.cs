using Newtonsoft.Json;
using System.Diagnostics;

namespace Launcher
{
    internal class FileItem : IItem
    {
        [JsonIgnore]
        public Icon Icon => Icon.ExtractAssociatedIcon(this.Path);
        [JsonProperty]
        public string Path { get; private set; }
        [JsonProperty]
        public Point Location { get; private set; }
        public FileItem(string vPath, Point vLocation)
        {
            if (string.IsNullOrEmpty(vPath)) throw new ArgumentException("ファイルパスが不正です");
            this.Path = vPath;
            Location = vLocation;
        }
        public FileItem() { }
        public bool Start()
        {
            try
            {
                Process.Start(this.Path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
