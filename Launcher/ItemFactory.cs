using Launcher.Items;

namespace Launcher
{
    internal class ItemFactory {
        public static IItem Create(string vPath, Point vLocation) {
            if (File.Exists(vPath)) {
                return new FileItem(vPath, vLocation);
            } else if (Directory.Exists(vPath)) {
                return new FolderItem(vPath, vLocation);
            } else if (Uri.IsWellFormedUriString(vPath, UriKind.RelativeOrAbsolute)){
                return new UrlItem(vPath, vLocation);
            } else {
                return null;
            }
        }
    }
}
