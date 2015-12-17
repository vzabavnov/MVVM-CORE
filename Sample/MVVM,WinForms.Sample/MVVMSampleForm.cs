using System;
using System.Windows.Forms;
using MVVM.Sample.Models.ColorModel;

namespace MVVMSample
{
    using Zabavnov.MVVM;
    using Zabavnov.Windows.Forms.MVVM;

    public partial class MVVMSampleForm : Form
    {
        #region Color Model

        public IColorModel ColorModel { get; } = new ColorModel();

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
