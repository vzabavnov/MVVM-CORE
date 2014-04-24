namespace Zabavnov.WFMVVM
{
    /// <summary>
    ///     Define functional command, the command that return value
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IFuncCommand<out TResult>
    {
        /// <summary>
        ///     The current status of command
        /// </summary>
        INotifiable<bool> Status { get; }

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        TResult Execute();

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        bool CanExecute();
    }
}