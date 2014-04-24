namespace Zabavnov.WFMVVM
{
    using System.ComponentModel;

    /// <summary>
    ///     Defines virtual property for the <typeparamref name="TControl" /> of type <typeparamref name="TProperty" />
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public interface IBindableProperty<out TControl, TProperty> : INotifyPropertyChanged
    {
        /// <summary>
        ///     the property ca be read
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        ///     The property can be write
        /// </summary>
        bool CanWrite { get; }

        /// <summary>
        ///     The control the property binder to
        /// </summary>
        TControl Control { get; }

        /// <summary>
        ///     The value of property
        /// </summary>
        TProperty Value { get; set; }

        /// <summary>
        ///     The property name
        /// </summary>
        string PropertyName { get; }
    }
}