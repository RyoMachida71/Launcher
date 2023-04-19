using Launcher.Items;
using Newtonsoft.Json;
using System.Text;

namespace Launcher
{
    public static class ItemSerializer 
    {
        public static List<T> LoadItem<T>(string vPath) where T : IItem
        {
            var wJson = File.ReadAllText(vPath);
            return JsonConvert.DeserializeObject<List<T>>(wJson);
        }
        public static void SaveItem<T>(List<T> vItems, string vPath) where T : IItem
        {
            using (var wStream = File.Create(vPath))
            using (var wWriter = new StreamWriter(wStream, Encoding.UTF8)) new JsonSerializer
            {
                Formatting = Formatting.Indented,
            }.Serialize(wWriter, vItems);
        }
    }
}
