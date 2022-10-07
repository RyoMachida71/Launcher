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
            // json�t�@�C�������݂���΁A�f�V���A���C�Y
        }
        private void Button_Clicked(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    File wAppAssociatedToButton = (File)((Button)sender).Tag;
                    if (wAppAssociatedToButton == null)
                    {
                        MessageBox.Show("�܂��E�N���b�N�����ăA�v����o�^������B");
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
            // ���X�g�ɗv�f�����݂���΁A�V���A���C�Y
            if (FFiles.Count == 0) return;
            // TODO ���̃p�X�w��ł͗�O����������B�f�B���N�g��������Ă����K�v����B
            var wPath = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(), C_JsonFileName);
            using (var wWriter = new StreamWriter(wPath))
            {
                var wJson = JsonSerializer.Serialize(FFiles);
                wWriter.Write(wJson);
            }
        }
    }
}