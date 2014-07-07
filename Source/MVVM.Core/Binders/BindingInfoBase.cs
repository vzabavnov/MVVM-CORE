using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Zabavnov.MVVM
{
    internal abstract class BindingInfoBase<TModel, TControl, TModelProperty, TControlProperty> : IBindingInfo<TModel, TControl, TModelProperty, TControlProperty>
        where TModel : class, INotifyPropertyChanged where TControl : class
    {
        protected readonly PropertyInfo _propertyInfo;
        protected readonly Func<TModel, TModelProperty> _modelGetter;
        protected readonly Action<TModel, TModelProperty> _modelSetter;
        protected readonly IDataConverter<TModelProperty, TControlProperty> _converter;
        protected IBindableProperty<TControl, TControlProperty> _property;
        protected TModel _model;
        private bool _disableModelUpdate;
        private bool _disableControlUpdate;

        protected BindingInfoBase(TModel model, PropertyInfo propertyInfo, Func<TModel, TModelProperty> modelGetter,
            Action<TModel, TModelProperty> modelSetter, IBindableProperty<TControl, TControlProperty> property, 
            IDataConverter<TModelProperty, TControlProperty> converter)
        {
            Contract.Requires(model != null);
            Contract.Requires(propertyInfo != null);
            Contract.Requires(property != null);
            Contract.Requires(converter != null);

            _model = model;
            _propertyInfo = propertyInfo;
            _modelGetter = modelGetter;
            _modelSetter = modelSetter;
            _property = property;
            _converter = converter;
            Enabled = true;
        }

        public abstract BindingMode Direction { get; }

        /// <summary>
        /// </summary>
        public bool Enabled { get; set; }

        public string PropertyName
        {
            get
            {
                Contract.Assume(!string.IsNullOrEmpty(PropertyInfo.Name));
                return PropertyInfo.Name;
            }
        }

        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
        }

        public IBindableProperty<TControl, TControlProperty> ControlProperty
        {
            get { return _property; }
            set
            {
                SetProperty(value);
            }
        }

        protected abstract void SetProperty(IBindableProperty<TControl, TControlProperty> property);

        public TModel Model
        {
            get { return _model; }
            set { SetModel(value); }
        }

        protected abstract void SetModel(TModel model);

        public TModelProperty Value
        {
            get
            {
                if(_modelGetter != null)
                    return _modelGetter(_model);
                throw new NotSupportedException();
            }
        }

        public abstract void Unbind();

        public abstract void NotifyModelPropertyChanged();

        public abstract void NotifyControlPropertyChanged();

        protected void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(Enabled && !_disableModelUpdate && e.PropertyName == _propertyInfo.Name)
            {
                var old = _disableModelUpdate;
                _disableModelUpdate = true;
                if(!_disableControlUpdate)
                {
                    _property.Value = _converter.ConvertTo(Value);
                }
                _disableModelUpdate = old;
            }
        }

        protected void OnControlPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(Enabled && !_disableControlUpdate )
            {
                var old = _disableControlUpdate;
                _disableControlUpdate = true;
                if(!_disableModelUpdate && _model != null)
                {
                    _modelSetter(_model, _converter.ConvertFrom(_property.Value));
                }
                _disableControlUpdate = old;
            }
        }

        protected void SetPropertyValue()
        {
            Contract.Assume(_property.CanWrite);
            Contract.Assume(_propertyInfo.CanRead);

            if(!_disableControlUpdate)
            {
                _property.Value = _converter.ConvertTo(Value);
            }
        }

        protected void SetModelValue()
        {
            Contract.Assume(_property.CanRead);
            Contract.Assume(_propertyInfo.CanWrite);
            Contract.Assume(_disableModelUpdate || _modelSetter != null);

            if(!_disableModelUpdate)
            {
                _modelSetter(_model, _converter.ConvertFrom(_property.Value));
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_converter != null);
            Contract.Invariant(_propertyInfo != null);
            Contract.Invariant(_property != null);
            Contract.Invariant(_model != null);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Unbind();
        }
    }
}