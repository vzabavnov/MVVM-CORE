namespace Zabavnov.WFMVVM
{
    using System;

    /// <summary>
    ///     The implementation of <see cref="IDataProvider{T}" /> as <see cref="WeakReference" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeakDataProvider<T> : IDataProvider<T>
    {
        private readonly WeakReference<T> _reference;
        private readonly SimpleNotifiable<DataProviderStatus> _status;
        private readonly object _syncObj;

        public WeakDataProvider(Func<T> provider, object syncObj = null)
        {
            this._reference = new WeakReference<T>(provider);
            this._syncObj = syncObj ?? new object();
            this._status = new SimpleNotifiable<DataProviderStatus>(DataProviderStatus.NotReady, this._syncObj);
        }

        public T Data
        {
            get
            {
                lock (this._syncObj)
                {
                    if (!this._reference.IsAlive)
                        this._status.Value = DataProviderStatus.NotReady;
                    var d = this._reference.Target;
                    this._status.Value = DataProviderStatus.Ready;
                    return d;
                }
            }
        }

        public void Reset()
        {
            lock (this._syncObj)
            {
                this._reference.Release();
                this._status.Value = DataProviderStatus.NotReady;
            }
        }

        public INotifiable<DataProviderStatus> Status
        {
            get { return this._status; }
        }
    }
}