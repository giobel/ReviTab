using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReviTab
{
    public partial class FormCreateViewSet : Form
    {
        public string tBoxViewsetName { get; set; }
        public string tBoxSheetNumber { get; set; }

        public FormCreateViewSet()
        {
            InitializeComponent();
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            tBoxViewsetName = formTBoxViewsetName.Text;
            tBoxSheetNumber = formTBoxSheetNumbers.Text;
        }
    }
}
