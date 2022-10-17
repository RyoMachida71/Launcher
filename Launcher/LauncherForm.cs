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
            Directory.CreateDirectory(C_Directory);
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
                        MessageBox.Show("まず右クリック押してファイルを登録せえや。");
                        break;
                    }
                    Process.Start(wItemAssociatedToButton.Path);
                    break;
                case MouseButtons.Right:
                    ((Button)sender).ContextMenuStrip = GetContextMenu((Button)sender);
                    break;
                default:
                    break;
            }
        }
        private void AttachClickEventToAllButtons()
        {
            foreach (Button wButton in this.Controls) wButton.MouseDown += Button_Clicked;
        }
        private ContextMenuStrip GetContextMenu(Button vButton)
        {
            var wMenu = new ContextMenuStrip();
            wMenu.Items.Add("登録", default, (s, e) => RegisterItem(vButton));
            wMenu.Items.Add("削除", default, (s, e) => RemoveItem(vButton));
            return wMenu;
        }
        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
        }
        private void RegisterItem(Button vButton)
        {
            var wDialog = new OpenFileDialog();
            if (wDialog.ShowDialog() == DialogResult.OK)
            {
                string wPath = wDialog.FileName;
                var wFile = new Item(wPath, vButton.Location);
                vButton.Tag = wFile;
                vButton.Image = wFile.Icon.ToBitmap();
            }
        }
        private void RemoveItem(Button vButton)
        {
            // TODO:削除時に、Itemオブジェクトの所有物をデフォルト化する。
        }
        private void Save()
        {
            var wItems = this.Controls.Cast<Button>().Select(x => (Item)x.Tag).Where(x => x != null).ToList();
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
            var wButtons = this.Controls.Cast<Button>().ToArray();
            for (int i = 0; i < wItems.Count; ++i)
            {
                for (int j = 0; j < wButtons.Length; ++j)
                {
                    if (wItems[i].Location == wButtons[j].Location)
                    {
                        wButtons[j].Tag = wItems[i];
                        wButtons[j].Image = wItems[i].Icon.ToBitmap();
                    }
                }
            }
        }
    }
}