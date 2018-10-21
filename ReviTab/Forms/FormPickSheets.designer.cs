/*
 * Created by SharpDevelop.
 * User: Giovanni.Brogiolo
 * Date: 20/10/2018
 * Time: 5:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ReviTab
{
	partial class FormPickSheets
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.sheetCheckedList = new System.Windows.Forms.CheckedListBox();
            this.tBoxSheetNumbers = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tBoxPrefix = new System.Windows.Forms.TextBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.cncl_btn = new System.Windows.Forms.Button();
            this.btn_check = new System.Windows.Forms.Button();
            this.btn_Uncheck = new System.Windows.Forms.Button();
            this.comboBoxViewset = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // sheetCheckedList
            // 
            this.sheetCheckedList.CheckOnClick = true;
            this.sheetCheckedList.FormattingEnabled = true;
            this.sheetCheckedList.HorizontalScrollbar = true;
            this.sheetCheckedList.Location = new System.Drawing.Point(12, 36);
            this.sheetCheckedList.Name = "sheetCheckedList";
            this.sheetCheckedList.Size = new System.Drawing.Size(526, 154);
            this.sheetCheckedList.TabIndex = 2;
            this.sheetCheckedList.UseCompatibleTextRendering = true;
            this.sheetCheckedList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SheetCheckedList_ItemCheck);
            // 
            // tBoxSheetNumbers
            // 
            this.tBoxSheetNumbers.Location = new System.Drawing.Point(197, 242);
            this.tBoxSheetNumbers.Name = "tBoxSheetNumbers";
            this.tBoxSheetNumbers.Size = new System.Drawing.Size(341, 20);
            this.tBoxSheetNumbers.TabIndex = 3;
            this.tBoxSheetNumbers.Text = "i.e. 101 102 103";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(393, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select the Sheet to Print or type their Sheet Number in the text box below";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Prefix";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // tBoxPrefix
            // 
            this.tBoxPrefix.Location = new System.Drawing.Point(12, 242);
            this.tBoxPrefix.Name = "tBoxPrefix";
            this.tBoxPrefix.Size = new System.Drawing.Size(179, 20);
            this.tBoxPrefix.TabIndex = 6;
            this.tBoxPrefix.Text = "ARP-AR-DR-ST-";
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(185, 275);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 7;
            this.okBtn.Text = "OK";
            this.okBtn.UseCompatibleTextRendering = true;
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.OkBtnClick);
            // 
            // cncl_btn
            // 
            this.cncl_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cncl_btn.Location = new System.Drawing.Point(313, 275);
            this.cncl_btn.Name = "cncl_btn";
            this.cncl_btn.Size = new System.Drawing.Size(75, 23);
            this.cncl_btn.TabIndex = 8;
            this.cncl_btn.Text = "Cancel";
            this.cncl_btn.UseCompatibleTextRendering = true;
            this.cncl_btn.UseVisualStyleBackColor = true;
            // 
            // btn_check
            // 
            this.btn_check.Location = new System.Drawing.Point(12, 196);
            this.btn_check.Name = "btn_check";
            this.btn_check.Size = new System.Drawing.Size(75, 20);
            this.btn_check.TabIndex = 9;
            this.btn_check.Text = "Select All";
            this.btn_check.UseCompatibleTextRendering = true;
            this.btn_check.UseVisualStyleBackColor = true;
            this.btn_check.Click += new System.EventHandler(this.Btn_checkClick);
            // 
            // btn_Uncheck
            // 
            this.btn_Uncheck.Location = new System.Drawing.Point(103, 196);
            this.btn_Uncheck.Name = "btn_Uncheck";
            this.btn_Uncheck.Size = new System.Drawing.Size(75, 20);
            this.btn_Uncheck.TabIndex = 10;
            this.btn_Uncheck.Text = "Select None";
            this.btn_Uncheck.UseCompatibleTextRendering = true;
            this.btn_Uncheck.UseVisualStyleBackColor = true;
            this.btn_Uncheck.Click += new System.EventHandler(this.Btn_UncheckClick);
            // 
            // comboBoxViewset
            // 
            this.comboBoxViewset.FormattingEnabled = true;
            this.comboBoxViewset.Location = new System.Drawing.Point(417, 198);
            this.comboBoxViewset.Name = "comboBoxViewset";
            this.comboBoxViewset.Size = new System.Drawing.Size(121, 21);
            this.comboBoxViewset.TabIndex = 11;
            this.comboBoxViewset.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // FormPickSheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 319);
            this.Controls.Add(this.comboBoxViewset);
            this.Controls.Add(this.btn_Uncheck);
            this.Controls.Add(this.btn_check);
            this.Controls.Add(this.cncl_btn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.tBoxPrefix);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tBoxSheetNumbers);
            this.Controls.Add(this.sheetCheckedList);
            this.Name = "FormPickSheets";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Background Printer";
            this.Load += new System.EventHandler(this.Form1Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button btn_Uncheck;
		private System.Windows.Forms.Button btn_check;
		private System.Windows.Forms.Button cncl_btn;
		private System.Windows.Forms.Button okBtn;
		private System.Windows.Forms.TextBox tBoxPrefix;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tBoxSheetNumbers;
		private System.Windows.Forms.CheckedListBox sheetCheckedList;
        private System.Windows.Forms.ComboBox comboBoxViewset;
    }
}
