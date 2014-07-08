#region Proprietary  Notice

//  ****************************************************************************
//    Copyright 2014 Vadim Zabavnov
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
//  ****************************************************************************
//  File Name: ColorCtrl.cs.
//  Created: 2014/06/02/5:54 PM.
//  Modified: 2014/06/09/4:01 PM.
//  ****************************************************************************

#endregion

#region Usings

using System.Windows.Forms;
using MVVM.Sample.Models;
using MVVM.Sample.Models.ColorModel;
using Zabavnov.MVVM;
using Zabavnov.Windows.Forms.MVVM;

#endregion

namespace MVVMSample
{
    /// <summary>
    /// </summary>
    public partial class ColorCtrl : UserControl
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly IColorModel _colorModel = new ColorModel();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public ColorCtrl()
        {
            InitializeComponent();

            BindColorModel();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public IColorModel ColorModel
        {
            get { return _colorModel; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void BindColorModel()
        {
            ColorModel.BindTo(z => z.R, trackR.ValueProperty());
            ColorModel.BindTo(z => z.G, trackG.ValueProperty());
            ColorModel.BindTo(z => z.B, trackB.ValueProperty());
            ColorModel.BindTo(z => z.Color, colorPanel.BackColorProperty(), 
                new DataConverter<Color, System.Drawing.Color>(
                    color => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B),
                    color => Color.FromArgb(color.R,color.G, color.B)), BindingMode.Default);
        }

        #endregion
    }
}