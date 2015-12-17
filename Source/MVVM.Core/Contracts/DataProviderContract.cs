namespace Zabavnov.MVVM.Contracts
{
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IDataProvider))]
    internal abstract class DataProviderContract : IDataProvider
    {
        #region Implementation of IDataProvider

        /// <summary>
        ///     The status of provider
        /// </summary>
        public INotifiable<DataProviderStatus> Status
        {
            get
            {
                Contract.Ensures(Contract.Result<INotifiable<DataProviderStatus>>() != null);
                throw new System.NotSupportedException();
            }
        }

        /// <summary>
        ///     Notify the provider that data must be refreshed
        /// </summary>
        public void Reset()
        {
            throw new System.NotSupportedException();
        }

        #endregion
    }

    [ContractClassFor(typeof(IDataProvider<>))]
    internal abstract class DataProviderContract<T> : IDataProvider<T>
    {
        #region Implementation of IDataProvider

        /// <summary>
        ///     The status of provider
        /// </summary>
        public INotifiable<DataProviderStatus> Status
        {
            get { throw new System.NotSupportedException(); }
        }

        /// <summary>
        ///     Notify the provider that data must be refreshed
        /// </summary>
        public void Reset()
        {
            throw new System.NotSupportedException();
        }

        #endregion

        #region Implementation of IDataProvider<out T>

        /// <summary>
        ///     The value that <see cref="IDataProvider{T}" /> provides. The data should be always ready
        /// </summary>
        /// <remarks>
        ///     If data is not ready than provider must retrieve data and set status as <see cref="DataProviderStatus.Ready" />
        /// </remarks>
        public T Data
        {
            get { throw new System.NotSupportedException(); }
        }

        #endregion
    }
}