// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Dispatcher.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Zabavnov.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    /// <summary>
    /// </summary>
    public abstract class Dispatcher : IDispatcher
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        public static readonly IDispatcher DirectDispatcher = new DirectDispatcher();

        /// <summary>
        /// </summary>
        public static readonly IDispatcher PDispatcher = new PDispatcher();

        /// <summary>
        /// </summary>
        protected static readonly Dictionary<object, Action> _actions = new Dictionary<object, Action>();

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        protected abstract object SyncObject { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Invoke action in parallel
        /// </summary>
        /// <param name="action">
        /// </param>
        [DebuggerStepThrough]
        public static void PInvoke(Action action)
        {
            Contract.Requires(action != null);

            PDispatcher.Invoke(action);
        }

        /// <summary>
        /// Invoke action in parallel
        /// </summary>
        /// <param name="keyObject">
        /// </param>
        /// <param name="action">
        /// </param>
        [DebuggerStepThrough]
        public static void PInvoke(object keyObject, Action action)
        {
            Contract.Requires(keyObject != null);
            Contract.Requires(action != null);

            PDispatcher.Invoke(keyObject, action);
        }

        /// <summary>
        /// invoke action in parallel and returns task for it
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        [DebuggerStepThrough]
        public static Task<T> PInvoke<T>(Func<T> action)
        {
            Contract.Requires(action != null);

            return Task.Factory.StartNew(action);
        }

        

        /// <summary>
        /// </summary>
        /// <param name="action">
        /// </param>
        [DebuggerStepThrough]
        public virtual void Invoke(Action action)
        {
            InvokeAction(action);
        }

        /// <summary>
        /// </summary>
        /// <param name="keyObject">
        /// </param>
        /// <param name="action">
        /// </param>
        public virtual void Invoke(object keyObject, Action action)
        {
            bool needInvoke;

            lock (SyncObject)
            {
                needInvoke = !_actions.ContainsKey(keyObject);
                _actions[keyObject] = action;
            }

            if (needInvoke)
            {
                Action invokeAction = () =>
                    {
                        lock (SyncObject)
                            if (_actions.Count > 0)
                            {
                                foreach (var act in _actions)
                                {
                                    InvokeAction(act.Value);
                                }

                                _actions.Clear();
                            }
                    };

                InvokeAction(invokeAction);
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="actionToInvoke">
        /// </param>
        protected abstract void InvokeAction(Action actionToInvoke);
    }
}