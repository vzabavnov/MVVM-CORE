using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using Zabavnov.MVVM.Contracts;

    /// <summary>
    ///     declare the data provider.
    /// </summary>
    /// <remarks>
    ///     the <see cref="IDataProvider{T}" /> provide ability to separate data usage and data initialization.
    ///     The goal of <see cref="IDataProvider{T}" /> to provide value in the <see cref="Data" /> field and hide data
    ///     retrieving process.
    ///     When is necessary to refresh/update/reload new value the call of <see cref="IDataProvider.Reset" /> should notify
    ///     underlining provider to necessity to retrieve fresh value
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    [ContractClass(typeof(DataProviderContract<>))]
    public interface IDataProvider<out T> : IDataProvider
    {
        /// <summary>
        ///     The value that <see cref="IDataProvider{T}" /> provides. The data should be always ready
        /// </summary>
        /// <remarks>
        ///     If data is not ready than provider must retrieve data and set status as <see cref="DataProviderStatus.Ready" />
        /// </remarks>
        T Data { get; }
    }

    /// <summary>
    ///     declare the data provider.
    /// </summary>
    /// <remarks>it provides reset and status info for all providers</remarks>
    [ContractClass(typeof(DataProviderContract))]
    public interface IDataProvider
    {
        /// <summary>
        ///     The status of provider
        /// </summary>
        INotifiable<DataProviderStatus> Status { get; }

        /// <summary>
        ///     Notify the provider that data must be refreshed
        /// </summary>
        void Reset();
    }
}