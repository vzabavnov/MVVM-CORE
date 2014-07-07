namespace Zabavnov.MVVM
{
    /// <summary>
    ///     Define functional command, the command that return value
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IFuncCommand<TResult>
    {
        /// <summary>
        ///     The current status of command
        /// </summary>
        INotifiable<bool> Status { get; }

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        /// <param name="result"></param>
        bool TryExecute(out TResult result);

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        bool CanExecute();
    }

    public interface IFuncCommand<T1, TResult> : IFuncCommand<TResult>
    {
        T1 Arg1 { get; set; }
    }

    public interface IFuncCommand<T1, T2, TResult> : IFuncCommand<T1, TResult>
    {
        T2 Arg2 { get; set; }
    }
}