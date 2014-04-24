namespace Zabavnov.WFMVVM
{
    public class EnablableCommand<T> : IFuncCommand<T>
    {
        private readonly IFuncCommand<T> _command;
        private readonly T _defaultValue;

        public EnablableCommand(IFuncCommand<T> command, bool allowExecute, T defaultValue)
        {
            this.AllowExecute = allowExecute;
            this._command = command;
            this._defaultValue = defaultValue;
        }

        #region Implementation of IFuncCommand<out T>

        /// <summary>
        ///     The current status of command
        /// </summary>
        public INotifiable<bool> Status
        {
            get { return this._command.Status; }
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        public T Execute()
        {
            return this.AllowExecute ? this._command.Execute() : this._defaultValue;
        }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return this._command.CanExecute();
        }

        #endregion

        public bool AllowExecute { get; set; }
    }
}