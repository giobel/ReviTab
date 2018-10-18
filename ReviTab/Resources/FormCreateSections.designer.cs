


namespace ReviTab
{
    partial class FormCreateSections
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreateSections));
            this.checkBoxLong = new System.Windows.Forms.CheckBox();
            this.checkBoxCross = new System.Windows.Forms.CheckBox();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.sectionPositionTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.farClipOffsetTxt = new System.Windows.Forms.TextBox();
            this.bottomLevelTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.topLevelTxt = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxLong
            // 
            this.checkBoxLong.Checked = true;
            this.checkBoxLong.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLong.Location = new System.Drawing.Point(64, 10);
            this.checkBoxLong.Name = "checkBoxLong";
            this.checkBoxLong.Size = new System.Drawing.Size(90, 26);
            this.checkBoxLong.TabIndex = 0;
            this.checkBoxLong.Text = "Long Section";
            this.checkBoxLong.UseCompatibleTextRendering = true;
            this.checkBoxLong.UseVisualStyleBackColor = true;
            this.checkBoxLong.Click += new System.EventHandler(this.CheckBoxLong_Click);
            // 
            // checkBoxCross
            // 
            this.checkBoxCross.Location = new System.Drawing.Point(173, 12);
            this.checkBoxCross.Name = "checkBoxCross";
            this.checkBoxCross.Size = new System.Drawing.Size(104, 24);
            this.checkBoxCross.TabIndex = 1;
            this.checkBoxCross.Text = "Cross Section";
            this.checkBoxCross.UseCompatibleTextRendering = true;
            this.checkBoxCross.UseVisualStyleBackColor = true;
            this.checkBoxCross.Click += new System.EventHandler(this.CheckBoxCross_Click);
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(75, 251);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(63, 33);
            this.ok_button.TabIndex = 2;
            this.ok_button.Text = "OK";
            this.ok_button.UseCompatibleTextRendering = true;
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.Ok_buttonClick);
            // 
            // cancel_button
            // 
            this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_button.Location = new System.Drawing.Point(196, 251);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(63, 33);
            this.cancel_button.TabIndex = 3;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseCompatibleTextRendering = true;
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // sectionPositionTxt
            // 
            this.sectionPositionTxt.Location = new System.Drawing.Point(15, 68);
            this.sectionPositionTxt.Name = "sectionPositionTxt";
            this.sectionPositionTxt.Size = new System.Drawing.Size(54, 20);
            this.sectionPositionTxt.TabIndex = 4;
            this.sectionPositionTxt.Text = "-10";
            this.sectionPositionTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "Section Offset from Wall CL [mm]";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Far Clip Offset from Wall CL [mm]";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // farClipOffsetTxt
            // 
            this.farClipOffsetTxt.Location = new System.Drawing.Point(15, 111);
            this.farClipOffsetTxt.Name = "farClipOffsetTxt";
            this.farClipOffsetTxt.Size = new System.Drawing.Size(54, 20);
            this.farClipOffsetTxt.TabIndex = 7;
            this.farClipOffsetTxt.Text = "500";
            this.farClipOffsetTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // bottomLevelTxt
            // 
            this.bottomLevelTxt.Location = new System.Drawing.Point(273, 68);
            this.bottomLevelTxt.Name = "bottomLevelTxt";
            this.bottomLevelTxt.Size = new System.Drawing.Size(54, 20);
            this.bottomLevelTxt.TabIndex = 8;
            this.bottomLevelTxt.Text = "-1";
            this.bottomLevelTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(246, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "Bottom Level [m]";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(246, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 10;
            this.label4.Text = "Top Level [m]";
            this.label4.UseCompatibleTextRendering = true;
            // 
            // topLevelTxt
            // 
            this.topLevelTxt.Location = new System.Drawing.Point(273, 111);
            this.topLevelTxt.Name = "topLevelTxt";
            this.topLevelTxt.Size = new System.Drawing.Size(54, 20);
            this.topLevelTxt.TabIndex = 11;
            this.topLevelTxt.Text = "10";
            this.topLevelTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(15, 137);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(312, 108);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 302);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.topLevelTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bottomLevelTxt);
            this.Controls.Add(this.farClipOffsetTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sectionPositionTxt);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.checkBoxCross);
            this.Controls.Add(this.checkBoxLong);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox topLevelTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox bottomLevelTxt;
        private System.Windows.Forms.TextBox farClipOffsetTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sectionPositionTxt;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.CheckBox checkBoxCross;
        private System.Windows.Forms.CheckBox checkBoxLong;

        void CheckBoxCrossCheckedChanged(object sender, System.EventArgs e)
        {

        }




    }
}
