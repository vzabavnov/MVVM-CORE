namespace MVVMSample
{
    partial class ColorCtrl
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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.colorPanel = new System.Windows.Forms.Panel();
            this.trackB = new System.Windows.Forms.TrackBar();
            this.trackG = new System.Windows.Forms.TrackBar();
            this.trackR = new System.Windows.Forms.TrackBar();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackR)).BeginInit();
            this.SuspendLayout();
            // 
            // colorPanel
            // 
            this.colorPanel.Location = new System.Drawing.Point(231, 9);
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new System.Drawing.Size(58, 59);
            this.colorPanel.TabIndex = 5;
            // 
            // trackB
            // 
            this.trackB.AutoSize = false;
            this.trackB.Location = new System.Drawing.Point(17, 48);
            this.trackB.Maximum = 255;
            this.trackB.Name = "trackB";
            this.trackB.Size = new System.Drawing.Size(208, 20);
            this.trackB.TabIndex = 2;
            this.trackB.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // trackG
            // 
            this.trackG.AutoSize = false;
            this.trackG.Location = new System.Drawing.Point(17, 27);
            this.trackG.Maximum = 255;
            this.trackG.Name = "trackG";
            this.trackG.Size = new System.Drawing.Size(208, 20);
            this.trackG.TabIndex = 3;
            this.trackG.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // trackR
            // 
            this.trackR.AutoSize = false;
            this.trackR.Location = new System.Drawing.Point(17, 7);
            this.trackR.Maximum = 255;
            this.trackR.Name = "trackR";
            this.trackR.Size = new System.Drawing.Size(208, 20);
            this.trackR.TabIndex = 4;
            this.trackR.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 50);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(14, 13);
            label3.TabIndex = 6;
            label3.Text = "B";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 30);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(15, 13);
            label2.TabIndex = 7;
            label2.Text = "G";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(15, 13);
            label1.TabIndex = 8;
            label1.Text = "R";
            // 
            // ColorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.colorPanel);
            this.Controls.Add(this.trackB);
            this.Controls.Add(this.trackG);
            this.Controls.Add(this.trackR);
            this.Name = "ColorCtrl";
            this.Size = new System.Drawing.Size(298, 77);
            ((System.ComponentModel.ISupportInitialize)(this.trackB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel colorPanel;
        private System.Windows.Forms.TrackBar trackB;
        private System.Windows.Forms.TrackBar trackG;
        private System.Windows.Forms.TrackBar trackR;
    }
}
