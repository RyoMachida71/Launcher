using Launcher.Items;

namespace Launcher
{
    public partial class RegisterForm : Form
    {
        Point FItemLocation;
        Action<IItem> FOnItemCreated;

        public RegisterForm(Point vLocation, Action<IItem> vOnItemCreated)
        {
            InitializeComponent();
            FItemLocation = vLocation;
            FOnItemCreated = vOnItemCreated;
        }
        private void btnFile_Clicked(object sender, EventArgs e)
        {
            var wDialog = new OpenFileDialog();
            if (wDialog.ShowDialog() == DialogResult.OK)
            {
                string wPath = wDialog.FileName;
                this.txtItem.Text = wPath;
            }
        }
        private void btnFolder_Clicked(object sender, EventArgs e)
        {
            var wDialog = new FolderBrowserDialog();
            if (wDialog.ShowDialog() == DialogResult.OK)
            {
                string wPath = wDialog.SelectedPath;
                this.txtItem.Text = wPath;
            }
        }
        private void btnOK_Clicked(object sender, EventArgs e)
        {
            var wPath = this.txtItem.Text;
            if (string.IsNullOrWhiteSpace(wPath)) {
                MessageBox.Show("パスまたはURLを入力してください。");
                return;
            }
            var wItem = ItemFactory.Create(wPath, FItemLocation);
            if (wItem == null) return;
            FOnItemCreated(wItem);
            this.Close();
        }
        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            this.Close();
        }
        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.txtItem.Text = "";
        }
    }
}
