using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The implementation of <see cref="INotifiable{T}"/> with support o notification in parallel
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class ParallelNotifiable<T> : INotifiable<T>
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly HashSet<Action<NotifiableEventArgs<T>>> _actions = new HashSet<Action<NotifiableEventArgs<T>>>();

        /// <summary>
        /// </summary>
        private readonly object _syncObj;

        /// <summary>
        /// </summary>
        private IEqualityComparer<T> _comparer;

        /// <summary>
        /// </summary>
        private T _value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="initialValue">
        /// </param>
        /// <param name="syncObj">
        /// </param>
        /// <param name="comparer">
        /// </param>
        public ParallelNotifiable(T initialValue, object syncObj = null, IEqualityComparer<T> comparer = null)
        {
            _value = initialValue;
            _syncObj = syncObj ?? new object();
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event Action<NotifiableEventArgs<T>> Notify
        {
            add
            {
                lock (_syncObj)
                    if (!_actions.Contains(value))
                    {
                        _actions.Add(value);
                    }
            }

            remove
            {
                lock (_syncObj)
                    _actions.Remove(value);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public IEqualityComparer<T> Comparer
        {
            get
            {
                lock (_syncObj)
                    return _comparer;
            }

            set
            {
                lock (_syncObj)
                    _comparer = value ?? EqualityComparer<T>.Default;
            }
        }

        /// <summary>
        /// </summary>
        public T Value
        {
            get
            {
                lock (_syncObj)
                    return _value;
            }

            set
            {
                bool notify = false;
                NotifiableEventArgs<T> args;
                lock (_syncObj)
                    if (!Comparer.Equals(value, _value))
                    {
                        args = new NotifiableEventArgs<T>(this, _value);
                        _value = value;
                        notify = true;
                    }
                    else
                    {
                        args = new NotifiableEventArgs<T>(this, default(T));
                    }

                if (notify)
                {
                    _actions.AsParallel().ForAll(a => a(args));
                }
            }
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_syncObj != null);
            Contract.Invariant(_actions != null);
        }
    }
}