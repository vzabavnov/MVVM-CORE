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
//  File Name: ModelBase.cs.
//  Created: 2014/05/30/4:59 PM.
//  Modified: 2015/06/13/10:41 AM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Zabavnov.MVVM
{
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class ModelBase<T> : INotifyPropertyChanged
        where T : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// </summary>
        protected readonly PropertyManager<T> _propertyManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        protected ModelBase()
        {
            _propertyManager = new PropertyManager<T>(info => RaisePropertyChanged(info.Name));
        }

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="names">
        /// </param>
        [DebuggerStepThrough]
        protected internal void RaisePropertyChanged(IDispatcher dispatcher, params string[] names)
        {
            Contract.Requires(dispatcher != null);
            Contract.Requires(names != null);

            var handler = PropertyChanged;
            if(handler != null)
                names.Select(name => new PropertyChangedEventArgs(name)).ForEach(arg => dispatcher.Invoke(() => handler(this, arg)));
        }

        [DebuggerStepThrough]
        protected internal void RaisePropertyChanged(params string[] names)
        {
            Contract.Requires(names != null);

            var handler = PropertyChanged;
            if(handler != null)
                names.Select(name => new PropertyChangedEventArgs(name)).ForEach(arg => handler(this, arg));
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected internal void RaisePropertyChanged(params Expression<Func<T, object>>[] propertyLambdas)
        {
            Contract.Requires(propertyLambdas != null);
            Contract.Requires(propertyLambdas.Length > 0);
            
            RaisePropertyChanged(Dispatcher.DirectDispatcher, propertyLambdas);
        }

        /// <summary>
        /// </summary>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected internal void RaisePropertyChanged(IDispatcher dispatcher, params Expression<Func<T, object>>[] propertyLambdas)
        {
            Contract.Requires(dispatcher != null);
            Contract.Requires(propertyLambdas != null);
            Contract.Requires(propertyLambdas.Length > 0);

            var handler = PropertyChanged;
            if(handler != null)
            {
                foreach(var lambda in propertyLambdas)
                {
                    var args = new PropertyChangedEventArgs(lambda.GetMemberInfo().Name);
                    dispatcher.Invoke(() => handler(this, args));
                }
            }
        }

        /// <summary>
        ///     Raise change event for specified property(s)
        /// </summary>
        /// <param name="propertyLambda"></param>
        /// <param name="propertyLambdas"></param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(Expression<Func<object>> propertyLambda,
                                            params Expression<Func<object>>[] propertyLambdas)
        {
            Contract.Requires(propertyLambda != null);

            RaisePropertyChanged(Dispatcher.DirectDispatcher, propertyLambda, propertyLambdas);
        }

        [DebuggerStepThrough]
        protected void RaisePropertyChanged(IDispatcher dispatcher, Expression<Func<object>> propertyLambda,
                                            params Expression<Func<object>>[] propertyLambdas)
        {
            Contract.Requires(dispatcher != null);
            Contract.Requires(propertyLambda != null);

            var handler = PropertyChanged;
            if (handler != null)
                RaisePropertyChanged(dispatcher,
                    propertyLambdas.AddHead(propertyLambda).Select(z => new PropertyChangedEventArgs(z.GetPropertyName())));
        }

        [DebuggerStepThrough]
        protected void RaisePropertyChanged(IDispatcher dispatcher, IEnumerable<PropertyChangedEventArgs> args)
        {
            Contract.Requires(dispatcher != null);
            Contract.Requires(args != null);

            var handler = this.PropertyChanged;
            if (handler != null)
                args.ForEach(z => dispatcher.Invoke(() => handler(this, z)));
        }

        /// <summary>
        ///     just a wrapper for <see cref="ExpressionExtensions.AttachActionTo(System.Action,System.Linq.Expressions.Expression{System.Func{object}},System.Linq.Expressions.Expression{System.Func{object}}[])" />
        /// </summary>
        /// <param name="actionToAttach"></param>
        /// <param name="propertyLambdas"></param>
        protected void AttachActionTo(Action actionToAttach, params Expression<Func<object>>[] propertyLambdas)
        {
            Contract.Requires(actionToAttach != null);
            Contract.Requires(propertyLambdas != null);
            Contract.Requires(propertyLambdas.Length > 0);

            actionToAttach.AttachActionTo(propertyLambdas);
        }

        /// <summary>
        /// </summary>
        /// <param name="providerToReset">
        /// </param>
        /// <param name="propertyLambdaToRaiseChangesOn">
        /// </param>
        /// <param name="propertyExpressionsToMonitor">
        /// </param>
        /// <typeparam name="TProviderData">
        /// </typeparam>
        [DebuggerStepThrough]
        protected void AttachResetAndRaiseOn<TProviderData>(
            IDataProvider<TProviderData> providerToReset,
            Expression<Func<T, object>> propertyLambdaToRaiseChangesOn,
            params Expression<Func<object>>[] propertyExpressionsToMonitor)
        {
            Contract.Requires(propertyExpressionsToMonitor != null);
            Contract.Requires(propertyExpressionsToMonitor.Length > 0);
            Contract.Requires(providerToReset != null);
            Contract.Requires(propertyLambdaToRaiseChangesOn != null);

            AttachActionTo(() => ResetAndRaise(providerToReset, propertyLambdaToRaiseChangesOn), propertyExpressionsToMonitor);
        }

        /// <summary>
        ///     Check command on property changes
        /// </summary>
        /// <param name="commandToCheck">
        ///     The command to check
        /// </param>
        /// <param name="propertyLambdas">
        ///     The properties to check command on
        /// </param>
        protected void CheckCommandOn(ICommand commandToCheck, params Expression<Func<object>>[] propertyLambdas)
        {
            Contract.Requires(commandToCheck != null);
            Contract.Requires(propertyLambdas != null);
            Contract.Requires(propertyLambdas.Length > 0);

            //check command first
            commandToCheck.CanExecute();

            AttachActionTo(() => commandToCheck.CanExecute(), propertyLambdas);
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyLambdaToRaise">
        /// </param>
        /// <param name="propertyLambdasToMonitor">
        /// </param>
        protected void RaisePropertyChangedOn(
            Expression<Func<T, object>> propertyLambdaToRaise,
            params Expression<Func<object>>[] propertyLambdasToMonitor)
        {
            Contract.Requires(propertyLambdasToMonitor != null);
            Contract.Requires(propertyLambdaToRaise != null);

            ExpressionExtensions.AttachActionTo(() => RaisePropertyChanged(propertyLambdaToRaise), propertyLambdasToMonitor);
        }

        /// <summary>
        /// </summary>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="propertyLambdaToRaise">
        /// </param>
        /// <param name="propertyLambdasToMonitor">
        /// </param>
        protected void RaisePropertyChangedOn(
            IDispatcher dispatcher,
            Expression<Func<T, object>> propertyLambdaToRaise,
            params Expression<Func<object>>[] propertyLambdasToMonitor)
        {
            Contract.Requires(dispatcher != null);
            Contract.Requires(propertyLambdaToRaise != null);
            Contract.Requires(propertyLambdasToMonitor != null);

            ExpressionExtensions.AttachActionTo(() => RaisePropertyChanged(dispatcher, propertyLambdaToRaise), propertyLambdasToMonitor);
        }

        /// <summary>
        ///     Reset provider status if it is in <b>ready</b> status and raise notification
        /// </summary>
        /// <param name="provider">
        ///     The provider to reset
        /// </param>
        /// <param name="propertyLambdas">
        ///     The properties to raise notification on
        /// </param>
        [DebuggerStepThrough]
        protected void ResetAndRaise(IDataProvider provider, params Expression<Func<T, object>>[] propertyLambdas)
        {
            Contract.Requires(provider != null);
            Contract.Requires(propertyLambdas != null);
            Contract.Requires(propertyLambdas.Length > 0);

            ResetAndRaise(provider, Dispatcher.DirectDispatcher, propertyLambdas);
        }

        /// <summary>
        ///     Reset provider status if it is in <b>ready</b> status and raise notification
        /// </summary>
        /// <param name="provider">
        ///     The provider to reset
        /// </param>
        /// <param name="dispatcher">
        ///     the dispatcher
        /// </param>
        /// <param name="propertyLambdas">
        ///     The properties to raise notification on
        /// </param>
        [DebuggerStepThrough]
        protected void ResetAndRaise(IDataProvider provider, IDispatcher dispatcher, params Expression<Func<T, object>>[] propertyLambdas)
        {
            Contract.Requires(provider != null);
            Contract.Requires(dispatcher != null);
            Contract.Requires(propertyLambdas != null);
            Contract.Requires(propertyLambdas.Length > 0);

            if(provider.Status.Value != DataProviderStatus.NotReady)
            {
                provider.Reset();
                RaisePropertyChanged(dispatcher, propertyLambdas);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_propertyManager != null);
        }

        #endregion
    }
}