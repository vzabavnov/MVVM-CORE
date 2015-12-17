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
//  File Name: PropertyBinder.cs.
//  Created: 2014/06/04/5:09 PM.
//  Modified: 2014/06/09/3:59 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Zabavnov.MVVM
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TControl">
    /// </typeparam>
    /// <typeparam name="TControlProperty">
    /// </typeparam>
    /// <typeparam name="TValueProperty">
    /// </typeparam>
    public class PropertyBinder<TControl, TControlProperty, TValueProperty> : IPropertyBinder<TControl, TValueProperty>
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly IDataConverter<TControlProperty, TValueProperty> _converter;

        /// <summary>
        /// </summary>
        private readonly Action<TControl, Action> _notifyActionSetter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="propertyName">
        /// </param>
        /// <param name="getter">
        /// </param>
        /// <param name="setter">
        /// </param>
        /// <param name="convert">
        /// </param>
        /// <param name="notifyActionSetter">
        /// </param>
        public PropertyBinder(
            string propertyName,
            Func<TControl, TControlProperty> getter,
            Action<TControl, TControlProperty> setter,
            IDataConverter<TControlProperty, TValueProperty> convert,
            Action<TControl, Action> notifyActionSetter = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));
            Contract.Requires(convert != null);

            PropertyName = propertyName;
            Getter = getter;
            Setter = setter;
            _notifyActionSetter = notifyActionSetter;
            _converter = convert;
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyLambda">
        /// </param>
        /// <param name="converter">
        /// </param>
        public PropertyBinder(Expression<Func<TControl, TControlProperty>> propertyLambda, IDataConverter<TControlProperty, TValueProperty> converter)
            : this(propertyLambda, null, converter)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Requires(converter != null);
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyLambda">
        /// </param>
        /// <param name="notifyActionSetter">
        /// </param>
        /// <param name="converter">
        /// </param>
        public PropertyBinder(
            Expression<Func<TControl, TControlProperty>> propertyLambda,
            Action<TControl, Action> notifyActionSetter,
            IDataConverter<TControlProperty, TValueProperty> converter)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Requires(converter != null);

            _notifyActionSetter = notifyActionSetter;
            _converter = converter;

            var memberInfo = propertyLambda.GetMemberInfo();

            PropertyName = memberInfo.Name;
            var info = (PropertyInfo)memberInfo;
            if(info.CanRead)
                Getter = propertyLambda.Compile();

            if(info.CanWrite)
                Setter = propertyLambda.GetPropertySetter();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public bool CanRead => Getter != null;

        /// <summary>
        /// </summary>
        public bool CanWrite => Setter != null;

        /// <summary>
        /// </summary>
        public Func<TControl, TControlProperty> Getter { get; set; }

        /// <summary>
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// </summary>
        public Action<TControl, TControlProperty> Setter { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="control">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public IBindableProperty<T, TValueProperty> BindTo<T>(T control) where T : class, TControl
        {
            Contract.Assume(Getter != null);
            var prop = new BindableProperty<T, TControlProperty, TValueProperty>(control, PropertyName, Getter, Setter, _notifyActionSetter, _converter);
            return prop;
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_converter != null);
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TControl">
    /// </typeparam>
    /// <typeparam name="TProperty">
    /// </typeparam>
    public class PropertyBinder<TControl, TProperty> : PropertyBinder<TControl, TProperty, TProperty>
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="propertyLambda">
        /// </param>
        public PropertyBinder(Expression<Func<TControl, TProperty>> propertyLambda)
            : this(propertyLambda, null)
        {
            Contract.Requires(propertyLambda != null);
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyLambda">
        /// </param>
        /// <param name="notifyActionSetter">
        /// </param>
        public PropertyBinder(Expression<Func<TControl, TProperty>> propertyLambda, Action<TControl, Action> notifyActionSetter)
            : base(propertyLambda, notifyActionSetter, DataConverter<TProperty>.EmptyConverter)
        {
            Contract.Requires(propertyLambda != null);
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyName">
        /// </param>
        /// <param name="getter">
        /// </param>
        /// <param name="setter">
        /// </param>
        /// <param name="notifyActionSetter">
        /// </param>
        public PropertyBinder(
            string propertyName,
            Func<TControl, TProperty> getter,
            Action<TControl, TProperty> setter,
            Action<TControl, Action> notifyActionSetter = null)
            : base(propertyName, getter, setter, DataConverter<TProperty>.EmptyConverter, notifyActionSetter)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));
        }

        #endregion
    }
}