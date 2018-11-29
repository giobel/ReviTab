/*
 * Created by SharpDevelop.
 * User: Giovanni.Brogiolo
 * Date: 28/11/2018
 * Time: 3:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ReviTab
{
    partial class FormPlaceTags
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
            this.btn_acceptChanges = new System.Windows.Forms.Button();
            this.btn_undoChanges = new System.Windows.Forms.Button();
            this.btn_placeTags = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_acceptChanges
            // 
            this.btn_acceptChanges.Location = new System.Drawing.Point(195, 12);
            this.btn_acceptChanges.Name = "btn_acceptChanges";
            this.btn_acceptChanges.Size = new System.Drawing.Size(100, 32);
            this.btn_acceptChanges.TabIndex = 0;
            this.btn_acceptChanges.Text = "Accept Changes";
            this.btn_acceptChanges.UseCompatibleTextRendering = true;
            this.btn_acceptChanges.UseVisualStyleBackColor = true;
            this.btn_acceptChanges.Click += new System.EventHandler(this.ButtonAcceptChangesClick);
            // 
            // btn_undoChanges
            // 
            this.btn_undoChanges.Location = new System.Drawing.Point(195, 63);
            this.btn_undoChanges.Name = "btn_undoChanges";
            this.btn_undoChanges.Size = new System.Drawing.Size(100, 32);
            this.btn_undoChanges.TabIndex = 1;
            this.btn_undoChanges.Text = "Undo Changes";
            this.btn_undoChanges.UseCompatibleTextRendering = true;
            this.btn_undoChanges.UseVisualStyleBackColor = true;
            this.btn_undoChanges.Click += new System.EventHandler(this.ButtonUndoChangesClick);
            // 
            // btn_placeTags
            // 
            this.btn_placeTags.Location = new System.Drawing.Point(34, 12);
            this.btn_placeTags.Name = "btn_placeTags";
            this.btn_placeTags.Size = new System.Drawing.Size(100, 32);
            this.btn_placeTags.TabIndex = 3;
            this.btn_placeTags.Text = "Place Tags";
            this.btn_placeTags.UseCompatibleTextRendering = true;
            this.btn_placeTags.UseVisualStyleBackColor = true;
            this.btn_placeTags.Click += new System.EventHandler(this.ButtonPlaceTagsClick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 40);
            this.label1.TabIndex = 4;
            this.label1.Text = "You are updating the beam tags. Click Accept Changes to update the beam Mark valu" +
            "es.";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 112);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_placeTags);
            this.Controls.Add(this.btn_undoChanges);
            this.Controls.Add(this.btn_acceptChanges);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Place Beam Tags";
            this.TopMost = true;
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_placeTags;
        private System.Windows.Forms.Button btn_undoChanges;
        private System.Windows.Forms.Button btn_acceptChanges;
    }
}
