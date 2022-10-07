using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    internal class File
    {
        public Icon Icon { get; private set; }
        public string Path { get; private set; }
        public File(string vPath)
        {
            if (string.IsNullOrEmpty(vPath)) throw new ArgumentException("ファイルパスが不正です");
            this.Path = vPath;
            this.Icon = Icon.ExtractAssociatedIcon(vPath);
        }
    }
}
