using Launcher.Items;

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
                        MessageBox.Show("まず右クリック押してファイルを登録するのだ。");
                        break;
                    }
                    if (!wItem.Start())
                    {
                        MessageBox.Show($"ファイル開けぬぞ。{Environment.NewLine}登録してるファイルを確認するのだ。");
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
            var wRegisterForm = new RegisterForm(vButton.Location, (IItem vItem) => {
                vButton.Tag = vItem;
                vButton.Image = vItem.Icon.ToBitmap();
                vButton.Text = vItem.Name;
            });
            wRegisterForm.ShowDialog();
        }
        private void RemoveItem(Button vButton)
        {
            vButton.Tag = default;
            vButton.Image = default;
            vButton.Text = "";
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
            ItemSerializer.SaveItem(wFileItems, Path.Combine(C_Directory, C_JsonForFileItem));
        }
        private void SaveFolderItem()
        {
            var wFolderItems = this.Controls.Cast<Button>().Where(x => x.Tag is FolderItem wFolderItem && wFolderItem != null).Select(x => (FolderItem)x.Tag).ToList();
            ItemSerializer.SaveItem(wFolderItems, Path.Combine(C_Directory, C_JsonForFolderItem));
        }
        private void SaveUrlItem()
        {
            var wUrlItems = this.Controls.Cast<Button>().Where(x => x.Tag is UrlItem wUrlItem && wUrlItem != null).Select(x => (UrlItem)x.Tag).ToList();
            ItemSerializer.SaveItem(wUrlItems, Path.Combine(C_Directory, C_JsonForUrlItem));
        }
        private void Load()
        {
            LoadFileItem();
            LoadFolderItem();
            LoadUrlItem();
        }
        private void LoadFileItem()
        {
            var wPath = Path.Combine(C_Directory, C_JsonForFileItem);
            if (!File.Exists(wPath)) return;
            AttachItemToButton(ItemSerializer.LoadItem<FileItem>(wPath));
        }
        private void LoadFolderItem()
        {
            var wPath = Path.Combine(C_Directory, C_JsonForFolderItem);
            if (!File.Exists(wPath)) return;
            AttachItemToButton(ItemSerializer.LoadItem<FolderItem>(wPath));
        }
        private void LoadUrlItem()
        {
            var wPath = Path.Combine(C_Directory, C_JsonForUrlItem);
            if (!File.Exists(wPath)) return;
            AttachItemToButton(ItemSerializer.LoadItem<UrlItem>(wPath));
        }
        private void AttachItemToButton<T>(List<T> vItems) where T: IItem
        {
            var wButtons = this.Controls.Cast<Button>().ToArray();
            foreach (var wItem in vItems)
            {
                var wButton = wButtons.FirstOrDefault(x => x.Location == wItem.Location);
                wButton.Tag = wItem;
                wButton.Image = wItem.Icon.ToBitmap();
                wButton.Text = wItem.Name;
            }
        }
        #endregion
    }
}