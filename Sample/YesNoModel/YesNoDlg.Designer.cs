namespace MVVMSample.YesNoModel
{
    partial class YesNoDlg
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
            this._yesBtn = new System.Windows.Forms.Button();
            this._noBtn = new System.Windows.Forms.Button();
            this._messageBox = new System.Windows.Forms.TextBox();
            this._hasChangesChk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _yesBtn
            // 
            this._yesBtn.Location = new System.Drawing.Point(315, 121);
            this._yesBtn.Name = "_yesBtn";
            this._yesBtn.Size = new System.Drawing.Size(75, 23);
            this._yesBtn.TabIndex = 0;
            this._yesBtn.Text = "Yes";
            this._yesBtn.UseVisualStyleBackColor = true;
            // 
            // _noBtn
            // 
            this._noBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._noBtn.Location = new System.Drawing.Point(396, 121);
            this._noBtn.Name = "_noBtn";
            this._noBtn.Size = new System.Drawing.Size(75, 23);
            this._noBtn.TabIndex = 0;
            this._noBtn.Text = "No";
            this._noBtn.UseVisualStyleBackColor = true;
            // 
            // _messageBox
            // 
            this._messageBox.Location = new System.Drawing.Point(13, 13);
            this._messageBox.Multiline = true;
            this._messageBox.Name = "_messageBox";
            this._messageBox.ReadOnly = true;
            this._messageBox.Size = new System.Drawing.Size(458, 102);
            this._messageBox.TabIndex = 1;
            // 
            // _hasChangesChk
            // 
            this._hasChangesChk.AutoSize = true;
            this._hasChangesChk.Location = new System.Drawing.Point(13, 125);
            this._hasChangesChk.Name = "_hasChangesChk";
            this._hasChangesChk.Size = new System.Drawing.Size(90, 17);
            this._hasChangesChk.TabIndex = 2;
            this._hasChangesChk.Text = "Has Changes";
            this._hasChangesChk.UseVisualStyleBackColor = true;
            // 
            // YesNoDlg
            // 
            this.AcceptButton = this._yesBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._noBtn;
            this.ClientSize = new System.Drawing.Size(483, 156);
            this.Controls.Add(this._hasChangesChk);
            this.Controls.Add(this._messageBox);
            this.Controls.Add(this._noBtn);
            this.Controls.Add(this._yesBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YesNoDlg";
            this.Text = "Title";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _yesBtn;
        private System.Windows.Forms.Button _noBtn;
        private System.Windows.Forms.TextBox _messageBox;
        private System.Windows.Forms.CheckBox _hasChangesChk;
    }
}