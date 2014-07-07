using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Zabavnov.MVVM.Contracts
{
    [ContractClassFor(typeof(IPropertyManager<>))]
    abstract class PropertyManagerContact<T> : IPropertyManager<T>
        where T : INotifyPropertyChanged
    {
        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IPropertyInfo> GetEnumerator()
        {
            throw new NotSupportedException();
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

        #region Implementation of IPropertyManager<T>

        /// <summary>
        /// Get value of property specified by <paramref name="propertyLambda"/>
        /// </summary>
        /// <typeparam name="TProperty">The type of property</typeparam>
        /// <param name="propertyLambda">The expression for property</param>
        /// <returns>The current value.</returns>
        public TProperty GetValue<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Requires(propertyLambda.Body.NodeType == ExpressionType.MemberAccess);
            throw new NotSupportedException();
        }

        /// <summary>
        /// set value to property specified by <paramref name="propertyLambda"/>
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyLambda"></param>
        /// <param name="value"></param>
        public void SetValue<TProperty>(Expression<Func<T, TProperty>> propertyLambda, TProperty value)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Requires(propertyLambda.Body.NodeType == ExpressionType.MemberAccess);

            throw new NotSupportedException();
        }

        /// <summary>
        /// get property information
        /// </summary>
        /// <typeparam name="TProperty">The type of property</typeparam>
        /// <param name="propertyLambda">The expression for the property</param>
        /// <returns>The <see cref="IPropertyInfo"/></returns>
        public IPropertyInfo<TProperty> GetProperty<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            Contract.Ensures(Contract.Result<IPropertyInfo<TProperty>>() != null);

            throw new NotSupportedException();
        }

        #endregion
    }
}