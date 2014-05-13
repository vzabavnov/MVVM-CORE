namespace Zabavnov.WFMVVM
{
    using System.Diagnostics;

    public static class DataProviderExtensions
    {
        [DebuggerStepThrough]
        public static string GetProviderStatus<T>(this IDataProvider<T> provider)
        {
            return string.Format("{0}=>{1}", provider.Status,
                provider.Status.Value == DataProviderStatus.Ready
                    ? provider.Data.ToString()
                    : provider.Status.Value == DataProviderStatus.NotReady ? "(Not Ready)" : "(Updating...)");
        }
    }
}