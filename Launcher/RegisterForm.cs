using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class RegisterForm : Form
    {
        Button FOwnerButton;
        public RegisterForm(Button vOwnerButton)
        {
            InitializeComponent();
            FOwnerButton = vOwnerButton;
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
            IItem wItem;
            var wPath = this.txtItem.Text;
            var wLocation = FOwnerButton.Location;
            if (File.Exists(wPath))
            {
                wItem = new FileItem(wPath, wLocation);
            }
            else if (Directory.Exists(wPath))
            {
                wItem = new FolderItem(wPath, wLocation);
            }
            else
            {
                wItem = new UrlItem(wPath, wLocation);
            }
            FOwnerButton.Tag = wItem;
            FOwnerButton.Image = wItem.Icon.ToBitmap();
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
