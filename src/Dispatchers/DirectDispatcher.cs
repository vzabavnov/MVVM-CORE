namespace Zabavnov.WFMVVM
{
    using System;
    using System.Diagnostics;

    [DebuggerStepThrough]
    public class DirectDispatcher : Dispatcher
    {
        private readonly object _syncObject = new object();

        #region Overrides of Dispatcher

        protected override object SyncObject
        {
            get { return this._syncObject; }
        }

        protected override void InvokeAction(Action actionToInvoke)
        {
            actionToInvoke();
        }

        #endregion
    }
}