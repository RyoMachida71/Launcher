using Newtonsoft.Json;
using System.Diagnostics;

namespace Launcher
{
    internal class Item
    {
        [JsonIgnore]
        public Icon Icon => Icon.ExtractAssociatedIcon(this.Path);
        [JsonProperty]
        public string Path { get; private set; }
        [JsonProperty]
        public Point Location { get; private set; }
        public Item(string vPath, Point vLocation)
        {
            if (string.IsNullOrEmpty(vPath)) throw new ArgumentException("ファイルパスが不正です");
            this.Path = vPath;
            Location = vLocation;
        }
        public Item() { }
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
