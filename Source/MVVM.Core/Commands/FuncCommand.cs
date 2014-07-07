namespace Zabavnov.MVVM
{
    using System;

    public class FuncCommand<TResult> : CommandBase, IFuncCommand<TResult>
    {
        private readonly TryFunc<TResult> _executeAction;

        public FuncCommand(bool canExecute, TryFunc<TResult> executeAction, Func<bool> canExecuteAction= null)
            : base(canExecute, canExecuteAction)
        {
            _executeAction = executeAction;
        }

        #region Implementation of IFuncCommand<TResult>

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        /// <param name="result"></param>
        public bool TryExecute(out TResult result)
        {
            if(CanExecute())
                return _executeAction(out result);
            
            result = default(TResult);
            return false;
        }

        #endregion
    }

    public class FuncCommand<T1, TResult> : CommandBase<T1>, IFuncCommand<T1, TResult>
    {
        private readonly TryFunc<T1, TResult> _executeAction;

        public FuncCommand(bool canExecute, TryFunc<T1, TResult> executeAction, Func<T1, bool> canExecuteAction = null)
            : base(canExecute, canExecuteAction)
        {
            _executeAction = executeAction;
        }

        #region Implementation of IFuncCommand<TResult>

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        /// <param name="result"></param>
        public bool TryExecute(out TResult result)
        {
            if (CanExecute())
                return _executeAction(Arg1, out result);

            result = default(TResult);
            return false;
        }

        #endregion
    }

    public class FuncCommand<T1, T2, TResult> : CommandBase<T1, T2>, IFuncCommand<T1, T2, TResult>
    {
        private readonly TryFunc<T1, T2, TResult> _executeAction;

        public FuncCommand(bool canExecute, TryFunc<T1, T2, TResult> executeAction, Func<T1, T2, bool> canExecuteAction = null)
            : base(canExecute, canExecuteAction)
        {
            _executeAction = executeAction;
        }

        #region Implementation of IFuncCommand<TResult>

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        /// <param name="result"></param>
        public bool TryExecute(out TResult result)
        {
            if (CanExecute())
                return _executeAction(Arg1, Arg2, out result);

            result = default(TResult);
            return false;
        }

        #endregion
    }
}