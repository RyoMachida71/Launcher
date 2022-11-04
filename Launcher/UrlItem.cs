using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using System.Windows.Forms.VisualStyles;

namespace Launcher
{
    internal class UrlItem : IItem
    {
        [JsonIgnore]
        public Icon Icon => Icon.ExtractAssociatedIcon($@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\Microsoft\Edge\Application\msedge.exe");
        [JsonProperty]
        public string Path { get; private set; }
        [JsonProperty]
        public Point Location { get; private set; }
        public UrlItem(string vPath, Point vLocation)
        {
            if (string.IsNullOrEmpty(vPath)) throw new ArgumentException("ファイルパスが不正です");
            this.Path = vPath;
            Location = vLocation;
        }
        public UrlItem() { }
        public bool Start()
        {
            try
            {
                var wInfo = new ProcessStartInfo()
                {
                    FileName = this.Path,
                    UseShellExecute = true,
                };
                Process.Start(wInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}