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
//  File Name: IYesNoModel.cs.
//  Created: 2014/06/01/3:22 PM.
//  Modified: 2014/06/09/4:01 PM.
//  ****************************************************************************

#endregion

#region Usings

using System.ComponentModel;
using System.Windows.Forms;
using Zabavnov.MVVM;

#endregion

namespace MVVMSample.YesNoModel
{
    public interface IYesNoModel : INotifyPropertyChanged
    {
        #region Public Properties

        IExternalCommand CloseCommand { get; }

        bool HasChanges { get; set; }

        string Message { get; set; }

        ICommand NoCommand { get; }

        string NoCommandText { get; set; }

        DialogResult? Result { get; }

        string Title { get; set; }

        ICommand YesCommand { get; }

        string YesCommandText { get; set; }

        #endregion
    }
}