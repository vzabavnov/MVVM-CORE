using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class ParallelDataProvider<T> : IDataProvider<T>
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Func<T> _provider;

        /// <summary>
        /// </summary>
        private readonly ParallelNotifiable<DataProviderStatus> _status;

        /// <summary>
        /// </summary>
        private Task<T> _task;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="provider">
        /// </param>
        /// <param name="syncObj">
        /// </param>
        public ParallelDataProvider(Func<T> provider, object syncObj = null)
        {
            _provider = provider;
            _status = new ParallelNotifiable<DataProviderStatus>(DataProviderStatus.NotReady, syncObj ?? new object());

            Reset();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public T Data
        {
            get
            {
                Contract.Assume(_task != null);

                return _task.Result;
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
            Contract.Assume(_status.Comparer != null);

            _status.Value = DataProviderStatus.NotReady;
            _task = Task.Factory.StartNew(() => Process());
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private T Process()
        {
            _status.Value = DataProviderStatus.Updating;
            T value = _provider();
            _status.Value = DataProviderStatus.Ready;
            return value;
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._status != null);
        }
    }
}