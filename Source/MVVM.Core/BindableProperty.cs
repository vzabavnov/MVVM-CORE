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
//  File Name: BindableProperty.cs.
//  Created: 2014/05/30/4:59 PM.
//  Modified: 2014/06/06/2:14 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Zabavnov.MVVM
{
    /// <summary>
    ///     Implements <see cref="IBindableProperty{TControl,TProperty}" /> where type of control property and value property
    ///     is not the same
    /// </summary>
    /// <typeparam name="TControl">
    ///     The control type to bind to
    /// </typeparam>
    /// <typeparam name="TControlProperty">
    ///     The type of control property
    /// </typeparam>
    /// <typeparam name="TValueProperty">
    ///     The type of value property
    /// </typeparam>
    public class BindableProperty<TControl, TControlProperty, TValueProperty> : IBindableProperty<TControl, TValueProperty>
        where TControl : class
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly IDataConverter<TControlProperty, TValueProperty> _converter;

        /// <summary>
        /// </summary>
        private readonly PropertyChangedEventArgs _eventArg;

        /// <summary>
        /// </summary>
        private readonly Func<TControl, TControlProperty> _getter;

        /// <summary>
        /// </summary>
        private readonly Action<TControl, TControlProperty> _setter;

        /// <summary>
        /// </summary>
        private IEqualityComparer<TValueProperty> _comparer;

        /// <summary>
        /// </summary>
        private bool _notificationEnabled = true;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     create new instance of bindable property
        /// </summary>
        /// <param name="control">
        ///     The control to bind to
        /// </param>
        /// <param name="propertyName">
        ///     The name of property. this name will be used to raise event on
        ///     <see cref="INotifyPropertyChanged" />
        /// </param>
        /// <param name="getter">
        ///     The method to get value from control's property
        /// </param>
        /// <param name="setter">
        ///     The method to set value to control's property
        /// </param>
        /// <param name="controlNotificationActionSetter">
        ///     The delegate to setup notification action when control. this parameter is
        ///     optional
        /// </param>
        /// <param name="converter">
        ///     The converter between control's property and value
        /// </param>
        /// <param name="comparer">
        ///     The comparer for
        ///     <typeparam name="TValueProperty"></typeparam>
        /// </param>
        public BindableProperty(
            TControl control,
            string propertyName,
            Func<TControl, TControlProperty> getter,
            Action<TControl, TControlProperty> setter,
            Action<TControl, Action> controlNotificationActionSetter,
            IDataConverter<TControlProperty, TValueProperty> converter,
            IEqualityComparer<TValueProperty> comparer = null)
        {
            Contract.Requires(propertyName != null);
            Contract.Requires(!ReferenceEquals(control, null));
            Contract.Requires(getter != null);
            Contract.Requires(converter != null);


            Control = control;
            _getter = getter;
            _setter = setter;
            _converter = converter;
            _comparer = comparer ?? EqualityComparer<TValueProperty>.Default;
            _eventArg = new PropertyChangedEventArgs(propertyName);

            if(controlNotificationActionSetter != null)
                controlNotificationActionSetter(control, OnControlPropertyChanged);
        }

        /// <param name="control">
        ///     The control to bind to
        /// </param>
        /// <param name="propertyLambda">
        ///     The lambda expression for property to assign
        ///     <see cref="IBindableProperty{TControl,TProperty}" /> to.
        /// </param>
        /// <param name="propertyName">
        ///     The name of property. this name will be used to raise event on <see cref="INotifyPropertyChanged" />.
        ///     If it is <b>null</b> then <paramref name="propertyLambda"/>'s name will be used instead.
        /// </param>
        /// <param name="controlNotificationActionSetter">
        ///     The delegate to setup notification action when control. this parameter is
        ///     optional
        /// </param>
        /// <param name="converter">
        ///     The converter between control's property and value
        /// </param>
        /// <param name="comparer">
        ///     The comparer for
        ///     <typeparam name="TValueProperty"></typeparam>
        /// </param>
        public BindableProperty(
            TControl control,
            Expression<Func<TControl, TControlProperty>> propertyLambda,
            string propertyName,
            Action<TControl, Action> controlNotificationActionSetter,
            IDataConverter<TControlProperty, TValueProperty> converter,
            IEqualityComparer<TValueProperty> comparer = null)
        {
            Contract.Requires(control != null);
            Contract.Requires(propertyLambda != null);
            Contract.Requires(converter != null);

            Control = control;

            var propInfo = (PropertyInfo)propertyLambda.GetMemberInfo();

            if(propertyName == null)
                propertyName = propInfo.Name;

            if(propInfo.CanRead)
                _getter = propertyLambda.Compile();

            if(propInfo.CanWrite)
                _setter = propertyLambda.GetPropertySetter();

            _converter = converter;
            _comparer = comparer ?? EqualityComparer<TValueProperty>.Default;
            _eventArg = new PropertyChangedEventArgs(propertyName);

            if(controlNotificationActionSetter != null)
                controlNotificationActionSetter(control, OnControlPropertyChanged);
        }

        /// <summary>
        ///     create new instance of bindable property
        /// </summary>
        /// <param name="control">
        ///     The control to bind to
        /// </param>
        /// <param name="propertyLambda">
        ///     The lambda expression for property to assign
        ///     <see cref="IBindableProperty{TControl,TProperty}" /> to.
        /// </param>
        /// <param name="controlNotificationActionSetter">
        ///     The delegate to setup notification action when control. this parameter is
        ///     optional
        /// </param>
        /// <param name="converter">
        ///     The converter between control's property and value
        /// </param>
        /// <param name="comparer">
        ///     The comparer for
        ///     <typeparam name="TValueProperty"></typeparam>
        /// </param>
        public BindableProperty(
            TControl control,
            Expression<Func<TControl, TControlProperty>> propertyLambda,
            Action<TControl, Action> controlNotificationActionSetter,
            IDataConverter<TControlProperty, TValueProperty> converter,
            IEqualityComparer<TValueProperty> comparer = null)
            : this(control, propertyLambda, null, controlNotificationActionSetter, converter, comparer)
        {
            Contract.Requires(control != null);
            Contract.Requires(propertyLambda != null);
            Contract.Requires(converter != null);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///     The property of control is readable
        /// </summary>
        [Pure]
        public bool CanRead
        {
            get { return _getter != null; }
        }

        /// <summary>
        ///     The property of control is writable
        /// </summary>
        [Pure]
        public bool CanWrite
        {
            get { return _setter != null; }
        }

        /// <summary>
        /// </summary>
        public IEqualityComparer<TValueProperty> Comparer
        {
            get { return _comparer; }

            set { _comparer = value ?? EqualityComparer<TValueProperty>.Default; }
        }

        /// <summary>
        ///     the control that property bound to
        /// </summary>
        public TControl Control { get; private set; }

        /// <summary>
        ///     use this value when <see cref="BindingMode" /> is equal to default
        /// </summary>
        public BindingMode DefaultBindingMode
        {
            get
            {
                if(CanRead && CanWrite)
                    return BindingMode.TwoWay;
                if(CanRead)
                    return BindingMode.OneWayToSource;
                if(CanWrite)
                    return BindingMode.OneWay;

                throw new Exception("Something wrong is here");
            }
        }

        /// <summary>
        ///     The name of property
        /// </summary>
        public string PropertyName
        {
            get
            {
                Contract.Assume(!string.IsNullOrEmpty(_eventArg.PropertyName));
                return _eventArg.PropertyName;
            }
        }

        /// <summary>
        ///     The value of property. it uses converter to get or set value to the control
        /// </summary>
        public TValueProperty Value
        {
            get { return _converter.ConvertTo(_getter(Control)); }

            set
            {
                Contract.Assume(CanRead);
                Contract.Assume(Comparer != null);

                if(!Comparer.Equals(Value, value))
                {
                    _notificationEnabled = false;
                    _setter(Control, _converter.ConvertFrom(value));
                    _notificationEnabled = true;

                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public virtual void NotifyPropertyChanged()
        {
            if(_notificationEnabled)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if(handler != null)
                    handler(Control, _eventArg);
            }
        }

        #endregion

        #region Methods

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_eventArg != null);
            Contract.Invariant(_getter != null);
            Contract.Invariant(_converter != null);
        }

        private void OnControlPropertyChanged()
        {
            if(_notificationEnabled)
                NotifyPropertyChanged();
        }

        #endregion

        
    }

    /// <summary>
    ///     Implements <see cref="IBindableProperty{TControl,TProperty}" />
    /// </summary>
    /// <typeparam name="TControl">
    /// </typeparam>
    /// <typeparam name="TControlProperty">
    /// </typeparam>
    [DebuggerStepThrough]
    public class BindableProperty<TControl, TControlProperty> : BindableProperty<TControl, TControlProperty, TControlProperty>
        where TControl : class
    {
        #region Constructors and Destructors

        /// <summary>
        ///     create new instance of bindable property
        /// </summary>
        /// <param name="control">
        ///     The control to bind to
        /// </param>
        /// <param name="propertyName">
        ///     The name of property. this name will be used to raise event on
        ///     <see cref="INotifyPropertyChanged" />
        /// </param>
        /// <param name="getter">
        ///     The method to get value from control's property
        /// </param>
        /// <param name="setter">
        ///     The method to set value to control's property
        /// </param>
        /// <param name="controlNotificationActionSetter">
        ///     The delegate to setup notification action when control. this parameter is
        ///     optional
        /// </param>
        /// <param name="comparer">
        ///     The comparer for
        ///     <typeparam name="TControlProperty"></typeparam>
        /// </param>
        public BindableProperty(
            TControl control,
            string propertyName,
            Func<TControl, TControlProperty> getter,
            Action<TControl, TControlProperty> setter,
            Action<TControl, Action> controlNotificationActionSetter,
            IEqualityComparer<TControlProperty> comparer = null)
            : base(control, propertyName, getter, setter, controlNotificationActionSetter, DataConverter<TControlProperty>.EmptyConverter, comparer)
        {
            Contract.Requires(control != null);
            Contract.Requires(!String.IsNullOrEmpty(propertyName));
            Contract.Requires(getter != null);
        }

        /// <summary>
        ///     create new instance of bindable property
        /// </summary>
        /// <param name="control">
        ///     The control to bind to
        /// </param>
        /// <param name="propertyLambda">
        ///     The lambda expression for property to assign
        ///     <see cref="IBindableProperty{TControl,TProperty}" /> to.
        /// </param>
        /// <param name="controlNotificationActionSetter">
        ///     The delegate to setup notification action when control. this parameter is
        ///     optional
        /// </param>
        /// <param name="comparer">
        ///     The comparer for
        ///     <typeparam name="TControlProperty"></typeparam>
        /// </param>
        public BindableProperty(
            TControl control,
            Expression<Func<TControl, TControlProperty>> propertyLambda,
            Action<TControl, Action> controlNotificationActionSetter,
            IEqualityComparer<TControlProperty> comparer = null)
            : base(control, propertyLambda, controlNotificationActionSetter, DataConverter<TControlProperty>.EmptyConverter, comparer)
        {
            Contract.Requires(control != null);
            Contract.Requires(propertyLambda != null);
        }

        #endregion

        
    }
}