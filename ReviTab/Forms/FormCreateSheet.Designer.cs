namespace ReviTab.Forms
{
    partial class FormCreateSheet
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
            this.tBoxSheetNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tBoxQuantity = new System.Windows.Forms.TextBox();
            this.ok_btn = new System.Windows.Forms.Button();
            this.cancel_btn = new System.Windows.Forms.Button();
            this.cboxPackage = new System.Windows.Forms.ComboBox();
            this.comboBoxTitleblocks = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tBoxSheetNumber
            // 
            this.tBoxSheetNumber.Location = new System.Drawing.Point(102, 46);
            this.tBoxSheetNumber.Name = "tBoxSheetNumber";
            this.tBoxSheetNumber.Size = new System.Drawing.Size(202, 20);
            this.tBoxSheetNumber.TabIndex = 0;
            this.tBoxSheetNumber.Text = "xxx";
            this.tBoxSheetNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sheet Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Package";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Quantity";
            // 
            // tBoxQuantity
            // 
            this.tBoxQuantity.Location = new System.Drawing.Point(103, 113);
            this.tBoxQuantity.Name = "tBoxQuantity";
            this.tBoxQuantity.Size = new System.Drawing.Size(201, 20);
            this.tBoxQuantity.TabIndex = 4;
            this.tBoxQuantity.Text = "1";
            this.tBoxQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ok_btn
            // 
            this.ok_btn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_btn.Location = new System.Drawing.Point(101, 151);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(69, 22);
            this.ok_btn.TabIndex = 6;
            this.ok_btn.Text = "OK";
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.Ok_btn_Click);
            // 
            // cancel_btn
            // 
            this.cancel_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_btn.Location = new System.Drawing.Point(234, 151);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(69, 22);
            this.cancel_btn.TabIndex = 7;
            this.cancel_btn.Text = "Cancel";
            this.cancel_btn.UseVisualStyleBackColor = true;
            // 
            // cboxPackage
            // 
            this.cboxPackage.FormattingEnabled = true;
            this.cboxPackage.Location = new System.Drawing.Point(102, 79);
            this.cboxPackage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboxPackage.Name = "cboxPackage";
            this.cboxPackage.Size = new System.Drawing.Size(202, 21);
            this.cboxPackage.TabIndex = 8;
            // 
            // comboBoxTitleblocks
            // 
            this.comboBoxTitleblocks.FormattingEnabled = true;
            this.comboBoxTitleblocks.Location = new System.Drawing.Point(101, 11);
            this.comboBoxTitleblocks.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxTitleblocks.Name = "comboBoxTitleblocks";
            this.comboBoxTitleblocks.Size = new System.Drawing.Size(203, 21);
            this.comboBoxTitleblocks.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Titleblock";
            // 
            // FormCreateSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 184);
            this.Controls.Add(this.comboBoxTitleblocks);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboxPackage);
            this.Controls.Add(this.cancel_btn);
            this.Controls.Add(this.ok_btn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tBoxQuantity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tBoxSheetNumber);
            this.Name = "FormCreateSheet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Sheet";
            this.Load += new System.EventHandler(this.FormCreateSheet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tBoxSheetNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBoxQuantity;
        private System.Windows.Forms.Button ok_btn;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.ComboBox cboxPackage;
        private System.Windows.Forms.ComboBox comboBoxTitleblocks;
        private System.Windows.Forms.Label label4;
    }
}