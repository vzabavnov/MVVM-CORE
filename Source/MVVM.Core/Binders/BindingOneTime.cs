using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Zabavnov.MVVM
{
    class BindingOneTime<TModel, TControl, TModelProperty, TControlProperty> : BindingInfoBase<TModel, TControl, TModelProperty, TControlProperty>
        where TModel : class, INotifyPropertyChanged where TControl : class
    {
        public BindingOneTime(TModel model, PropertyInfo propertyInfo, Func<TModel, TModelProperty> modelGetter, 
            IBindableProperty<TControl, TControlProperty> property, 
            IDataConverter<TModelProperty, TControlProperty> converter)
            :base(model, propertyInfo, modelGetter, null, property, converter)
        {
            Contract.Requires(model != null);
            Contract.Requires(propertyInfo != null);
            Contract.Requires(propertyInfo.CanRead);
            Contract.Requires(property != null);
            Contract.Requires(property.CanWrite);
            Contract.Requires(modelGetter != null);
            Contract.Requires(converter != null);

            SetPropertyValue();
        }

        public override BindingMode Direction => BindingMode.OneTime;

        protected override void SetProperty(IBindableProperty<TControl, TControlProperty> property)
        {
            if(_property != property)
            {
                SetPropertyValue();
            }
        }

        protected override void SetModel(TModel model)
        {
            if(!ReferenceEquals(model, _model))
            {
                _model = model;
                if(_model != null)
                    SetPropertyValue();
            }
        }

        public override void Unbind()
        {
           
        }

        public override void NotifyModelPropertyChanged()
        {
            SetPropertyValue();
        }

        public override void NotifyControlPropertyChanged()
        {
            
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_modelGetter != null);
        }
    }
}