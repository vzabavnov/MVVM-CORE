using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MVVMSample
{
    using MVVMSample.ColorModel;
    using MVVMSample.YesNoModel;
    using Zabavnov.MVVM;
    using Zabavnov.Windows.Forms.MVVM;

    public partial class MVVMSampleForm : Form
    {
        #region Color Model

        private readonly IColorModel _colorModel = new ColorModel.ColorModel();

        public IColorModel ColorModel
        {
            get { return _colorModel; }
        }
        #endregion

        readonly ICommand _openDlgCommand = new Command(true, () => new YesNoDlg().ShowDialog());

        private void BindColorModel()
        {
            this.BindCommandToClick(z=>z._openDlgCommand, _yesNoBtn);
        }



        public MVVMSampleForm()
        {
            InitializeComponent();
        }

        #region Overrides of Form

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            ControlBindings();
        }

        #endregion

        private void ControlBindings()
        {
            BindColorModel();
            BindFilterModel();
        }

        private void BindFilterModel()
        {
            
        }
    }
}
