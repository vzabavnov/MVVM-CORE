using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    public class PropertyConfig<TProperty>: IPropertyConfig<TProperty>
    {
        private readonly IPropertyInfo<TProperty> _propertyInfo;

        public PropertyConfig(IPropertyInfo<TProperty> propertyInfo)
        {
            Contract.Requires(propertyInfo != null);

            _propertyInfo = propertyInfo;
        }

        public IEqualityComparer<TProperty> Comparer
        {
            get
            {
                Contract.Assume(_propertyInfo.Comparer != null);
                return _propertyInfo.Comparer;
            }
            set
            {
                _propertyInfo.Comparer = value;
            }
        }

        public Func<TProperty, bool> Validator 
        { 
            get
            {
                Contract.Assume(_propertyInfo.Validator != null);

                return _propertyInfo.Validator;
            } 
            set
            {
                _propertyInfo.Validator = value;
            }
        }

        public void SetupStorage(TProperty initialValue)
        {
            _propertyInfo.Getter = () => initialValue;
            _propertyInfo.Setter = property => initialValue = property;
        }

        public void SetupStorage(Func<TProperty> getter, Action<TProperty> setter)
        {
            _propertyInfo.Getter = getter;
            _propertyInfo.Setter = setter;
        }

        public Action<TProperty> Setter
        {
            get
            {
                return _propertyInfo.Setter;
            }
        }

        public Func<TProperty> Getter
        {
            get
            {
                return _propertyInfo.Getter;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._propertyInfo != null);
        }
    }
}