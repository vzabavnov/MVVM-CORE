using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Implementation of <see cref="IDataProvider{T}"/> as lazy initialize
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [DebuggerStepThrough]
    public class LazyDataProvider<T> : IDataProvider<T>
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Func<T> _provider;

        /// <summary>
        /// </summary>
        private readonly SimpleDataProviderStatus _status;

        /// <summary>
        /// </summary>
        private readonly object _syncObj;

        /// <summary>
        /// </summary>
        private T _data;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="provider">
        /// </param>
        /// <param name="syncObj">
        /// </param>
        public LazyDataProvider(Func<T> provider, object syncObj = null)
        {
            Contract.Requires(provider != null);

            _provider = provider;
            _syncObj = syncObj ?? new object();
            _status = new SimpleDataProviderStatus(DataProviderStatus.NotReady, _syncObj);
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
                    if (_status.Value == DataProviderStatus.NotReady)
                    {
                        _data = _provider();
                        _status.Value = DataProviderStatus.Ready;
                    }

                    return _data;
                }
            }
        }

        /// <summary>
        /// </summary>
        public INotifiable<DataProviderStatus> Status
        {
            get
            {
                return _status;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void Reset()
        {
            _status.Value = DataProviderStatus.NotReady;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return string.Format("LazyDataProvider: {0}", this.GetProviderStatus());
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_status != null);
            Contract.Invariant(_syncObj != null);
            Contract.Invariant(_provider != null);
        }
    }
}