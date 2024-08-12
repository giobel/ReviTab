﻿


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
            this.checkBoxFlip = new System.Windows.Forms.CheckBox();
            this.textBoxParameter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.prefixLabel = new System.Windows.Forms.Label();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxSectionTypes = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxLong
            // 
            this.checkBoxLong.Checked = true;
            this.checkBoxLong.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLong.Location = new System.Drawing.Point(18, 10);
            this.checkBoxLong.Name = "checkBoxLong";
            this.checkBoxLong.Size = new System.Drawing.Size(91, 26);
            this.checkBoxLong.TabIndex = 0;
            this.checkBoxLong.Text = "Long Section";
            this.checkBoxLong.UseCompatibleTextRendering = true;
            this.checkBoxLong.UseVisualStyleBackColor = true;
            this.checkBoxLong.Click += new System.EventHandler(this.CheckBoxLong_Click);
            // 
            // checkBoxCross
            // 
            this.checkBoxCross.Location = new System.Drawing.Point(127, 12);
            this.checkBoxCross.Name = "checkBoxCross";
            this.checkBoxCross.Size = new System.Drawing.Size(105, 24);
            this.checkBoxCross.TabIndex = 1;
            this.checkBoxCross.Text = "Cross Section";
            this.checkBoxCross.UseCompatibleTextRendering = true;
            this.checkBoxCross.UseVisualStyleBackColor = true;
            this.checkBoxCross.Click += new System.EventHandler(this.CheckBoxCross_Click);
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(72, 373);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(63, 33);
            this.ok_button.TabIndex = 9;
            this.ok_button.Text = "OK";
            this.ok_button.UseCompatibleTextRendering = true;
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.Ok_buttonClick);
            // 
            // cancel_button
            // 
            this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_button.Location = new System.Drawing.Point(193, 373);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(63, 33);
            this.cancel_button.TabIndex = 10;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseCompatibleTextRendering = true;
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // sectionPositionTxt
            // 
            this.sectionPositionTxt.Location = new System.Drawing.Point(12, 143);
            this.sectionPositionTxt.Name = "sectionPositionTxt";
            this.sectionPositionTxt.Size = new System.Drawing.Size(151, 20);
            this.sectionPositionTxt.TabIndex = 5;
            this.sectionPositionTxt.Text = "-10";
            this.sectionPositionTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 14);
            this.label1.TabIndex = 99;
            this.label1.Text = "Section Offset from Line [mm]";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 13);
            this.label2.TabIndex = 99;
            this.label2.Text = "Far Clip Offset from Line [mm]";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // farClipOffsetTxt
            // 
            this.farClipOffsetTxt.Location = new System.Drawing.Point(12, 186);
            this.farClipOffsetTxt.Name = "farClipOffsetTxt";
            this.farClipOffsetTxt.Size = new System.Drawing.Size(151, 20);
            this.farClipOffsetTxt.TabIndex = 6;
            this.farClipOffsetTxt.Text = "500";
            this.farClipOffsetTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // bottomLevelTxt
            // 
            this.bottomLevelTxt.Location = new System.Drawing.Point(240, 143);
            this.bottomLevelTxt.Name = "bottomLevelTxt";
            this.bottomLevelTxt.Size = new System.Drawing.Size(87, 20);
            this.bottomLevelTxt.TabIndex = 7;
            this.bottomLevelTxt.Text = "-1";
            this.bottomLevelTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(238, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 18);
            this.label3.TabIndex = 99;
            this.label3.Text = "Bottom Level* [m]";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(240, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 23);
            this.label4.TabIndex = 99;
            this.label4.Text = "Top Level* [m]";
            this.label4.UseCompatibleTextRendering = true;
            // 
            // topLevelTxt
            // 
            this.topLevelTxt.Location = new System.Drawing.Point(240, 186);
            this.topLevelTxt.Name = "topLevelTxt";
            this.topLevelTxt.Size = new System.Drawing.Size(87, 20);
            this.topLevelTxt.TabIndex = 8;
            this.topLevelTxt.Text = "10";
            this.topLevelTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 253);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(312, 108);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // checkBoxFlip
            // 
            this.checkBoxFlip.AutoSize = true;
            this.checkBoxFlip.Location = new System.Drawing.Point(238, 16);
            this.checkBoxFlip.Name = "checkBoxFlip";
            this.checkBoxFlip.Size = new System.Drawing.Size(87, 17);
            this.checkBoxFlip.TabIndex = 2;
            this.checkBoxFlip.Text = "Flip Direction";
            this.checkBoxFlip.UseVisualStyleBackColor = true;
            // 
            // textBoxParameter
            // 
            this.textBoxParameter.Location = new System.Drawing.Point(12, 98);
            this.textBoxParameter.Name = "textBoxParameter";
            this.textBoxParameter.Size = new System.Drawing.Size(312, 20);
            this.textBoxParameter.TabIndex = 4;
            this.textBoxParameter.Text = "Mark";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(176, 13);
            this.label5.TabIndex = 99;
            this.label5.Text = "Element parameter for section name";
            // 
            // prefixLabel
            // 
            this.prefixLabel.AutoSize = true;
            this.prefixLabel.Location = new System.Drawing.Point(12, 37);
            this.prefixLabel.Name = "prefixLabel";
            this.prefixLabel.Size = new System.Drawing.Size(103, 13);
            this.prefixLabel.TabIndex = 99;
            this.prefixLabel.Text = "Section Name Prefix";
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.Location = new System.Drawing.Point(12, 55);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(103, 20);
            this.textBoxPrefix.TabIndex = 3;
            this.textBoxPrefix.Text = "New Section";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(166, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(161, 13);
            this.label6.TabIndex = 100;
            this.label6.Text = "*Level measured from Datum 0.0";
            // 
            // comboBoxSectionTypes
            // 
            this.comboBoxSectionTypes.FormattingEnabled = true;
            this.comboBoxSectionTypes.Location = new System.Drawing.Point(125, 54);
            this.comboBoxSectionTypes.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxSectionTypes.Name = "comboBoxSectionTypes";
            this.comboBoxSectionTypes.Size = new System.Drawing.Size(203, 21);
            this.comboBoxSectionTypes.TabIndex = 102;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(124, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 101;
            this.label7.Text = "Section Type";
            // 
            // FormCreateSections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 419);
            this.Controls.Add(this.comboBoxSectionTypes);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.prefixLabel);
            this.Controls.Add(this.textBoxPrefix);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxParameter);
            this.Controls.Add(this.checkBoxFlip);
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
            this.Name = "FormCreateSections";
            this.Text = "Create Sections";
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

        private System.Windows.Forms.CheckBox checkBoxFlip;
        private System.Windows.Forms.TextBox textBoxParameter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label prefixLabel;
        private System.Windows.Forms.TextBox textBoxPrefix;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxSectionTypes;
        private System.Windows.Forms.Label label7;
    }
}
