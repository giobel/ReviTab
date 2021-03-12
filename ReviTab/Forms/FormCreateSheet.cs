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
        public Autodesk.Revit.DB.Element ChosenTitleblock { get; set; }
        public string SheetNumber { get; set; }
        public string PackageName { get; set; }
        public int Count { get; set; }
        public List<string> Packages { get; set; }

        public ICollection<Autodesk.Revit.DB.Element> TitleblocksNames { get; set; }

        public FormCreateSheet()
        {
            InitializeComponent();
        }


        private void Ok_btn_Click(object sender, EventArgs e)
        {
            SheetNumber = tBoxSheetNumber.Text;
            var selectedItem = cboxPackage.SelectedItem;
            if (null != selectedItem)
            {
                PackageName = selectedItem.ToString();
                ChosenTitleblock = (Autodesk.Revit.DB.Element)comboBoxTitleblocks.SelectedItem;
            }
            else{
                PackageName = cboxPackage.Text;
            }

            Count = Int16.Parse(tBoxQuantity.Text);
        }

        private void FormCreateSheet_Load(object sender, EventArgs e)
        {
            cboxPackage.DataSource = Packages;
            comboBoxTitleblocks.DataSource = TitleblocksNames;
            comboBoxTitleblocks.DisplayMember = "Name";
        }
    }
}
