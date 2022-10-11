using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Launcher
{
    public partial class LauncherForm : Form
    {
        private List<Item> FItems  = new List<Item>();
        private const string C_Directory = @"C:\ProgramData\Launcher";
        private const string C_JsonFileName = @"Launcher.json";
        public LauncherForm()
        {
            InitializeComponent();
            AttachClickEventToAllButtons();
            // jsonファイルが存在すれば、デシリアライズ
        }
        private void Button_Clicked(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    Item wAppAssociatedToButton = (Item)((Button)sender).Tag;
                    if (wAppAssociatedToButton == null)
                    {
                        MessageBox.Show("まず右クリック押してアプリを登録せえや。");
                        break;
                    }
                    Process.Start(wAppAssociatedToButton.Path);
                    break;
                case MouseButtons.Right:
                    var wDialog = new OpenFileDialog();
                    if (wDialog.ShowDialog() == DialogResult.OK)
                    {
                        string wPath = wDialog.FileName;
                        var wFile = new Item(wPath);
                        ((Button)sender).Tag = wFile;
                        ((Button)sender).Image = wFile.Icon.ToBitmap();
                        FItems.Add(wFile);
                    }
                    break;
            }
        }
        private void AttachClickEventToAllButtons()
        {
            foreach (Button wButton in this.Controls) wButton.MouseDown += Button_Clicked;
        }

        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FItems.Count == 0) return;
            Save(Path.Combine(C_Directory, C_JsonFileName));
            
        }
        private void Save(string vPath)
        {
            if (string.IsNullOrEmpty(vPath)) return;
            using (var wStream = File.Create(vPath))
            using (var wWriter = new StreamWriter(wStream, Encoding.UTF8)) new JsonSerializer
            {
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Ignore
            }.Serialize(wWriter, FItems);
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