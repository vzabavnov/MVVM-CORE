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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label resultCount;
            this.colorPanel = new System.Windows.Forms.Panel();
            this.trackB = new System.Windows.Forms.TrackBar();
            this.trackG = new System.Windows.Forms.TrackBar();
            this.trackR = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.findText = new System.Windows.Forms.TextBox();
            this.findLbl = new System.Windows.Forms.Label();
            this.count = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.filterDetailCtrl = new MVVMSample.FilterDetailCtrl();
            this.deleteBtn = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            resultCount = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackR)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(this.colorPanel);
            groupBox1.Controls.Add(this.trackB);
            groupBox1.Controls.Add(this.trackG);
            groupBox1.Controls.Add(this.trackR);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(356, 116);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Color Box";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 87);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(14, 13);
            label3.TabIndex = 2;
            label3.Text = "B";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 55);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(15, 13);
            label2.TabIndex = 2;
            label2.Text = "G";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 23);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(15, 13);
            label1.TabIndex = 2;
            label1.Text = "R";
            // 
            // colorPanel
            // 
            this.colorPanel.Location = new System.Drawing.Point(235, 11);
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new System.Drawing.Size(115, 98);
            this.colorPanel.TabIndex = 1;
            // 
            // trackB
            // 
            this.trackB.AutoSize = false;
            this.trackB.Location = new System.Drawing.Point(20, 83);
            this.trackB.Maximum = 255;
            this.trackB.Name = "trackB";
            this.trackB.Size = new System.Drawing.Size(208, 26);
            this.trackB.TabIndex = 0;
            this.trackB.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // trackG
            // 
            this.trackG.AutoSize = false;
            this.trackG.Location = new System.Drawing.Point(20, 51);
            this.trackG.Maximum = 255;
            this.trackG.Name = "trackG";
            this.trackG.Size = new System.Drawing.Size(208, 26);
            this.trackG.TabIndex = 0;
            this.trackG.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // trackR
            // 
            this.trackR.AutoSize = false;
            this.trackR.Location = new System.Drawing.Point(20, 19);
            this.trackR.Maximum = 255;
            this.trackR.Name = "trackR";
            this.trackR.Size = new System.Drawing.Size(208, 26);
            this.trackR.TabIndex = 0;
            this.trackR.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.deleteBtn);
            this.groupBox2.Controls.Add(this.filterDetailCtrl);
            this.groupBox2.Controls.Add(this.addBtn);
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Controls.Add(this.count);
            this.groupBox2.Controls.Add(resultCount);
            this.groupBox2.Controls.Add(this.findLbl);
            this.groupBox2.Controls.Add(this.findText);
            this.groupBox2.Location = new System.Drawing.Point(12, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(356, 288);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter";
            // 
            // findText
            // 
            this.findText.Location = new System.Drawing.Point(39, 19);
            this.findText.Name = "findText";
            this.findText.Size = new System.Drawing.Size(149, 20);
            this.findText.TabIndex = 0;
            // 
            // findLbl
            // 
            this.findLbl.AutoSize = true;
            this.findLbl.Location = new System.Drawing.Point(6, 22);
            this.findLbl.Name = "findLbl";
            this.findLbl.Size = new System.Drawing.Size(27, 13);
            this.findLbl.TabIndex = 1;
            this.findLbl.Text = "Find";
            // 
            // resultCount
            // 
            resultCount.AutoSize = true;
            resultCount.Location = new System.Drawing.Point(215, 22);
            resultCount.Name = "resultCount";
            resultCount.Size = new System.Drawing.Size(68, 13);
            resultCount.TabIndex = 2;
            resultCount.Text = "Result Count";
            // 
            // count
            // 
            this.count.Location = new System.Drawing.Point(289, 19);
            this.count.Name = "count";
            this.count.ReadOnly = true;
            this.count.Size = new System.Drawing.Size(61, 20);
            this.count.TabIndex = 3;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(9, 45);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(179, 212);
            this.listBox1.TabIndex = 4;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(32, 259);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 5;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            // 
            // filterDetailCtrl
            // 
            this.filterDetailCtrl.Location = new System.Drawing.Point(203, 45);
            this.filterDetailCtrl.Name = "filterDetailCtrl";
            this.filterDetailCtrl.Size = new System.Drawing.Size(147, 212);
            this.filterDetailCtrl.TabIndex = 6;
            // 
            // deleteBtn
            // 
            this.deleteBtn.Location = new System.Drawing.Point(113, 259);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(75, 23);
            this.deleteBtn.TabIndex = 7;
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.UseVisualStyleBackColor = true;
            // 
            // MVVMSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 434);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(groupBox1);
            this.Name = "MVVMSampleForm";
            this.Text = "MVVM Sample";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackR)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel colorPanel;
        private System.Windows.Forms.TrackBar trackB;
        private System.Windows.Forms.TrackBar trackG;
        private System.Windows.Forms.TrackBar trackR;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox count;
        private System.Windows.Forms.Label findLbl;
        private System.Windows.Forms.TextBox findText;
        private System.Windows.Forms.Button deleteBtn;
        private FilterDetailCtrl filterDetailCtrl;
    }
}

