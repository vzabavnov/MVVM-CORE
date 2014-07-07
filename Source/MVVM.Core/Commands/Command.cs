namespace Zabavnov.MVVM
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    public class Command : CommandBase, ICommand
    {
        private readonly Action _executeAction;

        public Command(bool canExecute, Action executeAction, Func<bool> canExecuteAction = null)
            : base(canExecute, canExecuteAction)
        {
            Contract.Requires(executeAction != null);

            _executeAction = executeAction;
        }

        #region Implementation of ICommand

        [DebuggerStepThrough]
        public void Execute()
        {
            if (CanExecute())
                _executeAction();
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_executeAction != null);
        }
    }

    public class Command<T1> : CommandBase<T1>, ICommand<T1>
    {
        private readonly Action<T1> _executeAction;

        public Command(bool canExecute, Action<T1> executeAction, Func<T1, bool> canExecuteAction)
            : base(canExecute, canExecuteAction)
        {
            Contract.Requires(executeAction != null); 

            _executeAction = executeAction;
        }

        #region Implementation of ICommand

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        public void Execute()
        {
            if (CanExecute())
                _executeAction(Arg1);
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_executeAction != null);
        }
    }

    public class Command<T1, T2> : CommandBase<T1, T2>, ICommand<T1, T2>
    {
        private readonly Action<T1, T2> _executeAction;

        public Command(bool canExecute, Action<T1, T2> executeAction, Func<T1, T2, bool> canExecuteAction)
            : base(canExecute, canExecuteAction)
        {
            Contract.Requires(executeAction != null); 

            _executeAction = executeAction;
        }

        #region Implementation of ICommand

        /// <summary>
        ///     Defines the method to be called when the command is invoked
        /// </summary>
        public void Execute()
        {
            if (CanExecute())
                _executeAction(Arg1, Arg2);
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_executeAction != null);
        }
    }
}