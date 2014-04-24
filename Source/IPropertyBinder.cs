namespace Zabavnov.WFMVVM
{
    /// <summary>
    ///     declare binder for control of type <typeparamref name="TControl" /> to property of type
    ///     <typeparamref name="TValueProperty" />
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    /// <typeparam name="TValueProperty"></typeparam>
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
        IBindableProperty<T, TValueProperty> BindTo<T>(T control) where T : TControl;
    }
}