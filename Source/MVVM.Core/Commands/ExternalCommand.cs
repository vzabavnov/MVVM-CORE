namespace Zabavnov.MVVM
{
    using System;
    using System.Diagnostics;

    public class ExternalCommand : CommandBase, IExternalCommand
    {
        private readonly Action _executeAction;

        public ExternalCommand(bool canExecute, Action executeAction, Func<bool> canExecuteAction = null)
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
            if (Action != null)
            {
                if (CanExecute() && Action())
                    _executeAction();
            }
            else
            {
                Trace.WriteLine("The Action should be specified");
            }
        }

        /// <summary>
        /// The <see cref="IExternalCommand.Action"/> with will be executed during command's Execute
        /// </summary>
        public Func<bool> Action { get; set; }
    }
    
    public class ExternalCommand<T> : CommandBase<T>, IExternalCommand<T>
    {
        private readonly Action<Func<T, bool>> _executeAction;

        public ExternalCommand(bool canExecute, Action<Func<T, bool>> executeAction, Func<T, bool> canExecuteAction = null)
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
            if (Action != null)
            {
                if (CanExecute())
                    _executeAction(Action);
            }
            else
            {
                Trace.WriteLine("The Action should be specified");
            }
        }

        /// <summary>
        /// The <see cref="IExternalCommand.Action"/> with will be executed during command's Execute
        /// </summary>
        public Func<T, bool> Action { get; set; }
    }

    public class ExternalCommand<T1, T2> : CommandBase<T1, T2>, IExternalCommand<T1, T2>
    {
        private readonly Action<Func<T1, T2, bool>> _executeAction;

        public ExternalCommand(bool canExecute, Action<Func<T1, T2, bool>> executeAction, Func<T1, T2, bool> canExecuteAction = null)
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
            if (Action != null)
            {
                if (CanExecute())
                    _executeAction(Action);
            }
            else
            {
                Trace.WriteLine("The Action should be specified");
            }
        }

        /// <summary>
        /// The <see cref="IExternalCommand.Action"/> with will be executed during command's Execute
        /// </summary>
        public Func<T1, T2, bool> Action { get; set; }
    }
}