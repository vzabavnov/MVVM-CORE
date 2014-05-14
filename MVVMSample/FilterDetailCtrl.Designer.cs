namespace MVVMSample
{
    partial class FilterDetailCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label nameLbl;
            System.Windows.Forms.Label label1;
            this.nameTxt = new System.Windows.Forms.TextBox();
            this.age = new System.Windows.Forms.TextBox();
            nameLbl = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nameLbl
            // 
            nameLbl.AutoSize = true;
            nameLbl.Location = new System.Drawing.Point(3, 10);
            nameLbl.Name = "nameLbl";
            nameLbl.Size = new System.Drawing.Size(35, 13);
            nameLbl.TabIndex = 0;
            nameLbl.Text = "Name";
            // 
            // nameTxt
            // 
            this.nameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTxt.Location = new System.Drawing.Point(6, 26);
            this.nameTxt.Name = "nameTxt";
            this.nameTxt.Size = new System.Drawing.Size(138, 20);
            this.nameTxt.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 53);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(26, 13);
            label1.TabIndex = 2;
            label1.Text = "Age";
            // 
            // age
            // 
            this.age.Location = new System.Drawing.Point(9, 69);
            this.age.Name = "age";
            this.age.Size = new System.Drawing.Size(135, 20);
            this.age.TabIndex = 3;
            // 
            // FilterDetailCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.age);
            this.Controls.Add(label1);
            this.Controls.Add(this.nameTxt);
            this.Controls.Add(nameLbl);
            this.Name = "FilterDetailCtrl";
            this.Size = new System.Drawing.Size(147, 269);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameTxt;
        private System.Windows.Forms.TextBox age;
    }
}
