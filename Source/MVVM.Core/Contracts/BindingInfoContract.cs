using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Zabavnov.MVVM.Contracts
{
    /// <summary>
    /// </summary>
    [ContractClassFor(typeof(IBindingInfo))]
    internal abstract class BindingInfoContract : IBindingInfo
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public BindingMode Direction
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public bool Enabled
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public PropertyInfo PropertyInfo
        {
            get
            {
                Contract.Ensures(Contract.Result<PropertyInfo>() != null);
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public string PropertyName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public void NotifyControlPropertyChanged()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public void NotifyModelPropertyChanged()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public void Unbind()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    [ContractClassFor(typeof(IBindingInfo<,,,>))]
    internal abstract class BindingInfoContract<TModel, TControl, TModelProperty, TControlProperty>: 
        IBindingInfo<TModel, TControl, TModelProperty, TControlProperty>
        where TModel : class, INotifyPropertyChanged
        where TControl : class
    {

        #region Implementation of IBindingInfo<TModel,TControl,out TModelProperty,TControlProperty>

        /// <summary>
        /// </summary>
        public IBindableProperty<TControl, TControlProperty> ControlProperty
        {
            get
            {
                Contract.Ensures(Contract.Result<IBindableProperty<TControl, TControlProperty>>() != null);
                throw new NotSupportedException();
            }
            set
            {
                Contract.Requires(value != null);
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// </summary>
        public TModel Model
        {
            get
            {
                Contract.Ensures(Contract.Result<TModel>() != null);
                throw new NotSupportedException();
            }
            set
            {
                Contract.Requires(value != null);
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// </summary>
        public TModelProperty Value
        {
            get { throw new NotSupportedException(); }
        }

        #endregion

        #region Implementation of IBindingInfo

        /// <summary>
        /// </summary>
        public BindingMode Direction
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// </summary>
        public bool Enabled
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// </summary>
        public string PropertyName
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// </summary>
        public void NotifyControlPropertyChanged()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// </summary>
        public void NotifyModelPropertyChanged()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// </summary>
        public void Unbind()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}