namespace Zabavnov.WFMVVM
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    ///     The implementation of <see cref="INotifiable{T}" /> with simple get/set and raise event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //[DebuggerStepThrough]
    public class SimpleNotifiable<T> : INotifiable<T>
    {
        private readonly object _syncObj;
        private IEqualityComparer<T> _comparer;
        private T _value;

        public SimpleNotifiable(T initialValue, object syncObj = null, IEqualityComparer<T> comparer = null)
        {
            this._value = initialValue;
            this._syncObj = syncObj ?? new object();
            this._comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public IEqualityComparer<T> Comparer
        {
            get
            {
                lock(this._syncObj)
                    return this._comparer;
            }
            set
            {
                lock(this._syncObj)
                    this._comparer = value;
            }
        }

        #region INotifiable<T> Members

        public T Value
        {
            [DebuggerStepThrough]
            get
            {
                lock(this._syncObj)
                    return this._value;
            }
            set
            {
                var old = default(T);
                var notify = false;
                lock(this._syncObj)
                {
                    if(!this._comparer.Equals(this._value, value))
                    {
                        old = this._value;
                        this._value = value;
                        notify = true;
                    }
                }

                if(notify && this.Notify != null)
                    this.Notify(new NotifiableEventArgs<T>(this, old));
            }
        }

        public event Action<NotifiableEventArgs<T>> Notify;

        #endregion

        public override string ToString()
        {
            return string.Format("SimpleNotifiable: {0}", this.Value);
        }
    }
}