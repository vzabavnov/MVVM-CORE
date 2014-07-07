namespace Zabavnov.MVVM
{
    using System;

    public class ExternalFuncCommand<TResult> : CommandBase, IExternalFuncCommand<TResult>
    {
        private readonly Action<TResult> _executeAction;

        public ExternalFuncCommand(bool canExecute, Action<TResult> executeAction, Func<bool> canExecuteAction = null)
            : base(canExecute, canExecuteAction)
        {
            _executeAction = executeAction;
            _status.Value = canExecute;
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        public void Execute()
        {
            TResult result;
            if (CanExecute() && Action != null && Action(out result))
            {
                _executeAction(result);
            }
        }

        /// <summary>
        /// </summary>
        public TryFunc<TResult> Action { get; set; }
    }
    
    public class ExternalFuncCommand<T, TResult> : CommandBase<T>, IExternalFuncCommand<T, TResult>
    {
        private readonly Action<T, TResult> _executeAction;

        public ExternalFuncCommand(bool canExecute, Action<T, TResult> executeAction, Func<T, bool> canExecuteAction = null)
            : base(canExecute, canExecuteAction)
        {
            _executeAction = executeAction;
            _status.Value = canExecute;
        }

        public void Execute()
        {
            TResult result;
            if (CanExecute() && Action != null && Action(Arg1, out result))
            {
                _executeAction(Arg1, result);
            }
        }

        #region Overrides of CommandBase<T>

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        public override bool CanExecute()
        {
            TResult result;
            if(Action != null && Action(Arg1, out result))
            {
                return _canExecuteAction(Arg1);
            }
            return false;
        }

        #endregion

        /// <summary>
        /// </summary>
        public TryFunc<T, TResult> Action { get; set; }
    }

    public class ExternalFuncCommand<T1, T2, TResult> : CommandBase<T1, T2>, IExternalFuncCommand<T1, T2, TResult>
    {
        private readonly Action<T1, T2, TResult> _executeAction;

        public ExternalFuncCommand(bool canExecute, Action<T1, T2, TResult> executeAction, Func<T1, T2, bool> canExecuteAction = null)
            : base(canExecute, canExecuteAction)
        {
            _executeAction = executeAction;
            _status.Value = canExecute;
        }

        public void Execute()
        {
            TResult result;
            if (CanExecute() && Action != null && Action(Arg1, Arg2, out result))
                _executeAction(Arg1, Arg2,result);
        }

        /// <summary>
        /// </summary>
        public TryFunc<T1, T2, TResult> Action { get; set; }
    }
}