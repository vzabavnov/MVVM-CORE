namespace Zabavnov.WFMVVM
{
    using System;

    public class FuncCommand<TResult> : CommandBase, IFuncCommand<TResult>
    {
        private readonly Func<bool> _canExecuteAction;
        private readonly Func<TResult> _executeAction;

        #region Implementation of IFuncCommand<out TResult>

        public FuncCommand(bool canExecute, Func<TResult> executeAction, Func<bool> canExecuteAction = null) : base(canExecute)
        {
            this._executeAction = executeAction;
            this._canExecuteAction = canExecuteAction ?? (() => true);
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        public TResult Execute()
        {
            if(this.CanExecute())
                return this._executeAction();
            return default(TResult);
        }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return this._canExecuteAction();
        }

        #endregion
    }
}