using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace ReviTab
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class FormCreateSections : Form
	{
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string sectionOrientation{get;set;}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double sectionPositionOffset {get;set;}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double farClipOffset {get;set;}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double bottomLevel {get;set;}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double topLevel {get;set;}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool flipDirection {get;set;}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string columnParameter { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string prefixText { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Autodesk.Revit.DB.Element chosenSection { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICollection<Autodesk.Revit.DB.ViewFamilyType> SectionTypes { get; set; }
		public FormCreateSections()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
		}

		
		void Ok_buttonClick(object sender, EventArgs e)
		{
			if (checkBoxLong.Checked)
				sectionOrientation = "Long";
			else
				sectionOrientation = "Cross";

			if (checkBoxFlip.Checked)
				flipDirection = true;
			else
				flipDirection = false;

			sectionPositionOffset =	Int16.Parse(sectionPositionTxt.Text)/304.8;
			farClipOffset = Int16.Parse(farClipOffsetTxt.Text)/304.8;
			bottomLevel = Int16.Parse(bottomLevelTxt.Text)/304.8*1000;
			topLevel = Int16.Parse(topLevelTxt.Text)/304.8*1000;
			columnParameter = textBoxParameter.Text;
			prefixText = textBoxPrefix.Text;
			chosenSection = (Autodesk.Revit.DB.Element)comboBoxSectionTypes.SelectedItem;

		}

		void CheckBoxLong_Click(object sender, EventArgs e)
		{
				checkBoxCross.Checked = false;
				
		}
		
		void CheckBoxCross_Click(object sender, EventArgs e)
		{
				checkBoxLong.Checked = false;
				
		}
		
		
		void Form1Load(object sender, EventArgs e)
		{
			comboBoxSectionTypes.DataSource = SectionTypes;
			comboBoxSectionTypes.DisplayMember = "Name";
		}

	}
}
