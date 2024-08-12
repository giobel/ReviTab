/*
 * Created by SharpDevelop.
 * User: Giovanni.Brogiolo
 * Date: 27/02/2019
 * Time: 2:36 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ReviTab
{
	partial class FormJoin
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
			this.allowCheckStart = new System.Windows.Forms.CheckBox();
			this.allowCheckEnd = new System.Windows.Forms.CheckBox();
			this.disallowCheckStart = new System.Windows.Forms.CheckBox();
			this.disallowCheckEnd = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkMiterEnd = new System.Windows.Forms.CheckBox();
			this.checkMiterStart = new System.Windows.Forms.CheckBox();
			this.okBtn = new System.Windows.Forms.Button();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// allowCheckStart
			// 
			this.allowCheckStart.Location = new System.Drawing.Point(6, 19);
			this.allowCheckStart.Name = "allowCheckStart";
			this.allowCheckStart.Size = new System.Drawing.Size(91, 32);
			this.allowCheckStart.TabIndex = 0;
			this.allowCheckStart.Text = "Start Point";
			this.allowCheckStart.UseCompatibleTextRendering = true;
			this.allowCheckStart.UseVisualStyleBackColor = true;
			// 
			// allowCheckEnd
			// 
			this.allowCheckEnd.Location = new System.Drawing.Point(108, 19);
			this.allowCheckEnd.Name = "allowCheckEnd";
			this.allowCheckEnd.Size = new System.Drawing.Size(80, 32);
			this.allowCheckEnd.TabIndex = 1;
			this.allowCheckEnd.Text = "End Point";
			this.allowCheckEnd.UseCompatibleTextRendering = true;
			this.allowCheckEnd.UseVisualStyleBackColor = true;
			// 
			// disallowCheckStart
			// 
			this.disallowCheckStart.Location = new System.Drawing.Point(6, 19);
			this.disallowCheckStart.Name = "disallowCheckStart";
			this.disallowCheckStart.Size = new System.Drawing.Size(97, 32);
			this.disallowCheckStart.TabIndex = 3;
			this.disallowCheckStart.Text = "Start Point";
			this.disallowCheckStart.UseCompatibleTextRendering = true;
			this.disallowCheckStart.UseVisualStyleBackColor = true;
			// 
			// disallowCheckEnd
			// 
			this.disallowCheckEnd.Location = new System.Drawing.Point(108, 19);
			this.disallowCheckEnd.Name = "disallowCheckEnd";
			this.disallowCheckEnd.Size = new System.Drawing.Size(80, 32);
			this.disallowCheckEnd.TabIndex = 2;
			this.disallowCheckEnd.Text = "End Point";
			this.disallowCheckEnd.UseCompatibleTextRendering = true;
			this.disallowCheckEnd.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.allowCheckStart);
			this.groupBox1.Controls.Add(this.allowCheckEnd);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(194, 60);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ALLOW JOIN";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.disallowCheckEnd);
			this.groupBox2.Controls.Add(this.disallowCheckStart);
			this.groupBox2.Location = new System.Drawing.Point(12, 92);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(194, 60);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "DISALLOW JOIN";
			this.groupBox2.UseCompatibleTextRendering = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.checkMiterEnd);
			this.groupBox3.Controls.Add(this.checkMiterStart);
			this.groupBox3.Location = new System.Drawing.Point(6, 168);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(200, 60);
			this.groupBox3.TabIndex = 7;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "MITER BEAMS";
			this.groupBox3.UseCompatibleTextRendering = true;
			// 
			// checkMiterEnd
			// 
			this.checkMiterEnd.Location = new System.Drawing.Point(108, 19);
			this.checkMiterEnd.Name = "checkMiterEnd";
			this.checkMiterEnd.Size = new System.Drawing.Size(86, 32);
			this.checkMiterEnd.TabIndex = 2;
			this.checkMiterEnd.Text = "End Point";
			this.checkMiterEnd.UseCompatibleTextRendering = true;
			this.checkMiterEnd.UseVisualStyleBackColor = true;
			// 
			// checkMiterStart
			// 
			this.checkMiterStart.Location = new System.Drawing.Point(6, 19);
			this.checkMiterStart.Name = "checkMiterStart";
			this.checkMiterStart.Size = new System.Drawing.Size(97, 32);
			this.checkMiterStart.TabIndex = 3;
			this.checkMiterStart.Text = "Start Point";
			this.checkMiterStart.UseCompatibleTextRendering = true;
			this.checkMiterStart.UseVisualStyleBackColor = true;
			// 
			// okBtn
			// 
			this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okBtn.Location = new System.Drawing.Point(6, 245);
			this.okBtn.Name = "okBtn";
			this.okBtn.Size = new System.Drawing.Size(75, 23);
			this.okBtn.TabIndex = 8;
			this.okBtn.Text = "OK";
			this.okBtn.UseCompatibleTextRendering = true;
			this.okBtn.UseVisualStyleBackColor = true;
			this.okBtn.Click += new System.EventHandler(this.OkBtnClick);
			// 
			// cancelBtn
			// 
			this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelBtn.Location = new System.Drawing.Point(131, 245);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(75, 23);
			this.cancelBtn.TabIndex = 9;
			this.cancelBtn.Text = "Cancel";
			this.cancelBtn.UseCompatibleTextRendering = true;
			this.cancelBtn.UseVisualStyleBackColor = true;
			// 
			// JoinForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(215, 281);
			this.Controls.Add(this.cancelBtn);
			this.Controls.Add(this.okBtn);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "JoinForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "JoinForm";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.CheckBox checkMiterEnd;
		private System.Windows.Forms.CheckBox checkMiterStart;
		private System.Windows.Forms.Button cancelBtn;
		private System.Windows.Forms.Button okBtn;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox disallowCheckEnd;
		private System.Windows.Forms.CheckBox disallowCheckStart;
		private System.Windows.Forms.CheckBox allowCheckEnd;
		private System.Windows.Forms.CheckBox allowCheckStart;
		

	}
}
