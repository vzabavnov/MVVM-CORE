namespace Zabavnov.WFMVVM
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    public abstract class Dispatcher : IDispatcher
    {
        protected static readonly Dictionary<object, Action> _actions = new Dictionary<object, Action>();

        public static readonly IDispatcher UDispatcher = new UDispatcher();
        public static readonly IDispatcher PDispatcher = new PDispatcher();
        public static readonly IDispatcher DirectDispatcher = new DirectDispatcher();

        protected abstract object SyncObject { get; }

        #region IDispatcher implementation

        [DebuggerStepThrough]
        public virtual void Invoke(Action action)
        {
            this.InvokeAction(action);
        }

        public virtual void Invoke(object keyObject, Action action)
        {
            Contract.Requires(keyObject != null);
            Contract.Requires(action != null);

            bool needInvoke;

            lock(this.SyncObject)
            {
                needInvoke = !_actions.ContainsKey(keyObject);
                _actions[keyObject] = action;
            }

            if(needInvoke)
            {
                Action invokeAction = () =>
                    {
                        lock(this.SyncObject)
                            if(_actions.Count > 0)
                            {
                                foreach(var act in _actions)
                                    this.InvokeAction(act.Value);

                                _actions.Clear();
                            }
                    };

                this.InvokeAction(invokeAction);
            }
        }

        #endregion

        protected abstract void InvokeAction(Action actionToInvoke);

        /// <summary>
        ///     Invoke action on UI thread
        /// </summary>
        /// <param name="action"></param>
        [DebuggerStepThrough]
        public static void UInvoke(Action action)
        {
            UDispatcher.Invoke(action);
        }

        /// <summary>
        ///     Invoke action on UI thread
        /// </summary>
        /// <param name="keyObject"></param>
        /// <param name="action"></param>
        [DebuggerStepThrough]
        public static void UInvoke(object keyObject, Action action)
        {
            UDispatcher.Invoke(keyObject, action);
        }

        /// <summary>
        ///     Invoke action in parallel
        /// </summary>
        /// <param name="action"></param>
        [DebuggerStepThrough]
        public static void PInvoke(Action action)
        {
            PDispatcher.Invoke(action);
        }

        /// <summary>
        ///     Invoke action in parallel
        /// </summary>
        /// <param name="keyObject"></param>
        /// <param name="action"></param>
        [DebuggerStepThrough]
        public static void PInvoke(object keyObject, Action action)
        {
            PDispatcher.Invoke(keyObject, action);
        }

        /// <summary>
        ///     invoke action in parallel and returns task for it
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Task<T> PInvoke<T>(Func<T> action)
        {
            Contract.Requires(action != null);

            return Task.Factory.StartNew(action);
        }

        /// <summary>
        ///     Invoke <paramref name="action" /> in parallel and execute <paramref name="resultAction" /> when the
        ///     <paramref name="action" /> completed on UI thread
        /// </summary>
        /// <typeparam name="T">The type of result</typeparam>
        /// <param name="action">the action to execute to get result</param>
        /// <param name="resultAction">The action to execute to use result. it will be executed on UI thread</param>
        [DebuggerStepThrough]
        public static void Invoke<T>(Func<T> action, Action<T> resultAction)
        {
            Contract.Requires(action != null);
            Contract.Requires(resultAction != null);

            Task.Factory.StartNew(action).ContinueWith(z => UInvoke(() => resultAction(z.Result)));
        }
    }
}