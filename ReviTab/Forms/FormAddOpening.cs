/*
 * Created by SharpDevelop.
 * User: Giovanni.Brogiolo
 * Date: 26/10/2018
 * Time: 2:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using winForms = System.Windows.Forms;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace ReviTab
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class FormAddOpening : winForms.Form
    {
		
		public string distances {get; set;}

		public List<string> familyName = new List<string>();
		
		public string choosenFamily = null;

        public string choosenCategory = null;

        public int formVoidWidth;
		
		public int formVoidHeight;
		
		public string formPosition;

        Document rvtDoc = null;

		public FormAddOpening(Document doc)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

            rvtDoc = doc;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
	    void Form1Load(object sender, EventArgs e)
		{

            textBox1.Text = "0";
            textBoxWidth.Text = "400";
            textBoxHeight.Text = "350";

            comboBoxCategory.Items.Add("Structural Framing");
            comboBoxCategory.Items.Add("Structural Connections");
            
        }
		
		void Ok_btnClick(object sender, EventArgs e)
		{
			distances = textBox1.Text;
			formVoidWidth = Int16.Parse(textBoxWidth.Text);
			formVoidHeight = Int16.Parse(textBoxHeight.Text);
			
			if (checkBoxStart.Checked)
				formPosition = "start";
			if (checkBoxEnd.Checked)
				formPosition = "end";
			if (checkBoxMidPoint.Checked)
				formPosition = "mid";

            
		}
		

		void ComboBoxCategorySelectedIndexChanged(object sender, EventArgs e)
		{
			//choosenFamily = familyName[comboBoxCategory.SelectedIndex];
			choosenFamily = comboBoxFamily.SelectedItem.ToString();
			
		}
		
		void CheckBoxStart_Click(object sender, System.EventArgs e)
		{
					checkBoxEnd.Checked = false;
					checkBoxMidPoint.Checked = false;
		}
		
		
		
		void CheckBoxEnd_Click(object sender, System.EventArgs e)
		{
			checkBoxStart.Checked = false;
			checkBoxMidPoint.Checked = false;
		}
        
        private void btnLoadCategory_Click(object sender, EventArgs e)
        {
            choosenCategory = comboBoxCategory.SelectedItem.ToString();

            Dictionary<string, FamilySymbol> allCategories = Helpers.SelectFamilies(rvtDoc, choosenCategory);

            foreach (string s in allCategories.Keys)
            {
                try
                {
                    this.comboBoxFamily.Items.Add(s);
                }
                catch
                {
                    continue;
                }
            }
        }

        private void btnClearCat_Click(object sender, EventArgs e)
        {
            this.comboBoxFamily.Items.Clear();
            
        }
    }
}
