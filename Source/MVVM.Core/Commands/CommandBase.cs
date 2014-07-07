using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System;

    /// <summary>
    ///     base class for <see cref="ICommand" />. it contains the Status functionalities
    /// </summary>
    public abstract class CommandBase
    {
        protected readonly SimpleNotifiable<bool> _status;

        protected readonly Func<bool> _canExecuteAction;

        protected CommandBase(bool canExecute, Func<bool> canExecuteAction = null)
        {
            _canExecuteAction = canExecuteAction ?? (() => true);
            _status = new SimpleNotifiable<bool>(canExecute);
        }

        public INotifiable<bool> Status
        {
            get { return _status; }
        }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        public virtual bool CanExecute()
        {
            _status.Value = _canExecuteAction();
            return _status.Value;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_canExecuteAction != null);
            Contract.Invariant(_status != null);
        }
    }

    public abstract class CommandBase<T>
    {
        protected readonly SimpleNotifiable<bool> _status;

        protected readonly Func<T, bool> _canExecuteAction;

        protected CommandBase(bool canExecute, Func<T, bool> canExecuteAction = null)
        {
            _canExecuteAction = canExecuteAction ?? (t => true);
            _status = new SimpleNotifiable<bool>(canExecute);
        }

        public INotifiable<bool> Status
        {
            get { return _status; }
        }

        public T Arg1 { get; set; }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        public virtual bool CanExecute()
        {
            _status.Value = _canExecuteAction(Arg1);
            return _status.Value;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_canExecuteAction != null);
            Contract.Invariant(_status != null);
        }
    }

    public abstract class CommandBase<T1, T2>
    {
        protected readonly SimpleNotifiable<bool> _status;

        protected readonly Func<T1, T2, bool> _canExecuteAction;

        protected CommandBase(bool canExecute, Func<T1, T2, bool> canExecuteAction = null)
        {
            _canExecuteAction = canExecuteAction ?? ((t1, t2) => true);
            _status = new SimpleNotifiable<bool>(canExecute);
        }

        public INotifiable<bool> Status
        {
            get { return _status; }
        }

        public T1 Arg1 { get; set; }
        public T2 Arg2 { get; set; }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <returns></returns>
        public virtual bool CanExecute()
        {
            _status.Value = _canExecuteAction(Arg1, Arg2);
            return _status.Value;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_canExecuteAction != null);
            Contract.Invariant(_status != null);
        }
    }
}