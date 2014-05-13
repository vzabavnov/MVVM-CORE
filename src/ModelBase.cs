namespace Zabavnov.WFMVVM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class ModelBase<T> : INotifyPropertyChanged where T : INotifyPropertyChanged
    {
        private readonly Dictionary<string, ISet<Action>> _dependencyActions = new Dictionary<string, ISet<Action>>();

        #region INotifyPropertyChanged Members

        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        ///     delegate changes on property <see cref="propertyToMonitorLambda" /> to changes on property
        ///     <see cref="propertyToRaiseChangesLambda" /> and <see cref="propertyLambdas" />
        /// </summary>
        /// <param name="propertyToMonitorLambda">Property to monitor changes on</param>
        /// <param name="dispatcher">The dispatcher to use for dispatch the action</param>
        /// <param name="propertyToRaiseChangesLambda">The property to raise event on</param>
        /// <param name="propertyLambdas">Additional properties to raise event on</param>
        [DebuggerStepThrough]
        protected void DelegatePropertyChanged(Expression<Func<T, object>> propertyToMonitorLambda, IDispatcher dispatcher,
                                               Expression<Func<T, object>> propertyToRaiseChangesLambda, params Expression<Func<T, object>>[] propertyLambdas)
        {
            this.AddActionOn(() => this.RaisePropertyChanged(dispatcher, propertyToRaiseChangesLambda, propertyLambdas), propertyToMonitorLambda);
        }

        /// <summary>
        ///     delegate changes on property <see cref="propertyToMonitorLambda" /> to changes on property
        ///     <see cref="propertyToRaiseChangesLambda" /> and <see cref="propertyLambdas" />
        /// </summary>
        /// <param name="propertyToMonitorLambda">Property to monitor changes on</param>
        /// <param name="propertyToRaiseChangesLambda">The property to raise event on</param>
        /// <param name="propertyLambdas">Additional properties to raise event on</param>
        [DebuggerStepThrough]
        protected void DelegatePropertyChanged(Expression<Func<T, object>> propertyToMonitorLambda,
                                               Expression<Func<T, object>> propertyToRaiseChangesLambda, params Expression<Func<T, object>>[] propertyLambdas)
        {
            this.DelegatePropertyChanged(propertyToMonitorLambda, Dispatcher.DirectDispatcher, propertyToRaiseChangesLambda, propertyLambdas);
        }

        /// <summary>
        ///     delegate changes on property <see cref="propertyToMonitorLambda" /> to changes on property
        ///     <see cref="propertyToRaiseChangesLambda" /> and <see cref="propertyLambdas" />
        /// </summary>
        /// <param name="propertyToMonitorLambda">Property to monitor changes on</param>
        /// <param name="dispatch">specified that use <see cref="UDispatcher" /> to raise event</param>
        /// <param name="propertyToRaiseChangesLambda">The property to raise event on</param>
        /// <param name="propertyLambdas">Additional properties to raise event on</param>
        [DebuggerStepThrough]
        protected void DelegatePropertyChanged(Expression<Func<T, object>> propertyToMonitorLambda, bool dispatch,
                                               Expression<Func<T, object>> propertyToRaiseChangesLambda, params Expression<Func<T, object>>[] propertyLambdas)
        {
            this.DelegatePropertyChanged(propertyToMonitorLambda, dispatch ? Dispatcher.UDispatcher : Dispatcher.DirectDispatcher, 
                propertyToRaiseChangesLambda, propertyLambdas);
        }

        [DebuggerStepThrough]
        protected void RaisePropertyChanged(string propertyName, params string[] names)
        {
            this.RaisePropertyChanged(Dispatcher.DirectDispatcher, propertyName, names);
        }

        [DebuggerStepThrough]
        protected void RaisePropertyChanged(IDispatcher dispatcher, string propertyName, params string[] names)
        {
            var handler = this.PropertyChanged;
            if(handler != null)
                if(names.Length == 0)
                    dispatcher.Invoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
                else
                    this.RaisePropertyChanged(dispatcher, names.AddHead(propertyName).Select(z => new PropertyChangedEventArgs(z)));
        }

        [DebuggerStepThrough]
        protected void RaisePropertyChanged(IDispatcher dispatcher, IEnumerable<PropertyChangedEventArgs> args)
        {
            var handler = this.PropertyChanged;
            if(handler != null)
                args.ForEach(z => dispatcher.Invoke(() => handler(this, z)));
        }

        [DebuggerStepThrough]
        protected void RaisePropertyChanged(IDispatcher dispatcher, PropertyChangedEventArgs args)
        {
            var handler = this.PropertyChanged;
            if(handler != null)
                dispatcher.Invoke(() => handler(this, args));
        }

        [DebuggerStepThrough]
        protected void RaisePropertyChanged<TProperty>(IDispatcher dispatcher, Expression<Func<T, TProperty>> propertyLambda)
        {
            var handler = this.PropertyChanged;
            if(handler != null)
            {
                var propName = propertyLambda.GetPropertyName();
                dispatcher.Invoke(() => handler(this, new PropertyChangedEventArgs(propName)));
            }
        }

        [DebuggerStepThrough]
        protected void RaisePropertyChanged<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            this.RaisePropertyChanged(Dispatcher.DirectDispatcher, propertyLambda);
        }

        [DebuggerStepThrough]
        protected void RaisePropertyChanged(IDispatcher dispatcher, Expression<Func<T, object>> propertyLambda,
                                            params Expression<Func<T, object>>[] propertyLambdas)
        {
            var handler = this.PropertyChanged;
            if(handler != null)
                this.RaisePropertyChanged(dispatcher,
                    propertyLambdas.AddHead(propertyLambda).Select(z => new PropertyChangedEventArgs(z.GetPropertyName())));
        }

        /// <summary>
        ///     Dispatch notification about property changed onto UI thread via <see cref="UDispatcher" />
        /// </summary>
        /// <param name="propertyLambda">property's lambda to notify</param>
        /// <param name="propertyLambdas"></param>
        [DebuggerStepThrough]
        protected void DispatchPropertyChanged(Expression<Func<T, object>> propertyLambda,
                                               params Expression<Func<T, object>>[] propertyLambdas)
        {
            this.RaisePropertyChanged(Dispatcher.UDispatcher, propertyLambda, propertyLambdas);
        }

        /// <summary>
        ///     Raise change event for specified property(s)
        /// </summary>
        /// <param name="propertyLambda"></param>
        /// <param name="propertyLambdas"></param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(Expression<Func<T, object>> propertyLambda,
                                            params Expression<Func<T, object>>[] propertyLambdas)
        {
            this.RaisePropertyChanged(Dispatcher.DirectDispatcher, propertyLambda, propertyLambdas);
        }

        /// <summary>
        ///     raise change property notification event
        /// </summary>
        /// <param name="dispatch">use <see cref="UDispatcher" /> if true and <see cref="DirectDispatcher" /> otherwise</param>
        /// <param name="propertyLambda"></param>
        /// <param name="propertyLambdas"></param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(bool dispatch, Expression<Func<T, object>> propertyLambda,
                                            params Expression<Func<T, object>>[] propertyLambdas)
        {
            this.RaisePropertyChanged(dispatch ? Dispatcher.UDispatcher : Dispatcher.DirectDispatcher, propertyLambda, propertyLambdas);
        }

        /// <summary>
        ///     Reset provider status if it is in <b>ready</b> status and raise notification
        /// </summary>
        /// <param name="provider">The provider to reset</param>
        /// <param name="propertyLambda">The property to raise notification on</param>
        /// <param name="propertyLambdas"></param>
        /// <param name="dispatch">dispatch via Uthread or directly</param>
        [DebuggerStepThrough]
        protected void ResetAndRaise(IDataProvider provider, bool dispatch, Expression<Func<T, object>> propertyLambda,
                                     params Expression<Func<T, object>>[] propertyLambdas)
        {
            this.ResetAndRaise(provider, dispatch ? Dispatcher.UDispatcher : Dispatcher.DirectDispatcher, propertyLambda,
                propertyLambdas);
        }

        /// <summary>
        ///     Reset provider status if it is in <b>ready</b> status and raise notification
        /// </summary>
        /// <param name="provider">The provider to reset</param>
        /// <param name="propertyLambda">The property to raise notification on</param>
        /// <param name="propertyLambdas"></param>
        [DebuggerStepThrough]
        protected void ResetAndRaise(IDataProvider provider, Expression<Func<T, object>> propertyLambda,
                                     params Expression<Func<T, object>>[] propertyLambdas)
        {
            this.ResetAndRaise(provider, Dispatcher.DirectDispatcher, propertyLambda, propertyLambdas);
        }

        /// <summary>
        ///     Reset provider status if it is in <b>ready</b> status and raise notification
        /// </summary>
        /// <param name="provider">The provider to reset</param>
        /// <param name="propertyLambda">The property to raise notification on</param>
        /// <param name="propertyLambdas"></param>
        /// <param name="dispatcher">the dispatcher</param>
        [DebuggerStepThrough]
        protected void ResetAndRaise(IDataProvider provider, IDispatcher dispatcher, Expression<Func<T, object>> propertyLambda,
                                     params Expression<Func<T, object>>[] propertyLambdas)
        {
            if (provider.Status.Value != DataProviderStatus.NotReady)
            {
                provider.Reset();
                this.RaisePropertyChanged(dispatcher, propertyLambda, propertyLambdas);
            }
        }

        [DebuggerStepThrough]
        protected void AddActionOn(Action action, Expression<Func<T, object>> propertyLambda,
                                   params Expression<Func<T, object>>[] propertyLambdas)
        {
            if(this._dependencyActions.Count == 0)
                this.PropertyChanged += this.OnPropertyChanged;

            propertyLambdas.AddHead(propertyLambda).Select(z => z.GetPropertyName()).Distinct().ForEach(name =>
                {
                    ISet<Action> set;
                    if(!this._dependencyActions.TryGetValue(name, out set))
                        this._dependencyActions.Add(name, (set = new HashSet<Action>()));

                    if(!set.Contains(action))
                        set.Add(action);
                });
        }

        [DebuggerStepThrough]
        protected void AddActionOn<TProperty>(Action action, Expression<Func<T, TProperty>> propertyLambda)
        {
            if(this._dependencyActions.Count == 0)
                this.PropertyChanged += this.OnPropertyChanged;

            var name = propertyLambda.GetPropertyName();
            ISet<Action> set;
            if(!this._dependencyActions.TryGetValue(name, out set))
                this._dependencyActions.Add(name, (set = new HashSet<Action>()));

            if(!set.Contains(action))
                set.Add(action);
        }

        [DebuggerStepThrough]
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            ISet<Action> set;
            if(this._dependencyActions.TryGetValue(propertyChangedEventArgs.PropertyName, out set))
                set.ForEach(z => z());
        }

        [DebuggerStepThrough]
        protected void AttachResetAndRaise<TProviderData>(Expression<Func<T, object>> propertyExpressionToAttachTo,
                                                          IDataProvider<TProviderData> providerToReset, Expression<Func<T, object>> propertyLambdaToraiseChangesOn)
        {
            this.AddActionOn(() => this.ResetAndRaise(providerToReset, false, propertyLambdaToraiseChangesOn), propertyExpressionToAttachTo);
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = this.PropertyChanged;
            if(handler != null)
                handler(this, args);
        }
    }
}