namespace ReviTab
{
    partial class FormAddMetadata
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tBoxRvtFilePath = new System.Windows.Forms.TextBox();
            this.openBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "PDF folder path";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // tBoxRvtFilePath
            // 
            this.tBoxRvtFilePath.Location = new System.Drawing.Point(12, 42);
            this.tBoxRvtFilePath.Name = "tBoxRvtFilePath";
            this.tBoxRvtFilePath.Size = new System.Drawing.Size(526, 20);
            this.tBoxRvtFilePath.TabIndex = 6;
            this.tBoxRvtFilePath.Text = "C:\\Temp";
            // 
            // openBtn
            // 
            this.openBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.openBtn.Location = new System.Drawing.Point(225, 68);
            this.openBtn.Name = "openBtn";
            this.openBtn.Size = new System.Drawing.Size(75, 23);
            this.openBtn.TabIndex = 5;
            this.openBtn.Text = "OK";
            this.openBtn.UseCompatibleTextRendering = true;
            this.openBtn.UseVisualStyleBackColor = true;
            this.openBtn.Click += new System.EventHandler(this.openBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 108);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tBoxRvtFilePath);
            this.Controls.Add(this.openBtn);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Pdf Metadata";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBoxRvtFilePath;
        private System.Windows.Forms.Button openBtn;


    }
}