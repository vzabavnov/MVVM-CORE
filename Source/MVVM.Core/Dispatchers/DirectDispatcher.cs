using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// </summary>
    [DebuggerStepThrough]
    public class DirectDispatcher : Dispatcher
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly object _syncObject = new object();

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        protected override object SyncObject => _syncObject;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="actionToInvoke">
        /// </param>
        protected override void InvokeAction(Action actionToInvoke)
        {
            Contract.Assume(actionToInvoke != null);

            actionToInvoke();
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_syncObject != null);
        }
    }
}