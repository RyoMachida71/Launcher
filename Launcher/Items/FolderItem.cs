﻿using Newtonsoft.Json;
using System.Diagnostics;

namespace Launcher.Items
{
    internal class FolderItem : IItem
    {
        [JsonIgnore]
        public Icon Icon => WinAPI.GetIcon(Path);
        [JsonProperty]
        public string Path { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public Point Location { get; private set; }
        public FolderItem(string vPath, Point vLocation)
        {
            Path = Name = vPath;
            Location = vLocation;
        }
        public FolderItem() { }
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