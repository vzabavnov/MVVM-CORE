using System.Windows.Forms;

namespace MVVMSample.YesNoModel
{
    using System;
    using System.ComponentModel;
    using Zabavnov.MVVM;
    using Zabavnov.Windows.Forms.MVVM;

    public partial class YesNoDlg : Form
    {
        readonly IYesNoModel _model = new YesNoModel();

        public IYesNoModel Model 
        { 
            get
            {
                return _model;
            }
        }

        public YesNoDlg()
        {
            InitializeComponent();

            BindingControls();
        }

        private void BindingControls()
        {
            _model.BindTo(z=>z.Title, this.TextProperty());
            _model.BindTo(z=>z.YesCommandText, _yesBtn.TextProperty());
            _model.BindTo(z => z.NoCommandText, _noBtn.TextProperty());
            _model.BindTo(z => z.Message, _messageBox.TextProperty());
            _model.BindTo(z => z.HasChanges, _hasChangesChk.CheckedProperty(), BindingMode.TwoWay);
            _model.BindCommandToClick(z => z.YesCommand, _yesBtn);
            _model.BindCommandToClick(z => z.NoCommand, _noBtn);
            _model.BindCommandToClose(z => z.CloseCommand, this);
        }

        #region Overrides of Form

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Model.Title = "The Yes/No dialog example";
            Model.YesCommandText = "Yes";
            Model.NoCommandText = "No";
            Model.Message = "The form that will close only if \"Has Changes\" is unchecked";
        }

        #endregion
    }
}
