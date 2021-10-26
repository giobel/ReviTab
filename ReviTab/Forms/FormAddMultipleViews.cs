using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReviTab
{
    public partial class FormAddMultipleViews : Form
    {
        public string SheetNumber { get; set; }
        public int Spacing { get; set; }
        UIDocument thisUIdoc { get; set; }
        public Autodesk.Revit.DB.XYZ centerpoint { get; set; }
        public bool isCenterpoint { get; set; }

        public FormAddMultipleViews(UIDocument uidoc)
        {
            InitializeComponent();
            thisUIdoc = uidoc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SheetNumber = this.textBoxSheetNumber.Text;
            Spacing = int.Parse(this.textBoxSpacing.Text);


            

            string[] centerpointText = this.textBoxCenterpoint.Text.Split(',');
            centerpoint = new Autodesk.Revit.DB.XYZ(ParseStringToFloat(centerpointText[0]), ParseStringToFloat(centerpointText[1]), ParseStringToFloat(centerpointText[2]));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.Close();
            isCenterpoint = true;
            SheetNumber = this.textBoxSheetNumber.Text;
            Spacing = int.Parse(this.textBoxSpacing.Text);

        }

        private float ParseStringToFloat(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FormAddMultipleViews_Load(object sender, EventArgs e)
        {

        }
    }
}
