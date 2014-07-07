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
//  File Name: IBindableProperty.cs.
//  Created: 2014/05/30/4:59 PM.
//  Modified: 2014/06/06/1:24 PM.
//  ****************************************************************************

#endregion

#region Usings

using System.ComponentModel;
using System.Diagnostics.Contracts;
using Zabavnov.MVVM.Contracts;

#endregion

namespace Zabavnov.MVVM
{
    /// <summary>
    ///     Defines virtual property for the <typeparamref name="TControl" /> of type <typeparamref name="TProperty" />
    /// </summary>
    /// <typeparam name="TControl">
    /// </typeparam>
    /// <typeparam name="TProperty">
    /// </typeparam>
    [ContractClass(typeof(BindablePropertyContract<,>))]
    public interface IBindableProperty<out TControl, TProperty> : INotifyPropertyChanged
        where TControl : class
    {
        #region Public Properties

        /// <summary>
        ///     the property ca be read
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        ///     The property can be write
        /// </summary>
        bool CanWrite { get; }

        /// <summary>
        ///     The control the property binder to
        /// </summary>
        TControl Control { get; }

        /// <summary>
        ///     use this value when <see cref="BindingMode" /> is equal to default
        /// </summary>
        BindingMode DefaultBindingMode { get; }

        /// <summary>
        ///     The property name
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        ///     The value of property
        /// </summary>
        TProperty Value { get; set; }

        #endregion
    }
}