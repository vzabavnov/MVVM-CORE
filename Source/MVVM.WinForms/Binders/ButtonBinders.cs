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
//  File Name: ButtonBinders.cs.
//  Created: 2014/06/02/12:51 PM.
//  Modified: 2014/06/13/1:44 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Windows.Forms;
using Zabavnov.MVVM;

#endregion

namespace Zabavnov.Windows.Forms.MVVM
{
    public static class ButtonBinders
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Bind command to click event on <see cref="Button" />
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="model"></param>
        /// <param name="commandSelector"></param>
        /// <param name="control"></param>
        public static void BindCommandToClick<TModel, TControl>(this TModel model, Func<TModel, ICommand> commandSelector, TControl control)
            where TControl : Button where TModel : class
        {
            model.BindCommandTo(commandSelector, control, (btn, action) => btn.Click += (sender, args) => action(), (btn, b) => btn.Enabled = b);
        }

        #endregion
    }
}