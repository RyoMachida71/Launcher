using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Launcher
{
    public partial class LauncherForm : Form
    {
        private const string C_Directory = @"C:\ProgramData\Launcher";
        private const string C_JsonForFileItem = @"File.json";
        private const string C_JsonForFolderItem = @"Folder.json";
        private const string C_JsonForUrlItem = @"Url.json";
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
                    var wItem = (IItem)((Button)sender).Tag;
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
            wMenu.Items.Add("登録", default, (s, e) => OpenRegister(vButton));
            wMenu.Items.Add("削除", default, (s, e) => RemoveItem(vButton));
            return wMenu;
        }
        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
        }
        private void OpenRegister(Button vButton)
        {
            var wRegisterForm = new RegisterForm(vButton);
            wRegisterForm.ShowDialog();
        }
        private void RemoveItem(Button vButton)
        {
            vButton.Tag = default;
            vButton.Image = default;
        }
        #region 保存・ロード処理
        private void Save()
        {
            SaveFileItem();
            SaveFolderItem();
            SaveUrlItem();
        }
        private void SaveFileItem()
        {
            var wFileItems = this.Controls.Cast<Button>().Where(x => x.Tag is FileItem wFileItem && wFileItem != null).Select(x => (FileItem)x.Tag).ToList();
            if (wFileItems.Count == 0) return;
            using (var wStream = File.Create(Path.Combine(C_Directory, C_JsonForFileItem)))
            using (var wWriter = new StreamWriter(wStream, Encoding.UTF8)) new JsonSerializer
            {
                Formatting = Formatting.Indented,
            }.Serialize(wWriter, wFileItems);
        }
        private void SaveFolderItem()
        {
            var wFolderItems = this.Controls.Cast<Button>().Where(x => x.Tag is FolderItem wFolderItem && wFolderItem != null).Select(x => (FolderItem)x.Tag).ToList();
            if (wFolderItems.Count == 0) return;
            using (var wStream = File.Create(Path.Combine(C_Directory, C_JsonForFolderItem)))
            using (var wWriter = new StreamWriter(wStream, Encoding.UTF8)) new JsonSerializer
            {
                Formatting = Formatting.Indented,
            }.Serialize(wWriter, wFolderItems);
        }
        private void SaveUrlItem()
        {
            var wUrlItems = this.Controls.Cast<Button>().Where(x => x.Tag is UrlItem wUrlItem && wUrlItem != null).Select(x => (UrlItem)x.Tag).ToList();
            if (wUrlItems.Count == 0) return;
            using (var wStream = File.Create(Path.Combine(C_Directory, C_JsonForUrlItem)))
            using (var wWriter = new StreamWriter(wStream, Encoding.UTF8)) new JsonSerializer
            {
                Formatting = Formatting.Indented,
            }.Serialize(wWriter, wUrlItems);
        }
        private void Load()
        {
            LoadFileItem();
            LoadFolderItem();
            LoadUrlItem();
        }
        private void LoadFileItem()
        {
            if (!File.Exists(Path.Combine(C_Directory, C_JsonForFileItem))) return;
            var wJson = File.ReadAllText(Path.Combine(C_Directory, C_JsonForFileItem));
            var wItems = JsonConvert.DeserializeObject<List<FileItem>>(wJson);
            var wButtons = this.Controls.Cast<Button>().ToArray();
            foreach (var wItem in wItems)
            {
                var wButton = wButtons.FirstOrDefault(x => x.Location == wItem.Location);
                wButton.Tag = wItem;
                wButton.Image = wItem.Icon.ToBitmap();
            }
        }
        private void LoadFolderItem()
        {
            if (!File.Exists(Path.Combine(C_Directory, C_JsonForFolderItem))) return;
            var wJson = File.ReadAllText(Path.Combine(C_Directory, C_JsonForFolderItem));
            var wItems = JsonConvert.DeserializeObject<List<FolderItem>>(wJson);
            var wButtons = this.Controls.Cast<Button>().ToArray();
            foreach (var wItem in wItems)
            {
                var wButton = wButtons.FirstOrDefault(x => x.Location == wItem.Location);
                wButton.Tag = wItem;
                wButton.Image = wItem.Icon.ToBitmap();
            }
        }
        private void LoadUrlItem()
        {
            if (!File.Exists(Path.Combine(C_Directory, C_JsonForUrlItem))) return;
            var wJson = File.ReadAllText(Path.Combine(C_Directory, C_JsonForUrlItem));
            var wItems = JsonConvert.DeserializeObject<List<UrlItem>>(wJson);
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