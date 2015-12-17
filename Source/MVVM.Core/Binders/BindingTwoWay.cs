using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Zabavnov.MVVM
{
    class BindingTwoWay<TModel, TControl, TModelProperty, TControlProperty> : BindingInfoBase<TModel, TControl, TModelProperty, TControlProperty>
        where TModel : class, INotifyPropertyChanged where TControl : class
    {
        public BindingTwoWay(TModel model, PropertyInfo propertyInfo, Func<TModel, TModelProperty> modelGetter, Action<TModel, TModelProperty> modelSetter, 
            IBindableProperty<TControl, TControlProperty> property, IDataConverter<TModelProperty, TControlProperty> converter)
            : base(model, propertyInfo, modelGetter, modelSetter, property, converter)
        {
            Contract.Requires(property != null);
            Contract.Requires(model != null);
            Contract.Requires(propertyInfo != null);
            Contract.Requires(propertyInfo.CanRead);
            Contract.Requires(property.CanWrite);
            Contract.Requires(propertyInfo.CanWrite);
            
            Contract.Requires(property.CanRead);
            Contract.Requires(modelGetter != null);
            Contract.Requires(modelSetter != null);
            Contract.Requires(converter != null);
            
            SetPropertyValue();
            _property.PropertyChanged += OnControlPropertyChanged;
            _model.PropertyChanged += OnModelPropertyChanged;
        }

        #region Overrides of BindingInfoBase<TModel,TControl,TModelProperty,TControlProperty>

        public override BindingMode Direction => BindingMode.TwoWay;

        protected override void SetProperty(IBindableProperty<TControl, TControlProperty> property)
        {
            if(property != _property)
            {
                _property.PropertyChanged -= OnControlPropertyChanged;

                _property = property;

                SetPropertyValue();

                _property.PropertyChanged += OnControlPropertyChanged;
            }
        }

        protected override void SetModel(TModel model)
        {
            if(!ReferenceEquals(_model, model))
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

        public override void Unbind()
        {
            _model.PropertyChanged -= OnModelPropertyChanged;
            _property.PropertyChanged -= OnControlPropertyChanged;
        }

        public override void NotifyModelPropertyChanged()
        {
            Contract.Assume(_property.CanWrite);
            Contract.Assume(_propertyInfo.CanRead);

            SetPropertyValue();
        }

        public override void NotifyControlPropertyChanged()
        {
            Contract.Assume(_property.CanRead);
            Contract.Assume(_propertyInfo.CanWrite);
            
            SetModelValue();
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_modelGetter != null);
            Contract.Invariant(_modelSetter != null);
        }
    }
}