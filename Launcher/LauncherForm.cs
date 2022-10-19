using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
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
                    Item wItem = (Item)((Button)sender).Tag;
                    if (wItem == null)
                    {
                        MessageBox.Show("まず右クリック押してファイルを登録せえや。");
                        break;
                    }
                    if (!wItem.Start())
                    {
                        MessageBox.Show($"ファイル開けんかったぞ。{Environment.NewLine}登録してるファイルを確認せえ。");
                    }
                    break;
                case MouseButtons.Right:
                    var wButton = (Button)sender;
                    wButton.ContextMenuStrip = GetContextMenu(wButton);
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
                var wItem = new Item(wPath, vButton.Location);
                vButton.Tag = wItem;
                vButton.Image = wItem.Icon.ToBitmap();
            }
        }
        private void RemoveItem(Button vButton)
        {
            vButton.Tag = default;
            vButton.Image = default;
        }
        #region 保存・ロード処理
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
            foreach (var wItem in wItems)
            {
                var wButton = wButtons.FirstOrDefault(x => x.Location == wItem.Location);
                wButton.Tag = wItem;
                wButton.Image = wItem.Icon.ToBitmap();
            }
        }
        #endregion
    }
}