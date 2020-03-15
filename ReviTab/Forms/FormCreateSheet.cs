using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReviTab.Forms
{
    public partial class FormCreateSheet : Form
    {
        public string SheetNumber { get; set; }
        public string PackageName { get; set; }
        public int Count { get; set; }


        public FormCreateSheet()
        {
            InitializeComponent();
        }

        private void Ok_btn_Click(object sender, EventArgs e)
        {
            SheetNumber = tBoxSheetNumber.Text;
            PackageName = tboxPackage.Text;
            Count = Int16.Parse(tBoxQuantity.Text);
        }
    }
}
