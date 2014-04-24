namespace Zabavnov.WFMVVM
{
    using System;
    using System.Threading.Tasks;

    public class ParallelDataProvider<T> : IDataProvider<T>
    {
        private readonly Func<T> _provider;
        private Task<T> _task;
        private readonly ParallelNotifiable<DataProviderStatus> _status;

        private T Process()
        {
            this._status.Value = DataProviderStatus.Updating;
            var value = this._provider();
            this._status.Value = DataProviderStatus.Ready;
            return value;
        }

        public ParallelDataProvider(Func<T> provider, object syncObj = null)
        {
            this._provider = provider;
            this._status = new ParallelNotifiable<DataProviderStatus>(DataProviderStatus.NotReady, syncObj ?? new object());

            this.Reset();
        }

        public T Data
        {
            get { return this._task.Result; }
        }

        public void Reset()
        {
            this._status.Value = DataProviderStatus.NotReady;
            this._task = Task.Factory.StartNew(() => this.Process());
        }

        public INotifiable<DataProviderStatus> Status
        {
            get { return this._status; }
        }
    }
}