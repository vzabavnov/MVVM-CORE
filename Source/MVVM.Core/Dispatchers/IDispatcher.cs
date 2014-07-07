using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System;

    /// <summary>
    ///     Define the method call dispatcher
    /// </summary>
    [ContractClass(typeof(DispatcherContract))]
    public interface IDispatcher
    {
        /// <summary>
        ///     Dispatch method call
        /// </summary>
        /// <param name="action">The action that will be dispatched</param>
        void Invoke(Action action);

        /// <summary>
        ///     Dispatch method call with key
        /// </summary>
        /// <param name="keyObject">
        ///     the specified key object. the method will not be dispatched if more then one key object already
        ///     dispatched
        /// </param>
        /// <param name="action">The action that will be dispatched</param>
        /// <remarks>If dispatched method already processed then new method will be added to the queue</remarks>
        void Invoke(object keyObject, Action action);
    }
}