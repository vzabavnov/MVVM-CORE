namespace Zabavnov.MVVM
{
    using System;

    /// <summary>
    ///     Declare notifiable contract
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INotifiable<T>
    {
        /// <summary>
        ///     The value to handle
        /// </summary>
        T Value { get; }

        /// <summary>
        ///     this should raise event when <see cref="Value" /> has been changed
        /// </summary>
        event Action<NotifiableEventArgs<T>> Notify;
    }
}