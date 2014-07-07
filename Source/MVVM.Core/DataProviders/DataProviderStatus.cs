namespace Zabavnov.MVVM
{
    /// <summary>
    ///     The status of the <see cref="IDataProvider{T}" />
    /// </summary>
    public enum DataProviderStatus
    {
        /// <summary>
        ///     The data is not ready and access to <see cref="IDataProvider{T}.Data" /> will execute data initialization process
        /// </summary>
        NotReady,

        /// <summary>
        ///     The data in process of initialization
        /// </summary>
        Updating,

        /// <summary>
        ///     Data is ready for use
        /// </summary>
        Ready
    }
}