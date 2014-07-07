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
//  File Name: CheckBoxBinders.cs.
//  Created: 2014/06/02/11:32 AM.
//  Modified: 2014/07/07/4:32 PM.
//  ****************************************************************************

#endregion

#region Usings

using System.Windows.Forms;
using Zabavnov.MVVM;

#endregion

namespace Zabavnov.Windows.Forms.MVVM
{
    public static class CheckBoxBinders
    {
        #region Static Fields

        public static readonly IPropertyBinder<CheckBox, bool?> CheckStateBinder;

        public static readonly IPropertyBinder<CheckBox, bool> CheckedBinder = new PropertyBinder<CheckBox, bool>(
            ConstDef.CHECKED,
            box => box.Checked,
            (box, b) => box.Checked = b,
            (box, action) => box.CheckedChanged += (sender, args) => action());

        private static readonly IDataConverter<CheckState, bool?> _checkStateConverter = new DataConverter<CheckState, bool?>(
            state =>
                {
                    switch(state)
                    {
                        case CheckState.Checked:
                            return true;
                        case CheckState.Unchecked:
                            return false;
                        default:
                            return null;
                    }
                },
            b => b.HasValue ? (b.Value ? CheckState.Checked : CheckState.Unchecked) : CheckState.Indeterminate);

        #endregion

        #region Public Methods and Operators

        public static IBindableProperty<T, bool?> CheckStateProperty<T>(this T ctrl) where T : CheckBox
        {
            return CheckStateBinder.BindTo(ctrl);
        }

        public static IBindableProperty<CheckBox, bool> CheckedProperty(this CheckBox ctrl)
        {
            return CheckedBinder.BindTo(ctrl);
        }

        #endregion

        static CheckBoxBinders()
        {
            CheckStateBinder = new PropertyBinder<CheckBox, CheckState, bool?>(
            "CheckState",
            chk => chk.CheckState,
            (box, state) => box.CheckState = state,
            _checkStateConverter,
            (box, action) => box.CheckStateChanged += (sender, args) => action());
        }
    }
}