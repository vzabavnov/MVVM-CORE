using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System;

    /// <summary>
    /// The implementation of <see cref="IDataProvider{T}"/> as <see cref="WeakReference"/>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class WeakDataProvider<T> : IDataProvider<T>
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly WeakReference<T> _reference;

        /// <summary>
        /// </summary>
        private readonly SimpleNotifiable<DataProviderStatus> _status;

        /// <summary>
        /// </summary>
        private readonly object _syncObj;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="provider">
        /// </param>
        /// <param name="syncObj">
        /// </param>
        public WeakDataProvider(Func<T> provider, object syncObj = null)
        {
            Contract.Requires(provider != null);

            _reference = new WeakReference<T>(provider);
            _syncObj = syncObj ?? new object();
            _status = new SimpleNotifiable<DataProviderStatus>(DataProviderStatus.NotReady, _syncObj);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public T Data
        {
            get
            {
                lock (_syncObj)
                {
                    if (!_reference.IsAlive)
                    {
                        _status.Value = DataProviderStatus.NotReady;
                    }

                    var d = _reference.Target;
                    _status.Value = DataProviderStatus.Ready;
                    return d;
                }
            }
        }

        /// <summary>
        /// </summary>
        public INotifiable<DataProviderStatus> Status => _status;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void Reset()
        {
            lock (_syncObj)
            {
                _reference.Release();
                _status.Value = DataProviderStatus.NotReady;
            }
        }

        #endregion
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_reference != null);
            Contract.Invariant(_status != null);
            Contract.Invariant(_syncObj != null);
        }
    }
}