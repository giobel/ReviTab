using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReviTab
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class FormCreateSections : Form
	{
		public string sectionOrientation{get;set;}
		public double sectionPositionOffset {get;set;}
		public double farClipOffset {get;set;}
		public double bottomLevel {get;set;}
		public double topLevel {get;set;}
		public bool flipDirection {get;set;}
	
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
			
		}

	}
}
