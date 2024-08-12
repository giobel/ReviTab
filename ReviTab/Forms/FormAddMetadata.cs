using System;
using System.Windows.Forms;

namespace ReviTab
{
    public partial class FormAddMetadata : Form
    {
        public string filePath;

        public FormAddMetadata()
        {
            InitializeComponent();
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            filePath = tBoxRvtFilePath.Text;

        }



    }
}
