using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    ///     The implementation of <see cref="IDispatcher" /> with using of thread pool work item
    /// </summary>
    public class PDispatcher : Dispatcher
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly object _syncObject;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="syncObject">
        /// </param>
        public PDispatcher(object syncObject = null)
        {
            _syncObject = syncObject ?? new object();
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        protected override object SyncObject
        {
            [DebuggerStepThrough]
            get
            {
                return _syncObject;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="actionToInvoke">
        /// </param>
        [DebuggerStepThrough]
        protected override void InvokeAction(Action actionToInvoke)
        {
            ThreadPool.QueueUserWorkItem(obj => actionToInvoke());
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_syncObject != null);
        }
    }
}