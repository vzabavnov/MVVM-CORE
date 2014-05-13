namespace Zabavnov.WFMVVM
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    ///     Implementation of <see cref="Data" /> with get/set <see cref="Data" /> and raising when value changed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerStepThrough]
    public class SimpleDataProvider<T> : IDataProvider<T>
    {
        private readonly SimpleDataProviderStatus _status;
        private readonly object _syncObj;
        private IEqualityComparer<T> _comparator;
        private T _data;

        public SimpleDataProvider(T value, ReaderWriterLockSlim syncObj = null, IEqualityComparer<T> comparator = null)
        {
            this._syncObj = syncObj ?? new ReaderWriterLockSlim();
            this._status = new SimpleDataProviderStatus(DataProviderStatus.Ready, this._syncObj);
            this._data = value;
            this._comparator = comparator ?? EqualityComparer<T>.Default;
        }

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
                lock(this._syncObj)
                    return this._comparator;
            }
            set
            {
                lock(this._syncObj)
                    this._comparator = value;
            }
        }

        #region IDataProvider<T> Members

        public T Data
        {
            get
            {
                lock(this._syncObj)
                    return this._data;
            }
            set
            {
                lock(this._syncObj)
                    if(!this.Comparator.Equals(value, this._data))
                    {
                        this._status.Value = DataProviderStatus.NotReady;
                        this._data = value;
                        this._status.Value = DataProviderStatus.Ready;
                    }
            }
        }

        public void Reset()
        {
        }

        public INotifiable<DataProviderStatus> Status
        {
            get { return this._status; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("SimpleDataProvider: {0}", this.GetProviderStatus());
        }
    }
}