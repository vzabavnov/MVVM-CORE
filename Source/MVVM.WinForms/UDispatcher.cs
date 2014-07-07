namespace Zabavnov.Windows.Forms.MVVM
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Zabavnov.MVVM;

    /// <summary>
    ///     The implementation of <see cref="IDispatcher" /> with synchronization with UI thread
    /// </summary>
    public class UDispatcher : Dispatcher
    {
        private static readonly WindowsFormsSynchronizationContext _ctx = new WindowsFormsSynchronizationContext();
        private readonly object _syncObj;

        /// <summary>
        /// </summary>
        public static readonly IDispatcher Instance = new UDispatcher();

        [DebuggerStepThrough]
        public UDispatcher(object syncObj = null)
        {
            _syncObj = syncObj ?? new object();
        }

        protected override object SyncObject
        {
            [DebuggerStepThrough] get { return _syncObj; }
        }

        [DebuggerStepThrough]
        protected override void InvokeAction(Action actionToInvoke)
        {
            Contract.Requires(actionToInvoke != null);

            _ctx.Post(state => actionToInvoke(), null);
        }


        /// <summary>
        /// Invoke action on UI thread
        /// </summary>
        /// <param name="action">
        /// </param>
        [DebuggerStepThrough]
        public static void UInvoke(Action action)
        {
            Instance.Invoke(action);
        }

        /// <summary>
        /// Invoke action on UI thread
        /// </summary>
        /// <param name="keyObject">
        /// </param>
        /// <param name="action">
        /// </param>
        [DebuggerStepThrough]
        public static void UInvoke(object keyObject, Action action)
        {
            Instance.Invoke(keyObject, action);
        }

        /// <summary>
        /// Invoke <paramref name="action"/> in parallel and execute <paramref name="resultAction"/> when the
        ///     <paramref name="action"/> completed on UI thread
        /// </summary>
        /// <typeparam name="T">
        /// The type of result
        /// </typeparam>
        /// <param name="action">
        /// the action to execute to get result
        /// </param>
        /// <param name="resultAction">
        /// The action to execute to use result. it will be executed on UI thread
        /// </param>
        [DebuggerStepThrough]
        public static void Invoke<T>(Func<T> action, Action<T> resultAction)
        {
            Contract.Requires(action != null);
            Contract.Requires(resultAction != null);

            Task.Factory.StartNew(action).ContinueWith(z => UInvoke(() => resultAction(z.Result)));
        }
    }
}