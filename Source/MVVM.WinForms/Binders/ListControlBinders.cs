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
//  File Name: ListControlBinders.cs.
//  Created: 2014/07/07/5:12 PM.
//  Modified: 2014/07/07/5:47 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using Zabavnov.MVVM;

#endregion

namespace Zabavnov.Windows.Forms.MVVM
{
    public static class ListControlBinders
    {
        #region Public Methods and Operators

        public static IBindableProperty<ListControl, IList<TItem>> DataSourceProperty<TItem>(
            this ListControl control,
            string displayMemberName,
            string valueMemberName)
        {
            return new BindableProperty<ListControl, IList<TItem>>(
                control,
                ConstDef.DATA_SOURCE,
                lst => lst.DataSource as IList<TItem>,
                (lst, data) =>
                    {
                        lst.DataSource = data;
                        lst.DisplayMember = displayMemberName;
                        lst.ValueMember = valueMemberName;
                    },
                null);
        }

        public static IBindableProperty<ListControl, IList<TItem>> DataSourceProperty<TItem>(
            this ListControl control,
            Expression<Func<TItem, object>> displayMemberLambda,
            Expression<Func<TItem, object>> valueMemberLambda)
        {
            return DataSourceProperty<TItem>(control, displayMemberLambda.GetMemberInfo().Name, valueMemberLambda.GetMemberInfo().Name);
        }

        public static IBindableProperty<ListControl, TItem> SelectedItemProperty<TItem>(this ListControl control)
        {
            return new BindableProperty<ListControl, TItem>(
                control,
                "SelectedItem",
                ctrl =>
                    {
                        if(ctrl.SelectedIndex != -1)
                        {
                            var lst = ctrl.DataSource as IList<TItem>;
                            if(lst != null)
                                return lst[ctrl.SelectedIndex];
                            var en = ctrl.DataSource as IEnumerable<TItem>;
                            if(en != null)
                                return en.Skip(ctrl.SelectedIndex).First();
                            throw new ArgumentException();
                        }
                        return default(TItem);
                    },
                (ctrl, item) =>
                    {
                        var en = ctrl.DataSource as IEnumerable<TItem>;
                        if(en != null)
                        {
                            ctrl.SelectedIndex = en.IndexOf(t => Equals(t, item));
                        }
                        else
                            throw new ArgumentException();
                    },
                (listControl, action) => listControl.SelectedValueChanged += (sender, args) => action());
        }

        #endregion
    }
}