using System.Diagnostics;
using System.Drawing.Text;
using System.Text.Json;

namespace Launcher
{
    public partial class LauncherForm : Form
    {
        private List<File> FFiles  = new List<File>();
        private const string C_JsonFileName = "Launcher.json";
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
                    File wAppAssociatedToButton = (File)((Button)sender).Tag;
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
                        var wFile = new File(wPath);
                        ((Button)sender).Tag = wFile;
                        ((Button)sender).Image = wFile.Icon.ToBitmap();
                        FFiles.Add(wFile);
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
            // リストに要素が存在すれば、シリアライズ
            if (FFiles.Count == 0) return;
            // TODO ↓のパス指定では例外が発生する。ディレクトリを作っておく必要あり。
            var wPath = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(), C_JsonFileName);
            using (var wWriter = new StreamWriter(wPath))
            {
                var wJson = JsonSerializer.Serialize(FFiles);
                wWriter.Write(wJson);
            }
        }
    }
}