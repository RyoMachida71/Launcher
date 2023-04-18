using Newtonsoft.Json;
using System.Diagnostics;

namespace Launcher.Items
{
    internal class UrlItem : IItem
    {
        [JsonIgnore]
        public Icon Icon => WinAPI.GetIcon($@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\Microsoft\Edge\Application\msedge.exe");
        [JsonProperty]
        public string Path { get; private set; }
        [JsonProperty]
        public string Name => Path;
        [JsonProperty]
        public Point Location { get; private set; }
        public UrlItem(string vPath, Point vLocation)
        {
            Path = vPath;
            Location = vLocation;
        }
        public UrlItem() { }
        public bool Start()
        {
            try
            {
                var wInfo = new ProcessStartInfo()
                {
                    FileName = Path,
                    UseShellExecute = true,
                };
                Process.Start(new ProcessStartInfo() { FileName = Path, UseShellExecute = true });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}