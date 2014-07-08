namespace MVVMSample
{
    partial class ComboDataSource
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
            this._comboBox = new System.Windows.Forms.ComboBox();
            this._listBox = new System.Windows.Forms.ListBox();
            this._allItemsRadioButton = new System.Windows.Forms.RadioButton();
            this._evenItemsRadioButton = new System.Windows.Forms.RadioButton();
            this._oddItemsRadioButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // _comboBox
            // 
            this._comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._comboBox.FormattingEnabled = true;
            this._comboBox.Location = new System.Drawing.Point(88, 3);
            this._comboBox.Name = "_comboBox";
            this._comboBox.Size = new System.Drawing.Size(143, 21);
            this._comboBox.TabIndex = 1;
            // 
            // _listBox
            // 
            this._listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._listBox.FormattingEnabled = true;
            this._listBox.Location = new System.Drawing.Point(88, 34);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(143, 82);
            this._listBox.TabIndex = 2;
            // 
            // _allItemsRadioButton
            // 
            this._allItemsRadioButton.AutoSize = true;
            this._allItemsRadioButton.Location = new System.Drawing.Point(3, 7);
            this._allItemsRadioButton.Name = "_allItemsRadioButton";
            this._allItemsRadioButton.Size = new System.Drawing.Size(64, 17);
            this._allItemsRadioButton.TabIndex = 3;
            this._allItemsRadioButton.TabStop = true;
            this._allItemsRadioButton.Text = "All Items";
            this._allItemsRadioButton.UseVisualStyleBackColor = true;
            // 
            // _evenItemsRadioButton
            // 
            this._evenItemsRadioButton.AutoSize = true;
            this._evenItemsRadioButton.Location = new System.Drawing.Point(3, 30);
            this._evenItemsRadioButton.Name = "_evenItemsRadioButton";
            this._evenItemsRadioButton.Size = new System.Drawing.Size(78, 17);
            this._evenItemsRadioButton.TabIndex = 3;
            this._evenItemsRadioButton.TabStop = true;
            this._evenItemsRadioButton.Text = "Even Items";
            this._evenItemsRadioButton.UseVisualStyleBackColor = true;
            // 
            // _oddItemsRadioButton
            // 
            this._oddItemsRadioButton.AutoSize = true;
            this._oddItemsRadioButton.Location = new System.Drawing.Point(4, 53);
            this._oddItemsRadioButton.Name = "_oddItemsRadioButton";
            this._oddItemsRadioButton.Size = new System.Drawing.Size(73, 17);
            this._oddItemsRadioButton.TabIndex = 3;
            this._oddItemsRadioButton.TabStop = true;
            this._oddItemsRadioButton.Text = "Odd Items";
            this._oddItemsRadioButton.UseVisualStyleBackColor = true;
            // 
            // ComboDataSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._oddItemsRadioButton);
            this.Controls.Add(this._evenItemsRadioButton);
            this.Controls.Add(this._allItemsRadioButton);
            this.Controls.Add(this._listBox);
            this.Controls.Add(this._comboBox);
            this.Name = "ComboDataSource";
            this.Size = new System.Drawing.Size(234, 120);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _comboBox;
        private System.Windows.Forms.ListBox _listBox;
        private System.Windows.Forms.RadioButton _allItemsRadioButton;
        private System.Windows.Forms.RadioButton _evenItemsRadioButton;
        private System.Windows.Forms.RadioButton _oddItemsRadioButton;
    }
}
