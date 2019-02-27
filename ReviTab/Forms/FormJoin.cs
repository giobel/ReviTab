using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReviTab
{
	/// <summary>
	/// Description of JoinForm.
	/// </summary>
	public partial class FormJoin : Form
	{
		
		public bool disallowStartValue {get; set;}
		public bool disallowEndValue {get; set;}
		
		public bool allowStartValue {get; set;}
		public bool allowEndValue {get; set;}
		
		public bool miterStart {get; set;}
		public bool miterEnd {get; set;}
		
		
		public FormJoin()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			

		}
		
		
		
		void OkBtnClick(object sender, System.EventArgs e)
		{
			if (disallowCheckStart.Checked){
				disallowStartValue = true;
			}
			
			if (disallowCheckEnd.Checked){
				disallowEndValue = true;
			}
			
			if (allowCheckStart.Checked){
				allowStartValue = true;
			}
			
			if (allowCheckEnd.Checked){
				allowEndValue = true;
			}
			
			if (checkMiterStart.Checked){
				miterStart = true;
			}
			
			if (checkMiterEnd.Checked){
				miterEnd = true;
			}
			
		}
		

	}
}
