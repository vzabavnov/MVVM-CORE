namespace Zabavnov.WFMVVM
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    ///     The implementation of <see cref="IDispatcher" /> with using of thread pool work item
    /// </summary>
    public class PDispatcher : Dispatcher
    {
        private readonly object _syncObject;

        public PDispatcher(object syncObject = null)
        {
            this._syncObject = syncObject ?? new object();
        }

        protected override object SyncObject
        {
            [DebuggerStepThrough] get { return this._syncObject; }
        }

        [DebuggerStepThrough]
        protected override void InvokeAction(Action actionToInvoke)
        {
            ThreadPool.QueueUserWorkItem(obj => actionToInvoke());
        }
    }
}