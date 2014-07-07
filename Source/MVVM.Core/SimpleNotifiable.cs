using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
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
            _value = initialValue;
            _syncObj = syncObj ?? new object();
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public IEqualityComparer<T> Comparer
        {
            get
            {
                lock (_syncObj)
                    return _comparer;
            }
            set
            {
                Contract.Requires(value != null);
                lock (_syncObj)
                    _comparer = value;
            }
        }

        #region INotifiable<T> Members

        public T Value
        {
            [DebuggerStepThrough]
            get
            {
                lock (_syncObj)
                    return _value;
            }
            set
            {
                T old = default(T);
                bool notify = false;
                lock (_syncObj)
                {
                    if (!_comparer.Equals(_value, value))
                    {
                        old = _value;
                        _value = value;
                        notify = true;
                    }
                }

                if (notify && Notify != null)
                {
                    Notify(new NotifiableEventArgs<T>(this, old));
                }
            }
        }

        public event Action<NotifiableEventArgs<T>> Notify;

        #endregion

        public override string ToString()
        {
            return string.Format("SimpleNotifiable: {0}", Value);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_comparer != null);
            Contract.Invariant(_syncObj != null);
        }
    }
}