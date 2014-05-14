using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zabavnov.WFMVVM;

namespace MVVMSample
{
    public partial class MVVMSampleForm : Form
    {
        #region Color Model

        private IColorModel _colorModel = new ColorModel();

        private IFilterModel _filterModel = new FilterModel();
        public IColorModel Model
        {
            get
            {
                return _colorModel ?? (_colorModel = new ColorModel());
            }
            set
            {
                _colorModel = value;
            }
        }

        private void BindColorModel()
        {
            // set it to BindingDirection.TwoWay to accept changes from  control to model's property

            Model.BindTo(z => z.R, trackR.ValueProperty(), BindingDirection.TwoWay);
            Model.BindTo(z => z.G, trackG.ValueProperty(), BindingDirection.TwoWay);
            Model.BindTo(z => z.B, trackB.ValueProperty(), BindingDirection.TwoWay);
            Model.BindTo(z => z.Color, colorPanel.BackColorProperty());
        }

        #endregion


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
