namespace Zabavnov.WFMVVM
{
    using System;

    public class NotifiableEventArgs<T> : EventArgs
    {
        public NotifiableEventArgs(INotifiable<T> notifiable, T oldValue)
        {
            this.OldValue = oldValue;
            this.Notifiable = notifiable;
        }

        public T OldValue { get; private set; }

        public T Value
        {
            get { return this.Notifiable.Value; }
        }

        public INotifiable<T> Notifiable { get; private set; }
    }
}