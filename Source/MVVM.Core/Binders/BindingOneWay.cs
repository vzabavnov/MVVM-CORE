#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
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
    internal class BindingOneWay<TModel, TControl, TModelProperty, TControlProperty> :
        BindingInfoBase<TModel, TControl, TModelProperty, TControlProperty>
        where TModel : class, INotifyPropertyChanged where TControl : class
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="propertyInfo">
        /// </param>
        /// <param name="modelGetter">
        /// </param>
        /// <param name="property">
        /// </param>
        /// <param name="converter">
        /// </param>
        public BindingOneWay(TModel model,
            PropertyInfo propertyInfo, 
            Func<TModel, TModelProperty> modelGetter,
            IBindableProperty<TControl, TControlProperty> property, 
            IDataConverter<TModelProperty, TControlProperty> converter)
            : base(model, propertyInfo, modelGetter, null, property, converter)
        {
            Contract.Requires(model != null);
            Contract.Requires(propertyInfo != null);
            Contract.Requires(propertyInfo.CanRead);
            Contract.Requires(property != null);
            Contract.Requires(property.CanWrite);
            Contract.Requires(converter != null);

            SetPropertyValue();
            _model.PropertyChanged += OnModelPropertyChanged;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public override BindingMode Direction => BindingMode.OneWay;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public override void NotifyControlPropertyChanged()
        {
        }

        /// <summary>
        /// </summary>
        public override void NotifyModelPropertyChanged()
        {
            SetPropertyValue();
        }

        /// <summary>
        /// </summary>
        public override void Unbind()
        {
            _model.PropertyChanged -= OnModelPropertyChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="model">
        /// </param>
        protected override void SetModel(TModel model)
        {
            if(!ReferenceEquals(model, _model))
            {
                if(_model != null)
                    _model.PropertyChanged -= OnModelPropertyChanged;

                _model = model;

                if(_model != null)
                {
                    SetPropertyValue();

                    _model.PropertyChanged += OnModelPropertyChanged;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="property">
        /// </param>
        protected override void SetProperty(IBindableProperty<TControl, TControlProperty> property)
        {
            if(_property != property)
            {
                _property = property;
                SetPropertyValue();
            }
        }

        #endregion
    }
}