/*
 * Created by SharpDevelop.
 * User: Giovanni.Brogiolo
 * Date: 26/10/2018
 * Time: 2:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ReviTab
{
    partial class FormAddOpening
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
            this.ok_btn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxFamily = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxStart = new System.Windows.Forms.CheckBox();
            this.checkBoxEnd = new System.Windows.Forms.CheckBox();
            this.checkBoxMidPoint = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLoadCategory = new System.Windows.Forms.Button();
            this.btnClearCat = new System.Windows.Forms.Button();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ok_btn
            // 
            this.ok_btn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_btn.Location = new System.Drawing.Point(38, 253);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(90, 33);
            this.ok_btn.TabIndex = 11;
            this.ok_btn.Text = "OK";
            this.ok_btn.UseCompatibleTextRendering = true;
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.Ok_btnClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 207);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(261, 20);
            this.textBox1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Offset";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // comboBoxFamily
            // 
            this.comboBoxFamily.FormattingEnabled = true;
            this.comboBoxFamily.Location = new System.Drawing.Point(13, 75);
            this.comboBoxFamily.Name = "comboBoxFamily";
            this.comboBoxFamily.Size = new System.Drawing.Size(261, 21);
            this.comboBoxFamily.Sorted = true;
            this.comboBoxFamily.TabIndex = 4;
            this.comboBoxFamily.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCategorySelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select Opening Family";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(157, 253);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(86, 33);
            this.btn_cancel.TabIndex = 12;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseCompatibleTextRendering = true;
            this.btn_cancel.UseVisualStyleBackColor = true;
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(12, 120);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(100, 20);
            this.textBoxWidth.TabIndex = 5;
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(173, 120);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(100, 20);
            this.textBoxHeight.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "Width";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(173, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 19);
            this.label4.TabIndex = 10;
            this.label4.Text = "Height";
            this.label4.UseCompatibleTextRendering = true;
            // 
            // checkBoxStart
            // 
            this.checkBoxStart.Location = new System.Drawing.Point(12, 151);
            this.checkBoxStart.Name = "checkBoxStart";
            this.checkBoxStart.Size = new System.Drawing.Size(104, 28);
            this.checkBoxStart.TabIndex = 7;
            this.checkBoxStart.Text = "Start Point";
            this.checkBoxStart.UseCompatibleTextRendering = true;
            this.checkBoxStart.UseVisualStyleBackColor = true;
            this.checkBoxStart.Click += new System.EventHandler(this.CheckBoxStart_Click);
            // 
            // checkBoxEnd
            // 
            this.checkBoxEnd.Location = new System.Drawing.Point(205, 151);
            this.checkBoxEnd.Name = "checkBoxEnd";
            this.checkBoxEnd.Size = new System.Drawing.Size(104, 28);
            this.checkBoxEnd.TabIndex = 9;
            this.checkBoxEnd.Text = "End Point";
            this.checkBoxEnd.UseCompatibleTextRendering = true;
            this.checkBoxEnd.UseVisualStyleBackColor = true;
            this.checkBoxEnd.Click += new System.EventHandler(this.CheckBoxEnd_Click);
            // 
            // checkBoxMidPoint
            // 
            this.checkBoxMidPoint.Checked = true;
            this.checkBoxMidPoint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMidPoint.Location = new System.Drawing.Point(105, 151);
            this.checkBoxMidPoint.Name = "checkBoxMidPoint";
            this.checkBoxMidPoint.Size = new System.Drawing.Size(73, 28);
            this.checkBoxMidPoint.TabIndex = 8;
            this.checkBoxMidPoint.Text = "Mid Point";
            this.checkBoxMidPoint.UseCompatibleTextRendering = true;
            this.checkBoxMidPoint.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Category";
            // 
            // btnLoadCategory
            // 
            this.btnLoadCategory.Location = new System.Drawing.Point(173, 27);
            this.btnLoadCategory.Name = "btnLoadCategory";
            this.btnLoadCategory.Size = new System.Drawing.Size(52, 20);
            this.btnLoadCategory.TabIndex = 2;
            this.btnLoadCategory.Text = "Load";
            this.btnLoadCategory.UseVisualStyleBackColor = true;
            this.btnLoadCategory.Click += new System.EventHandler(this.btnLoadCategory_Click);
            // 
            // btnClearCat
            // 
            this.btnClearCat.Location = new System.Drawing.Point(231, 27);
            this.btnClearCat.Name = "btnClearCat";
            this.btnClearCat.Size = new System.Drawing.Size(46, 20);
            this.btnClearCat.TabIndex = 3;
            this.btnClearCat.Text = "Clear";
            this.btnClearCat.UseVisualStyleBackColor = true;
            this.btnClearCat.Click += new System.EventHandler(this.btnClearCat_Click);
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(12, 26);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(156, 21);
            this.comboBoxCategory.TabIndex = 16;
            // 
            // FormAddOpening
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 300);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.btnClearCat);
            this.Controls.Add(this.btnLoadCategory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBoxMidPoint);
            this.Controls.Add(this.checkBoxEnd);
            this.Controls.Add(this.checkBoxStart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxHeight);
            this.Controls.Add(this.textBoxWidth);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxFamily);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ok_btn);
            this.Name = "FormAddOpening";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Place Instance By Face";
            this.Load += new System.EventHandler(this.Form1Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.CheckBox checkBoxMidPoint;
        private System.Windows.Forms.CheckBox checkBoxEnd;
        private System.Windows.Forms.CheckBox checkBoxStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxFamily;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button ok_btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLoadCategory;
        private System.Windows.Forms.Button btnClearCat;
        private System.Windows.Forms.ComboBox comboBoxCategory;
    }
}
