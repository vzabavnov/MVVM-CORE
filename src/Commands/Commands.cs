namespace Zabavnov.WFMVVM
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    public class Command : CommandBase, ICommand
    {
        private readonly Func<bool> _canExecuteAction;
        private readonly Action _executeAction;

        public Command(bool canExecute, Action executeAction, Func<bool> canExecuteAction = null) : base(canExecute)
        {
            Contract.Requires(executeAction != null);

            this._executeAction = executeAction;
            this._canExecuteAction = canExecuteAction ?? (() => true);

            this._status.Value = this._canExecuteAction();
        }

        #region Implementation of ICommand

        [DebuggerStepThrough]
        public void Execute()
        {
            if(this.CanExecute())
                this._executeAction();
        }

        [DebuggerStepThrough]
        public bool CanExecute()
        {
            this._status.Value = this._canExecuteAction();
            return this._status.Value;
        }

        #endregion
    }
}