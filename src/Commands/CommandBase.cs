namespace Zabavnov.WFMVVM
{
    /// <summary>
    ///     base class for <see cref="ICommand" />. it contains the Status functionalities
    /// </summary>
    public abstract class CommandBase
    {
        protected readonly SimpleNotifiable<bool> _status;

        protected CommandBase(bool canExecute)
        {
            this._status = new SimpleNotifiable<bool>(canExecute);
        }

        public INotifiable<bool> Status
        {
            get { return this._status; }
        }
    }
}