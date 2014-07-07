#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;
using Zabavnov.MVVM.Contracts;

#endregion

namespace Zabavnov.MVVM
{
    /// <summary>
    /// </summary>
    [ContractClass(typeof(BindingInfoContract))]
    public interface IBindingInfo: IDisposable
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        BindingMode Direction { get; }

        /// <summary>
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// </summary>
        string PropertyName { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        void NotifyControlPropertyChanged();

        /// <summary>
        /// </summary>
        void NotifyModelPropertyChanged();

        /// <summary>
        /// </summary>
        void Unbind();

        #endregion
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TModel">
    /// </typeparam>
    /// <typeparam name="TControl">
    /// </typeparam>
    /// <typeparam name="TModelProperty">
    /// </typeparam>
    /// <typeparam name="TControlProperty">
    /// </typeparam>
    [ContractClass(typeof(BindingInfoContract<,,,>))]
    public interface IBindingInfo<TModel, TControl, out TModelProperty, TControlProperty> : IBindingInfo
        where TModel : class, INotifyPropertyChanged where TControl : class
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        IBindableProperty<TControl, TControlProperty> ControlProperty { get; set; }

        /// <summary>
        /// </summary>
        TModel Model { get; set; }

        /// <summary>
        /// </summary>
        TModelProperty Value { get; }

        #endregion
    }
}