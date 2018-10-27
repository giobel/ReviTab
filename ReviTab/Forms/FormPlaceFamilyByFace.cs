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
using System.Windows.Forms;

namespace ReviTab
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class FormPlaceFamilyByFace : Form
	{
		
		public string distances {get; set;}
			
		public FormPlaceFamilyByFace()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		void Ok_btnClick(object sender, EventArgs e)
		{
			distances = textBox1.Text;
		}
	}
}
