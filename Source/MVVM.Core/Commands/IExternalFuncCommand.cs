namespace Zabavnov.MVVM
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TResult">
    /// </typeparam>
    public interface IExternalFuncCommand<TResult> : ICommand
    {
        /// <summary>
        /// The external action that will be executed
        /// </summary>
        TryFunc<TResult> Action { get; set; }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    /// <typeparam name="TResult">
    /// </typeparam>
    public interface IExternalFuncCommand<T, TResult> : ICommand<T>
    {
        /// <summary>
        /// The external action that will be executed
        /// </summary>
       TryFunc<T, TResult> Action { get; set; }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T1">
    /// </typeparam>
    /// <typeparam name="T2">
    /// </typeparam>
    /// <typeparam name="TResult">
    /// </typeparam>
    public interface IExternalFuncCommand<T1, T2, TResult> : ICommand<T1, T2>
    {
        /// <summary>
        /// The external action that will be executed
        /// </summary>
        TryFunc<T1, T2, TResult> Action { get; set; }
    }
}