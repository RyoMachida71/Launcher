using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Launcher
{
    public partial class LauncherForm : Form
    {
        private const string C_Directory = @"C:\ProgramData\Launcher";
        private const string C_JsonFileName = @"Launcher.json";
        public LauncherForm()
        {
            InitializeComponent();
            AttachClickEventToAllButtons();
            Load();
        }
        private void Button_Clicked(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Item wItemAssociatedToButton = (Item)((Button)sender).Tag;
                    if (wItemAssociatedToButton == null)
                    {
                        MessageBox.Show("まず右クリック押してアプリを登録せえや。");
                        break;
                    }
                    Process.Start(wItemAssociatedToButton.Path);
                    break;
                case MouseButtons.Right:
                    var wDialog = new OpenFileDialog();
                    if (wDialog.ShowDialog() == DialogResult.OK)
                    {
                        string wPath = wDialog.FileName;
                        var wFile = new Item(wPath);
                        ((Button)sender).Tag = wFile;
                        ((Button)sender).Image = wFile.Icon.ToBitmap();
                    }
                    break;
                default:
                    return;
                    break;
            }
        }
        private void AttachClickEventToAllButtons()
        {
            foreach (Button wButton in this.Controls) wButton.MouseDown += Button_Clicked;
        }

        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
        }
        private void Save()
        {
            var wItems = this.Controls.Cast<Button>().OrderBy(x => x.Top).ThenBy(x => x.Location.X).Select(x => (Item)x.Tag).Where(x => x != null).ToList();
            if (wItems.Count == 0) return;
            using (var wStream = File.Create(Path.Combine(C_Directory, C_JsonFileName)))
            using (var wWriter = new StreamWriter(wStream, Encoding.UTF8)) new JsonSerializer
            {
                Formatting = Formatting.Indented,
            }.Serialize(wWriter, wItems);
        }
        private void Load()
        {
            if (!File.Exists(Path.Combine(C_Directory, C_JsonFileName))) return;
            var wJson = File.ReadAllText(Path.Combine(C_Directory, C_JsonFileName));
            var wItems = JsonConvert.DeserializeObject<List<Item>>(wJson);
            var wButtons = this.Controls.Cast<Button>().OrderBy(x => x.Top).ThenBy(x => x.Location.X).ToArray();
            for (int i = 0; i < wItems.Count; ++i)
            {
                wButtons[i].Tag = wItems[i];
                wButtons[i].Image = wItems[i].Icon.ToBitmap();
            }
        }
    }
}
/*
public void Save(string vPath) {
            using (var wStream = File.Create(vPath))
            using (var wWriter = new StreamWriter(wStream, Encoding.UTF8)) new JsonSerializer {
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Ignore
            }.Serialize(wWriter, this);
        }
*/