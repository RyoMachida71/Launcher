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
        public Icon Icon => WinAPI.GetIcon($@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\Microsoft\Edge\Application\msedge.exe");
        [JsonProperty]
        public string Path { get; private set; }
        [JsonProperty]
        public string Name => this.Path;
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
                Process.Start(new ProcessStartInfo(){FileName = this.Path, UseShellExecute = true });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}