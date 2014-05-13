namespace Zabavnov.WFMVVM
{
    using System;
    using System.Diagnostics;

    /// <summary>
    ///     Implementation of <see cref="IDataProvider{T}" /> as lazy initialize
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerStepThrough]
    public class LazyDataProvider<T> : IDataProvider<T>
    {
        private readonly Func<T> _provider;
        private readonly SimpleDataProviderStatus _status;
        private readonly object _syncObj;
        private T _data;

        public LazyDataProvider(Func<T> provider, object syncObj = null)
        {
            this._provider = provider;
            this._syncObj = syncObj ?? new object();
            this._status = new SimpleDataProviderStatus(DataProviderStatus.NotReady, this._syncObj);
        }

        #region IDataProvider<T> Members

        public T Data
        {
            get
            {
                lock(this._syncObj)
                {
                    if(this._status.Value == DataProviderStatus.NotReady)
                    {
                        this._data = this._provider();
                        this._status.Value = DataProviderStatus.Ready;
                    }

                    return this._data;
                }
            }
        }
        
        public void Reset()
        {
            this._status.Value = DataProviderStatus.NotReady;
        }

        public INotifiable<DataProviderStatus> Status
        {
            get { return this._status; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("LazyDataProvider: {0}", this.GetProviderStatus());
        }
    }
}