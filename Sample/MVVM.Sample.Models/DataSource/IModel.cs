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
//  File Name: IModel.cs.
//  Created: 2014/07/07/4:07 PM.
//  Modified: 2014/07/07/4:09 PM.
//  ****************************************************************************

#endregion

#region Usings

using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace MVVM.Sample.Models.DataSource
{
    public enum Filter
    {
        Unselected,
        All,
        Even,
        Odd
    }

    public interface IModel : INotifyPropertyChanged
    {
        #region Public Properties

        bool AllItems { get; set; }

        bool EvenItems { get; set; }

        bool OddItems { get; set; }

        DataItem SelectedItem { get; set; }

        IList<DataItem> Values { get; }

        Filter Filter { get; set; }

        #endregion
    }
}