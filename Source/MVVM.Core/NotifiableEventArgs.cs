namespace Zabavnov.MVVM
{
    using System;

    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class NotifiableEventArgs<T> : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="notifiable">
        /// </param>
        /// <param name="oldValue">
        /// </param>
        public NotifiableEventArgs(INotifiable<T> notifiable, T oldValue)
        {
            OldValue = oldValue;
            Notifiable = notifiable;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public INotifiable<T> Notifiable { get; private set; }

        /// <summary>
        /// </summary>
        public T OldValue { get; private set; }

        /// <summary>
        /// </summary>
        public T Value
        {
            get
            {
                return Notifiable.Value;
            }
        }

        #endregion
    }
}