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
//  File Name: Binder.cs.
//  Created: 2014/06/04/9:52 AM.
//  Modified: 2014/06/06/3:58 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Zabavnov.MVVM
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TModel">
    /// </typeparam>
    /// <typeparam name="TControl">
    /// </typeparam>
    /// <typeparam name="TModelProperty">
    /// </typeparam>
    /// <typeparam name="TControlProperty">
    /// </typeparam>
    public class Binder<TModel, TControl, TModelProperty, TControlProperty>
        where TModel : class, INotifyPropertyChanged where TControl : class
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Func<IDataConverter<TModelProperty, TControlProperty>> _converterProvider;

        /// <summary>
        /// </summary>
        private readonly Func<TModel, TModelProperty> _modelPropertyGetter;

        /// <summary>
        /// </summary>
        private readonly PropertyInfo _modelPropertyInfo;

        /// <summary>
        /// </summary>
        private readonly Action<TModel, TModelProperty> _modelPropertySetter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Create binder for property specified by <paramref name="modelPropertyInfo"/> with provided getter and setter
        /// </summary>
        /// <param name="modelPropertyInfo">The <see cref="PropertyInfo"/> to use as property descriptor</param>
        /// <param name="modelPropertyGetter">The property value getter</param>
        /// <param name="modelPropertySetter">The property value setter</param>
        /// <param name="converterProvider">The converter factory</param>
        public Binder(
            PropertyInfo modelPropertyInfo,
            Func<TModel, TModelProperty> modelPropertyGetter,
            Action<TModel, TModelProperty> modelPropertySetter,
            Func<IDataConverter<TModelProperty, TControlProperty>> converterProvider)
        {
            Contract.Requires(modelPropertyInfo != null);
            Contract.Requires(modelPropertyInfo.PropertyType == typeof(TModelProperty));
            Contract.Requires(converterProvider != null);

            _modelPropertyInfo = modelPropertyInfo;
            _converterProvider = converterProvider;

            if (_modelPropertyInfo.CanWrite)
                _modelPropertySetter = modelPropertySetter;

            if (_modelPropertyInfo.CanRead)
                _modelPropertyGetter = modelPropertyGetter;
        }

        /// <summary>
        /// Create binder for property specified by <paramref name="propertyLambda"/>
        /// </summary>
        /// <param name="propertyLambda"> The lambda expression for property.The property must have <see cref="PropertyInfo.CanRead"/>.</param>
        /// <param name="converterProvider">
        /// </param>
        public Binder(Expression<Func<TModel, TModelProperty>> propertyLambda, 
            Func<IDataConverter<TModelProperty, TControlProperty>> converterProvider)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Requires(converterProvider != null);

            _converterProvider = converterProvider;
            _modelPropertyInfo = (PropertyInfo)propertyLambda.GetMemberInfo();

            Contract.Assert(_modelPropertyInfo.CanRead);

            _modelPropertyGetter = propertyLambda.Compile();

            if(_modelPropertyInfo.CanWrite)
                _modelPropertySetter = propertyLambda.GetPropertySetter();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public PropertyInfo ModelPropertyInfo => this._modelPropertyInfo;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="model">
        /// </param>
        /// <param name="property">
        /// </param>
        /// <param name="direction">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public IBindingInfo<TModel, TControl, TModelProperty, TControlProperty> Bind(
            TModel model,
            IBindableProperty<TControl, TControlProperty> property,
            BindingMode direction)
        {
            Contract.Requires(model != null);
            Contract.Requires(property != null);

            if(direction == BindingMode.Default)
                direction = property.DefaultBindingMode;

            var converter = _converterProvider();

            switch(direction)
            {
                case BindingMode.TwoWay:
                    return new BindingTwoWay<TModel, TControl, TModelProperty, TControlProperty>(
                        model,
                        _modelPropertyInfo,
                        _modelPropertyGetter,
                        _modelPropertySetter,
                        property,
                        converter);
                case BindingMode.OneTime:
                    return new BindingOneTime<TModel, TControl, TModelProperty, TControlProperty>(
                        model,
                        _modelPropertyInfo,
                        _modelPropertyGetter,
                        property,
                        converter);
                case BindingMode.OneWay:
                    return new BindingOneWay<TModel, TControl, TModelProperty, TControlProperty>(
                        model,
                        _modelPropertyInfo,
                        _modelPropertyGetter,
                        property,
                        converter);
                case BindingMode.OneWayToSource:
                    return new BindingOneWayToSource<TModel, TControl, TModelProperty, TControlProperty>(
                        model,
                        _modelPropertyInfo,
                        _modelPropertySetter,
                        property,
                        converter);
            }

            throw new ArgumentException("Unsupported binding direction " + direction);
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._converterProvider != null);
            Contract.Invariant(this._modelPropertyInfo != null);
        }
    }
}