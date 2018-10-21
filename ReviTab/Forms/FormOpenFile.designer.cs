/*
 * Created by SharpDevelop.
 * User: Giovanni.Brogiolo
 * Date: 20/10/2018
 * Time: 5:54 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ReviTab
{
    partial class FormOpenFile
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
            if (disposing)
            {
                if (components != null)
                {
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
            this.openBtn = new System.Windows.Forms.Button();
            this.tBoxRvtFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_browse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // openBtn
            // 
            this.openBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.openBtn.Location = new System.Drawing.Point(305, 53);
            this.openBtn.Name = "openBtn";
            this.openBtn.Size = new System.Drawing.Size(75, 23);
            this.openBtn.TabIndex = 1;
            this.openBtn.Text = "OK";
            this.openBtn.UseCompatibleTextRendering = true;
            this.openBtn.UseVisualStyleBackColor = true;
            this.openBtn.Click += new System.EventHandler(this.OpenBtnClick);
            // 
            // tBoxRvtFilePath
            // 
            this.tBoxRvtFilePath.Location = new System.Drawing.Point(12, 25);
            this.tBoxRvtFilePath.Name = "tBoxRvtFilePath";
            this.tBoxRvtFilePath.Size = new System.Drawing.Size(526, 20);
            this.tBoxRvtFilePath.TabIndex = 2;
            this.tBoxRvtFilePath.Text = "C:\\Program Files\\Autodesk\\Revit 2018\\Samples\\rst_basic_sample_project.rvt";
            this.tBoxRvtFilePath.TextChanged += new System.EventHandler(this.TBoxRvtFilePathTextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Revit file path";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // btn_browse
            // 
            this.btn_browse.Location = new System.Drawing.Point(176, 53);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(75, 23);
            this.btn_browse.TabIndex = 4;
            this.btn_browse.Text = "Browse...";
            this.btn_browse.UseCompatibleTextRendering = true;
            this.btn_browse.UseVisualStyleBackColor = true;
            this.btn_browse.Click += new System.EventHandler(this.Btn_browseClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1FileOk);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 88);
            this.Controls.Add(this.btn_browse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tBoxRvtFilePath);
            this.Controls.Add(this.openBtn);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open model in background";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBoxRvtFilePath;
        private System.Windows.Forms.Button openBtn;
    }
}
