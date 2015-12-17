using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Zabavnov.MVVM
{
    class BindingOneWayToSource<TModel, TControl, TModelProperty, TControlProperty> : BindingInfoBase<TModel, TControl, TModelProperty, TControlProperty>
        where TModel : class, INotifyPropertyChanged where TControl : class
    {
        public BindingOneWayToSource(TModel model, PropertyInfo propertyInfo, Action<TModel, TModelProperty> modelSetter, 
            IBindableProperty<TControl, TControlProperty> property, IDataConverter<TModelProperty, TControlProperty> converter)
            : base(model, propertyInfo, null, modelSetter, property, converter)
        {
            Contract.Requires(model != null);
            Contract.Requires(propertyInfo != null);
            Contract.Requires(propertyInfo.CanWrite);
            Contract.Requires(property != null);
            Contract.Requires(property.CanRead);
            Contract.Requires(converter != null);

            SetModelValue();
            _property.PropertyChanged += OnControlPropertyChanged;
        }

        #region Overrides of BindingInfoBase<TModel,TControl,TModelProperty,TControlProperty>

        /// <summary/>
        public override BindingMode Direction => BindingMode.OneWayToSource;

        protected override void SetProperty(IBindableProperty<TControl, TControlProperty> property)
        {
            if(!ReferenceEquals(property, _property))
            {
                _property.PropertyChanged -= OnControlPropertyChanged;

                _property = property;

                SetModelValue();

                _property.PropertyChanged += OnControlPropertyChanged;
            }
        }

        protected override void SetModel(TModel model)
        {
            if(!ReferenceEquals(model, _model))
            {
                _model = model;
                if(_model != null)
                    SetModelValue();
            }
        }

        /// <summary/>
        public override void Unbind()
        {
            _property.PropertyChanged -= OnControlPropertyChanged;
        }

        /// <summary/>
        public override void NotifyModelPropertyChanged()
        {
            
        }

        /// <summary/>
        public override void NotifyControlPropertyChanged()
        {
            SetModelValue();
        }

        #endregion
    }
}