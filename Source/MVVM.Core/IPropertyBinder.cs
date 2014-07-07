using System;
using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    /// <summary>
    ///     declare binder for control of type <typeparamref name="TControl" /> to property of type
    ///     <typeparamref name="TValueProperty" />
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    /// <typeparam name="TValueProperty"></typeparam>
    [ContractClass(typeof(PropertyBinderContract<,>))]
    public interface IPropertyBinder<in TControl, TValueProperty>
    {
        /// <summary>
        ///     The property name
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        ///     Bind control to property and return <see cref="IBindableProperty{TControl,TProperty}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        IBindableProperty<T, TValueProperty> BindTo<T>(T control) where T : class, TControl ;
    }

    [ContractClassFor(typeof(IPropertyBinder<,>))]
    internal abstract class PropertyBinderContract<TControl, TValueProperty> : IPropertyBinder<TControl, TValueProperty>
    {
        #region Implementation of IPropertyBinder<in TControl,TValueProperty>

        /// <summary>
        ///     The property name
        /// </summary>
        public string PropertyName
        {
            get
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Bind control to property and return <see cref="IBindableProperty{TControl,TProperty}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public IBindableProperty<T, TValueProperty> BindTo<T>(T control) where T : class, TControl
        {
            Contract.Requires(control != null);
            throw new NotSupportedException();
        }

        #endregion
    }
}