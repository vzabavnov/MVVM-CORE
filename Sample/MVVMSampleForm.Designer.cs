namespace MVVMSample
{
    partial class MVVMSampleForm
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
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.GroupBox groupBox2;
            this._yesNoBtn = new System.Windows.Forms.Button();
            this.colorCtrl1 = new MVVMSample.ColorModel.ColorCtrl();
            this.comboDataSource1 = new MVVMSample.DataSource.ComboDataSource();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.colorCtrl1);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(309, 102);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Color Box";
            // 
            // _yesNoBtn
            // 
            this._yesNoBtn.Location = new System.Drawing.Point(95, 120);
            this._yesNoBtn.Name = "_yesNoBtn";
            this._yesNoBtn.Size = new System.Drawing.Size(139, 57);
            this._yesNoBtn.TabIndex = 2;
            this._yesNoBtn.Text = "Dialog with changes";
            this._yesNoBtn.UseVisualStyleBackColor = true;
            // 
            // colorCtrl1
            // 
            this.colorCtrl1.Location = new System.Drawing.Point(6, 19);
            this.colorCtrl1.Name = "colorCtrl1";
            this.colorCtrl1.Size = new System.Drawing.Size(298, 77);
            this.colorCtrl1.TabIndex = 0;
            // 
            // comboDataSource1
            // 
            this.comboDataSource1.Location = new System.Drawing.Point(12, 18);
            this.comboDataSource1.Name = "comboDataSource1";
            this.comboDataSource1.Size = new System.Drawing.Size(182, 140);
            this.comboDataSource1.TabIndex = 3;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.comboDataSource1);
            groupBox2.Location = new System.Drawing.Point(328, 13);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(200, 164);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Data Source";
            // 
            // MVVMSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 189);
            this.Controls.Add(groupBox2);
            this.Controls.Add(this._yesNoBtn);
            this.Controls.Add(groupBox1);
            this.Name = "MVVMSampleForm";
            this.Text = "MVVM Sample";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _yesNoBtn;
        private ColorModel.ColorCtrl colorCtrl1;
        private DataSource.ComboDataSource comboDataSource1;
    }
}

