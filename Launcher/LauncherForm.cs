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
                        MessageBox.Show("�܂��E�N���b�N�����ăt�@�C����o�^������B");
                        break;
                    }
                    if (!wItem.Start())
                    {
                        MessageBox.Show($"�t�@�C���J���񂩂������B{Environment.NewLine}�o�^���Ă�t�@�C�����m�F�����B");
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
            wMenu.Items.Add("�o�^", default, (s, e) => OpenRegister(vButton));
            wMenu.Items.Add("�폜", default, (s, e) => RemoveItem(vButton));
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
        #region �ۑ��E���[�h����
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
            ItemSerializer.SaveItem(wFileItems, Path.Combine(C_Directory, C_JsonForFileItem));
        }
        private void SaveFolderItem()
        {
            var wFolderItems = this.Controls.Cast<Button>().Where(x => x.Tag is FolderItem wFolderItem && wFolderItem != null).Select(x => (FolderItem)x.Tag).ToList();
            if (wFolderItems.Count == 0) return;
            ItemSerializer.SaveItem(wFolderItems, Path.Combine(C_Directory, C_JsonForFolderItem));
        }
        private void SaveUrlItem()
        {
            var wUrlItems = this.Controls.Cast<Button>().Where(x => x.Tag is UrlItem wUrlItem && wUrlItem != null).Select(x => (UrlItem)x.Tag).ToList();
            if (wUrlItems.Count == 0) return;
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
            var wItems = ItemSerializer.LoadItem<FileItem>(wPath);
            var wButtons = this.Controls.Cast<Button>().ToArray();
            AttachItemToButton(wItems);
        }
        private void LoadFolderItem()
        {
            var wPath = Path.Combine(C_Directory, C_JsonForFolderItem);
            if (!File.Exists(wPath)) return;
            var wItems = ItemSerializer.LoadItem<FolderItem>(wPath);
            AttachItemToButton(wItems);
        }
        private void LoadUrlItem()
        {
            var wPath = Path.Combine(C_Directory, C_JsonForUrlItem);
            if (!File.Exists(wPath)) return;
            var wItems = ItemSerializer.LoadItem<UrlItem>(wPath);
            AttachItemToButton(wItems);
        }
        private void AttachItemToButton<T>(List<T> vItems) where T: IItem
        {
            var wButtons = this.Controls.Cast<Button>().ToArray();
            foreach (var wItem in vItems)
            {
                var wButton = wButtons.FirstOrDefault(x => x.Location == wItem.Location);
                wButton.Tag = wItem;
                wButton.Image = wItem.Icon.ToBitmap();
            }
        }
        #endregion
    }
}