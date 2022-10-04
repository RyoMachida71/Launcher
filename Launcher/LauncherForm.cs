using System.Diagnostics;

namespace Launcher
{
    public partial class LauncherForm : Form
    {
        public LauncherForm()
        {
            InitializeComponent();
            AttachClickEventToAllButtons();
        }
        private void Button_Clicked(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    App wAppAssociatedToButton = (App)((Button)sender).Tag;
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
                        var wApp = new App(wPath);
                        ((Button)sender).Tag = wApp;
                        ((Button)sender).Image = wApp.Icon.ToBitmap();
                    }
                    break;
            }
        }
        private void AttachClickEventToAllButtons()
        {
            foreach (Button wButton in this.Controls) wButton.MouseDown += Button_Clicked;
        }
    }
}