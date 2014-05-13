namespace Zabavnov.WFMVVM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The implementation of <see cref="INotifiable{T}"/> with support o notification in parallel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ParallelNotifiable<T> : INotifiable<T>
    {
        private T _value;
        private readonly object _syncObj;

        private IEqualityComparer<T> _comparer;
        private readonly HashSet<Action<NotifiableEventArgs<T>>> _actions = new HashSet<Action<NotifiableEventArgs<T>>>();

        public ParallelNotifiable(T initialValue, object syncObj = null, IEqualityComparer<T> comparer = null)
        {
            this._value = initialValue;
            this._syncObj = syncObj ?? new object();
            this._comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public T Value
        {
            get
            {
                lock (this._syncObj)
                    return this._value;
            }
            set
            {
                var notify = false;
                NotifiableEventArgs<T> args;
                lock (this._syncObj)
                    if (!this.Comparer.Equals(value, this._value))
                    {
                        args = new NotifiableEventArgs<T>(this,  this._value);
                        this._value = value;
                        notify = true;
                    }
                    else
                    {
                        args = new NotifiableEventArgs<T>(this, default(T));
                    }

                if (notify)
                    this._actions.AsParallel()
                            .ForAll(a => a(args));
            }
        }

        public IEqualityComparer<T> Comparer
        {
            get
            {
                lock (this._syncObj)
                    return this._comparer;
            }
            set
            {
                lock (this._syncObj)
                    this._comparer = value ?? EqualityComparer<T>.Default;
            }
        }

        public event Action<NotifiableEventArgs<T>> Notify
        {
            add
            {
                lock (this._syncObj)
                    if (!this._actions.Contains(value))
                        this._actions.Add(value);
            }
            remove
            {
                lock (this._syncObj)
                    this._actions.Remove(value);
            }
        }
    }
}