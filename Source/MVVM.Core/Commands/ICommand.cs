namespace Zabavnov.MVVM
{
    /// <summary>
    ///     Defines a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        ///     The current status of command
        /// </summary>
        INotifiable<bool> Status { get; }

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        void Execute();

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        bool CanExecute();
    }

    public interface ICommand<T1> : ICommand
    {
        T1 Arg1 { get; set; }
    }

    public interface ICommand<T1, T2> : ICommand<T1>
    {
        T2 Arg2 { get; set; }
    }
}