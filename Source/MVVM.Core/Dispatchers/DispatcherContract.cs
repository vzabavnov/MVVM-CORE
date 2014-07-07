using System;
using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    [ContractClassFor(typeof(IDispatcher))]
    internal abstract class DispatcherContract : IDispatcher
    {
        #region Implementation of IDispatcher

        /// <summary>
        ///     Dispatch method call
        /// </summary>
        /// <param name="action">The action that will be dispatched</param>
        public void Invoke(Action action)
        {
            Contract.Requires(action != null);
        }

        /// <summary>
        ///     Dispatch method call with key
        /// </summary>
        /// <param name="keyObject">
        ///     the specified key object. the method will not be dispatched if more then one key object already
        ///     dispatched
        /// </param>
        /// <param name="action">The action that will be dispatched</param>
        /// <remarks>If dispatched method already processed then new method will be added to the queue</remarks>
        public void Invoke(object keyObject, Action action)
        {
            Contract.Requires(keyObject != null);
            Contract.Requires(action != null);
        }

        #endregion
    }
}