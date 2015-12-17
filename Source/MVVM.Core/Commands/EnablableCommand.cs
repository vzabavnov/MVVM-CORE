using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    public class EnablableCommand<T> : IFuncCommand<T>
    {
        private readonly IFuncCommand<T> _command;
        private readonly T _defaultValue;

        public EnablableCommand(IFuncCommand<T> command, bool allowExecute, T defaultValue)
        {
            Contract.Requires(command != null);

            AllowExecute = allowExecute;
            _command = command;
            _defaultValue = defaultValue;
        }

        #region Implementation of IFuncCommand<out T>

        /// <summary>
        ///     The current status of command
        /// </summary>
        public INotifiable<bool> Status => _command.Status;

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        /// <param name="result"></param>
        public bool TryExecute(out T result)
        {
            if (AllowExecute)
            {
                return _command.TryExecute(out result);
            }
            
            result = _defaultValue;
            return false;
        }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return _command.CanExecute();
        }

        #endregion

        public bool AllowExecute { get; set; }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_command != null);
        }
    }
}