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
//  File Name: ComboDataSource.cs.
//  Created: 2014/07/07/3:42 PM.
//  Modified: 2014/07/07/4:35 PM.
//  ****************************************************************************

#endregion

#region Usings

using System.Windows.Forms;
using Zabavnov.MVVM;
using Zabavnov.Windows.Forms.MVVM;
using MVVM.Sample.Models.DataSource;

#endregion

namespace MVVMSample
{
    public partial class ComboDataSource : UserControl
    {
        #region Fields

        private readonly IModel _model = new Model();

        #endregion

        #region Constructors and Destructors

        public ComboDataSource()
        {
            InitializeComponent();

            Bindings();
        }

        #endregion

        #region Methods

        private void Bindings()
        {
            _model.BindTo(z => z.AllItems, _allItemsRadioButton.CheckedProperty());
            _model.BindTo(z => z.OddItems, _oddItemsRadioButton.CheckedProperty());
            _model.BindTo(z => z.EvenItems, _evenItemsRadioButton.CheckedProperty());

            _model.BindTo(z => z.Values, _listBox.DataSourceProperty<DataItem>(z=>z.Name, z=>z.ID));
            _model.BindTo(z => z.Values, _comboBox.DataSourceProperty<DataItem>(z=>z.Name, z=>z.ID));

            _model.BindTo(z => z.SelectedItem, _listBox.SelectedItemProperty<DataItem>());
            _model.BindTo(z => z.SelectedItem, _comboBox.SelectedItemProperty<DataItem>());
        }

        #endregion
    }
}