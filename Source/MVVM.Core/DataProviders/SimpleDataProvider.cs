// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleDataProvider.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Implementation of <see cref="Data"/> with get/set <see cref="Data"/> and raising when value changed
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [DebuggerStepThrough]
    public class SimpleDataProvider<T> : IDataProvider<T>
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly SimpleDataProviderStatus _status;

        /// <summary>
        /// </summary>
        private readonly object _syncObj;

        /// <summary>
        /// </summary>
        private IEqualityComparer<T> _comparator;

        /// <summary>
        /// </summary>
        private T _data;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="syncObj">
        /// </param>
        /// <param name="comparator">
        /// </param>
        public SimpleDataProvider(T value, ReaderWriterLockSlim syncObj = null, IEqualityComparer<T> comparator = null)
        {
            _syncObj = syncObj ?? new ReaderWriterLockSlim();
            _status = new SimpleDataProviderStatus(DataProviderStatus.Ready, _syncObj);
            _data = value;
            _comparator = comparator ?? EqualityComparer<T>.Default;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The data comparer.
        /// </summary>
        /// <returns>
        ///     The comparer used when value set to the <see cref="Data" /> to check if new value is applied
        /// </returns>
        /// <returns>
        ///     when <b>null</b> is set than <see cref="System.Collections.Generic.EqualityComparer{T}" /> will be used
        /// </returns>
        public IEqualityComparer<T> Comparator
        {
            get
            {
                lock (_syncObj)
                    return _comparator;
            }

            set
            {
                lock (_syncObj)
                    _comparator = value;
            }
        }

        /// <summary>
        /// </summary>
        public T Data
        {
            get
            {
                lock (_syncObj)
                    return _data;
            }

            set
            {
                lock (_syncObj)
                    if (!Comparator.Equals(value, _data))
                    {
                        _status.Value = DataProviderStatus.NotReady;
                        _data = value;
                        _status.Value = DataProviderStatus.Ready;
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
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return string.Format("SimpleDataProvider: {0}", this.GetProviderStatus());
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_status != null);
            Contract.Invariant(_syncObj != null);
        }
    }
}