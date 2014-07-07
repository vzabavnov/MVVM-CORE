using System.Diagnostics.Contracts;
using System.Reflection;

namespace Zabavnov.MVVM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;

    public class PropertyManager<T>: IPropertyManager<T>
        where T : INotifyPropertyChanged
    {
        private readonly Action<IPropertyInfo> _changeNotifyAction;

        readonly Dictionary<string, IPropertyInfo> _properties = new Dictionary<string, IPropertyInfo>();

        public PropertyManager(Action<IPropertyInfo> changeNotifyAction)
        {
            Contract.Requires(changeNotifyAction != null);

            _changeNotifyAction = changeNotifyAction;
        }


        void SetupDefaultStorage<TProperty>(IPropertyInfo<TProperty> propertyInfo)
        {
            Contract.Requires(propertyInfo != null);

            if (propertyInfo.Getter == null || propertyInfo.Setter == null)
            {
                var value = default(TProperty);
                propertyInfo.Getter = () => value;
                propertyInfo.Setter = property => value = property;
            }
        }

        public IPropertyInfo<TProperty> RegisterProperty<TProperty>(
            Expression<Func<T, TProperty>> propertyLambda,
            Action<IPropertyConfig<TProperty>> propertyConfigurationAction)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Requires(propertyConfigurationAction != null);

            var prop = GetProperty(propertyLambda);
            var cfg = new PropertyConfig<TProperty>(prop);
            propertyConfigurationAction(cfg);

            SetupDefaultStorage(prop);

            return prop;
        }

        public TProperty GetValue<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            return GetProperty(propertyLambda).Value;
        }

        public void SetValue<TProperty>(Expression<Func<T, TProperty>> propertyLambda, TProperty value)
        {
            var prop = GetProperty(propertyLambda);
            prop.Value = value;
        }

        public IPropertyInfo<TProperty> GetProperty<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            Contract.Assume(propertyLambda != null);

            var name = propertyLambda.GetMemberInfo().Name;
            IPropertyInfo propertyInfo;
            if (!_properties.TryGetValue(name, out propertyInfo))
            {
                var prop = new PropertyInfo<T, TProperty>(propertyLambda);
                SetupDefaultStorage(prop);
                prop.Changed += _changeNotifyAction;
                propertyInfo = prop;

                _properties.Add(name, propertyInfo);
            }

            return (IPropertyInfo<TProperty>)propertyInfo;
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IPropertyInfo> GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_properties != null);
            Contract.Invariant(_changeNotifyAction != null);
        }
    }
}