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
//  File Name: BinderExtensions.cs.
//  Created: 2014/05/30/5:13 PM.
//  Modified: 2014/06/06/3:58 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

#endregion

namespace Zabavnov.MVVM
{
    public static class BinderExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Bind command to control
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="model"></param>
        /// <param name="commandSelector"></param>
        /// <param name="control"></param>
        /// <param name="setupEventAction">The action to set up specified action on event for the control</param>
        /// <param name="setupCommandStatusAction">
        ///     The action that called when the command changed its status for setting up the
        ///     status of control
        /// </param>
        public static void BindCommandTo<TModel, TControl>(
            this TModel model,
            Func<TModel, ICommand> commandSelector,
            TControl control,
            Action<TControl, Action> setupEventAction,
            Action<TControl, bool> setupCommandStatusAction)
             where TModel : class
            where TControl : class
        {
            Contract.Requires(model != null);
            Contract.Requires(commandSelector != null);
            Contract.Requires(control != null);
            Contract.Requires(setupEventAction != null);
            Contract.Requires(setupCommandStatusAction != null);

            var cmd = commandSelector(model);
            setupCommandStatusAction(control, cmd.Status.Value);
            cmd.Status.Notify += arg => setupCommandStatusAction(control, arg.Value);
            setupEventAction(control, cmd.Execute);
        }

        [DebuggerStepThrough]
        public static void BindTo<TModel, TControl, TProperty>(
            this TModel model,
            TControl control,
            Expression<Func<TModel, TProperty>> modelPropertyLambda,
            Expression<Func<TControl, TProperty>> controlPropertyLambda,
            BindingMode direction,
            Action<TControl, Action> controlNotificationActionSetter) where TModel : class, INotifyPropertyChanged where TControl : class
        {
            Contract.Requires(model != null);
            Contract.Requires(control != null);
            Contract.Requires(modelPropertyLambda != null);
            Contract.Requires(controlPropertyLambda != null);
            Contract.Requires(controlNotificationActionSetter != null);

            var prop = new BindableProperty<TControl, TProperty>(control, controlPropertyLambda, controlNotificationActionSetter);

            BindTo(model, modelPropertyLambda, prop, direction);
        }

        /// <summary>
        ///     Bind model to control
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TControl">The type of the control to bind model to.</typeparam>
        /// <typeparam name="TProperty">The type of model's property to bind control to</typeparam>
        /// <param name="model">The instance of the model</param>
        /// <param name="propertyLambda">the lambda expression for property of model to bind control to</param>
        /// <param name="property">The <see cref="IBindableProperty{TControl,TProperty}" /> to bind model's property to</param>
        /// <param name="direction">
        ///     The direction of binding. when value is <see cref=" BindingDirection.OneWay" /> only changes from model promoted to
        ///     control.
        ///     If need to accept changes from control to the mode then <see cref="BindingDirection.TwoWay" /> need to be used
        /// </param>
        [DebuggerStepThrough]
        public static void BindTo<TModel, TControl, TProperty>(
            this TModel model,
            Expression<Func<TModel, TProperty>> propertyLambda,
            IBindableProperty<TControl, TProperty> property,
            BindingMode direction = BindingMode.Default) where TModel : class, INotifyPropertyChanged where TControl : class
        {
            Contract.Requires(model != null);
            Contract.Requires(propertyLambda != null);
            Contract.Requires(property != null);
            
            BindTo(model, propertyLambda, property, DataConverter<TProperty>.EmptyConverter, direction);
        }

        /// <summary>
        ///     bind model to control
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TControl"></typeparam>
        /// <typeparam name="TModelProperty"></typeparam>
        /// <typeparam name="TControlProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="propertyLambda"></param>
        /// <param name="property"></param>
        /// <param name="converter"></param>
        /// <param name="direction"></param>
        [DebuggerStepThrough]
        public static IBindingInfo<TModel, TControl, TModelProperty, TControlProperty> BindTo<TModel, TControl, TModelProperty, TControlProperty>(
            this TModel model,
            Expression<Func<TModel, TModelProperty>> propertyLambda,
            IBindableProperty<TControl, TControlProperty> property,
            IDataConverter<TModelProperty, TControlProperty> converter,
            BindingMode direction) where TModel : class, INotifyPropertyChanged where TControl : class
        {
            Contract.Requires(model != null);
            Contract.Requires(propertyLambda != null);
            Contract.Requires(property != null);
            Contract.Requires(converter != null);

            var binder = new Binder<TModel, TControl, TModelProperty, TControlProperty>(propertyLambda, () => converter);
            return binder.Bind(model, property, direction);
        }

        #endregion
    }
}