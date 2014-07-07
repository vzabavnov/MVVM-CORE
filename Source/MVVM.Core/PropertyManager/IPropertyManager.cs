using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Zabavnov.MVVM.Contracts;

namespace Zabavnov.MVVM
{
    [ContractClass(typeof(PropertyManagerContact<>))]
    public interface IPropertyManager<T> : IEnumerable<IPropertyInfo>
        where T : INotifyPropertyChanged
    {
        /// <summary>
        /// Get value of property specified by <paramref name="propertyLambda"/>
        /// </summary>
        /// <typeparam name="TProperty">The type of property</typeparam>
        /// <param name="propertyLambda">The expression for property</param>
        /// <returns>The current value.</returns>
        TProperty GetValue<TProperty>(Expression<Func<T, TProperty>> propertyLambda);

        /// <summary>
        /// set value to property specified by <paramref name="propertyLambda"/>
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyLambda"></param>
        /// <param name="value"></param>
        void SetValue<TProperty>(Expression<Func<T, TProperty>> propertyLambda, TProperty value);

        /// <summary>
        /// get property information
        /// </summary>
        /// <typeparam name="TProperty">The type of property</typeparam>
        /// <param name="propertyLambda">The expression for the property</param>
        /// <returns>The <see cref="IPropertyInfo"/></returns>
        IPropertyInfo<TProperty> GetProperty<TProperty>(Expression<Func<T, TProperty>> propertyLambda);
    }
}