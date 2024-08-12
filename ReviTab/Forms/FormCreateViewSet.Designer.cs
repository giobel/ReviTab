namespace ReviTab
{
    partial class FormCreateViewSet
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
            this.ok_btn = new System.Windows.Forms.Button();
            this.cancel_btn = new System.Windows.Forms.Button();
            this.formTBoxViewsetName = new System.Windows.Forms.TextBox();
            this.formTBoxSheetNumbers = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ok_btn
            // 
            this.ok_btn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_btn.Location = new System.Drawing.Point(51, 102);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(75, 23);
            this.ok_btn.TabIndex = 0;
            this.ok_btn.Text = "OK";
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.ok_btn_Click);
            // 
            // cancel_btn
            // 
            this.cancel_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_btn.Location = new System.Drawing.Point(160, 102);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(75, 23);
            this.cancel_btn.TabIndex = 1;
            this.cancel_btn.Text = "Cancel";
            this.cancel_btn.UseVisualStyleBackColor = true;
            // 
            // formTBoxViewsetName
            // 
            this.formTBoxViewsetName.Location = new System.Drawing.Point(12, 26);
            this.formTBoxViewsetName.Name = "formTBoxViewsetName";
            this.formTBoxViewsetName.Size = new System.Drawing.Size(255, 20);
            this.formTBoxViewsetName.TabIndex = 2;
            // 
            // formTBoxSheetNumbers
            // 
            this.formTBoxSheetNumbers.Location = new System.Drawing.Point(12, 71);
            this.formTBoxSheetNumbers.Name = "formTBoxSheetNumbers";
            this.formTBoxSheetNumbers.Size = new System.Drawing.Size(255, 20);
            this.formTBoxSheetNumbers.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Viewset Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(246, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sheet Numbers space separated (eg 101 102 103)";
            // 
            // FormCreateViewSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 137);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.formTBoxSheetNumbers);
            this.Controls.Add(this.formTBoxViewsetName);
            this.Controls.Add(this.cancel_btn);
            this.Controls.Add(this.ok_btn);
            this.Name = "FormCreateViewSet";
            this.Text = "FormCreateViewSet";
            this.Load += new System.EventHandler(this.FormCreateViewSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_btn;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.TextBox formTBoxViewsetName;
        private System.Windows.Forms.TextBox formTBoxSheetNumbers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}