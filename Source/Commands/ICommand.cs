namespace Zabavnov.WFMVVM
{
    /// <summary>
    ///     defines command with parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommand<in T>
    {
        /// <summary>
        ///     The current status of command
        /// </summary>
        INotifiable<bool> Status { get; }

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        void Execute(T parameter);

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        bool CanExecute(T parameter);
    }
    
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
}