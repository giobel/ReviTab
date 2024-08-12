using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReviTab
{
	/// <summary>
	/// Description of Form2.
	/// </summary>
	public partial class FormOpenFile : Form
	{
		public string filePath;

        public bool cleanArchModel { get; internal set; }

        public bool purgeModel { get; internal set; }

        public FormOpenFile()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void OpenBtnClick(object sender, EventArgs e)
		{
			filePath = tBoxRvtFilePath.Text;
			
		}
		
		void TBoxRvtFilePathTextChanged(object sender, EventArgs e)
		{
			
		}
		
		void Btn_browseClick(object sender, EventArgs e)
		{	
			DialogResult result = openFileDialog1.ShowDialog();
			tBoxRvtFilePath.Text = openFileDialog1.FileName;
		}
		
		
		void OpenFileDialog1FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}

        private void checkBoxCleanArchModel_CheckedChanged(object sender, EventArgs e)
        {
           if (checkBoxCleanArchModel.Checked)
            {
                cleanArchModel = true;
            }
        }

        private void checkBoxPurge_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPurge.Checked)
            {
                purgeModel = true;
            }
        }
    }
}
