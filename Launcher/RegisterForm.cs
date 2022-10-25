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
            // ↓の実装ではフォルダのアイコン取得時に例外が発生するため、Itemクラスだけでは無理があるかも、、、
            // URLが入力されたとき用のクラスも必要かも
            var wItem = new Item(this.txtItem.Text, FOwnerButton.Location);
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
