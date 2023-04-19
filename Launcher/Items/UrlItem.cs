using Newtonsoft.Json;
using System.Diagnostics;

namespace Launcher.Items
{
    internal class UrlItem : IItem
    {
        private static string DefaultUrlExe = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\Microsoft\Edge\Application\msedge.exe";
        [JsonIgnore]
        public Icon Icon => WinAPI.GetIcon(DefaultUrlExe);
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