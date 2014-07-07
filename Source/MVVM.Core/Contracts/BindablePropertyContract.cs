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
//  File Name: BindablePropertyContract.cs.
//  Created: 2014/06/04/1:07 PM.
//  Modified: 2014/06/06/2:15 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

#endregion

namespace Zabavnov.MVVM.Contracts
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TControl">
    /// </typeparam>
    /// <typeparam name="TProperty">
    /// </typeparam>
    [ContractClassFor(typeof(IBindableProperty<,>))]
    internal abstract class BindablePropertyContract<TControl, TProperty> : IBindableProperty<TControl, TProperty>
        where TControl : class
    {
        #region Public Events

        /// <summary>
        /// </summary>
        public abstract event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///     the property ca be read
        /// </summary>
        public bool CanRead
        {
            get
            {
                Contract.Ensures(Contract.Result<bool>() || CanWrite);
                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     The property can be write
        /// </summary>
        public bool CanWrite
        {
            get
            {
                Contract.Ensures(Contract.Result<bool>() || CanRead);
                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     The control the property binder to
        /// </summary>
        public TControl Control
        {
            get
            {
                Contract.Ensures(Contract.Result<TControl>() != null);
                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     use this value when <see cref="BindingMode" /> is equal to default
        /// </summary>
        public BindingMode DefaultBindingMode
        {
            get
            {
                Contract.Ensures(Contract.Result<BindingMode>() != BindingMode.Default);
                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     The property name
        /// </summary>
        public string PropertyName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     The value of property
        /// </summary>
        public TProperty Value
        {
            get
            {
                Contract.Requires(CanRead);
                throw new NotSupportedException();
            }
            set
            {
                Contract.Requires(CanWrite);
                throw new NotSupportedException();
            }
        }

        #endregion
    }
}