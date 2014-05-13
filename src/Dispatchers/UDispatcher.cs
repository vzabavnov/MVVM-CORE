namespace Zabavnov.WFMVVM
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Windows.Forms;

    /// <summary>
    ///     The implementation of <see cref="IDispatcher" /> with synchronization with UI thread
    /// </summary>
    public class UDispatcher : Dispatcher
    {
        private static readonly WindowsFormsSynchronizationContext _ctx = new WindowsFormsSynchronizationContext();
        private readonly object _syncObj;

        [DebuggerStepThrough]
        public UDispatcher(object syncObj = null)
        {
            this._syncObj = syncObj ?? new object();
        }

        protected override object SyncObject
        {
            [DebuggerStepThrough] get { return this._syncObj; }
        }

        [DebuggerStepThrough]
        protected override void InvokeAction(Action actionToInvoke)
        {
            Contract.Requires(actionToInvoke != null);

            _ctx.Post(state => actionToInvoke(), null);
        }
    }
}