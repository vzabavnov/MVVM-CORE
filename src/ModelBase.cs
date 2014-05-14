#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Zabavnov.WFMVVM
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class ModelBase<T> : INotifyPropertyChanged
        where T : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Dictionary<string, ISet<Action>> _dependencyActions = new Dictionary<string, ISet<Action>>();

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <param name="propertyLambda">
        /// </param>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected void AddActionOn(Action action, Expression<Func<T, object>> propertyLambda, params Expression<Func<T, object>>[] propertyLambdas)
        {
            if (_dependencyActions.Count == 0)
            {
                PropertyChanged += OnPropertyChanged;
            }

            propertyLambdas.AddHead(propertyLambda).Select(z => z.GetPropertyName()).Distinct().ForEach(
                name =>
                    {
                        ISet<Action> set;
                        if (!_dependencyActions.TryGetValue(name, out set))
                        {
                            _dependencyActions.Add(name, set = new HashSet<Action>());
                        }

                        if (!set.Contains(action))
                        {
                            set.Add(action);
                        }
                    });
        }

        /// <summary>
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <param name="propertyLambda">
        /// </param>
        /// <typeparam name="TProperty">
        /// </typeparam>
        [DebuggerStepThrough]
        protected void AddActionOn<TProperty>(Action action, Expression<Func<T, TProperty>> propertyLambda)
        {
            if (_dependencyActions.Count == 0)
            {
                PropertyChanged += OnPropertyChanged;
            }

            var name = propertyLambda.GetPropertyName();
            ISet<Action> set;
            if (!_dependencyActions.TryGetValue(name, out set))
            {
                _dependencyActions.Add(name, set = new HashSet<Action>());
            }

            if (!set.Contains(action))
            {
                set.Add(action);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyExpressionToAttachTo">
        /// </param>
        /// <param name="providerToReset">
        /// </param>
        /// <param name="propertyLambdaToraiseChangesOn">
        /// </param>
        /// <typeparam name="TProviderData">
        /// </typeparam>
        [DebuggerStepThrough]
        protected void AttachResetAndRaise<TProviderData>(
            Expression<Func<T, object>> propertyExpressionToAttachTo, 
            IDataProvider<TProviderData> providerToReset, 
            Expression<Func<T, object>> propertyLambdaToraiseChangesOn)
        {
            AddActionOn(() => ResetAndRaise(providerToReset, false, propertyLambdaToraiseChangesOn), propertyExpressionToAttachTo);
        }

        /// <summary>
        /// delegate changes on property <see cref="propertyToMonitorLambda"/> to changes on property
        ///     <see cref="propertyToRaiseChangesLambda"/> and <see cref="propertyLambdas"/>
        /// </summary>
        /// <param name="propertyToMonitorLambda">
        /// Property to monitor changes on
        /// </param>
        /// <param name="dispatcher">
        /// The dispatcher to use for dispatch the action
        /// </param>
        /// <param name="propertyToRaiseChangesLambda">
        /// The property to raise event on
        /// </param>
        /// <param name="propertyLambdas">
        /// Additional properties to raise event on
        /// </param>
        [DebuggerStepThrough]
        protected void DelegatePropertyChanged(
            Expression<Func<T, object>> propertyToMonitorLambda, 
            IDispatcher dispatcher, 
            Expression<Func<T, object>> propertyToRaiseChangesLambda, 
            params Expression<Func<T, object>>[] propertyLambdas)
        {
            AddActionOn(() => RaisePropertyChanged(dispatcher, propertyToRaiseChangesLambda, propertyLambdas), propertyToMonitorLambda);
        }

        /// <summary>
        /// delegate changes on property <see cref="propertyToMonitorLambda"/> to changes on property
        ///     <see cref="propertyToRaiseChangesLambda"/> and <see cref="propertyLambdas"/>
        /// </summary>
        /// <param name="propertyToMonitorLambda">
        /// Property to monitor changes on
        /// </param>
        /// <param name="propertyToRaiseChangesLambda">
        /// The property to raise event on
        /// </param>
        /// <param name="propertyLambdas">
        /// Additional properties to raise event on
        /// </param>
        [DebuggerStepThrough]
        protected void DelegatePropertyChanged(
            Expression<Func<T, object>> propertyToMonitorLambda, 
            Expression<Func<T, object>> propertyToRaiseChangesLambda, 
            params Expression<Func<T, object>>[] propertyLambdas)
        {
            DelegatePropertyChanged(propertyToMonitorLambda, Dispatcher.DirectDispatcher, propertyToRaiseChangesLambda, propertyLambdas);
        }

        /// <summary>
        /// delegate changes on property <see cref="propertyToMonitorLambda"/> to changes on property
        ///     <see cref="propertyToRaiseChangesLambda"/> and <see cref="propertyLambdas"/>
        /// </summary>
        /// <param name="propertyToMonitorLambda">
        /// Property to monitor changes on
        /// </param>
        /// <param name="dispatch">
        /// specified that use <see cref="UDispatcher"/> to raise event
        /// </param>
        /// <param name="propertyToRaiseChangesLambda">
        /// The property to raise event on
        /// </param>
        /// <param name="propertyLambdas">
        /// Additional properties to raise event on
        /// </param>
        [DebuggerStepThrough]
        protected void DelegatePropertyChanged(
            Expression<Func<T, object>> propertyToMonitorLambda, 
            bool dispatch, 
            Expression<Func<T, object>> propertyToRaiseChangesLambda, 
            params Expression<Func<T, object>>[] propertyLambdas)
        {
            DelegatePropertyChanged(
                propertyToMonitorLambda, 
                dispatch ? Dispatcher.UDispatcher : Dispatcher.DirectDispatcher, 
                propertyToRaiseChangesLambda, 
                propertyLambdas);
        }

        /// <summary>
        /// Dispatch notification about property changed onto UI thread via <see cref="UDispatcher"/>
        /// </summary>
        /// <param name="propertyLambda">
        /// property's lambda to notify
        /// </param>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected void DispatchPropertyChanged(Expression<Func<T, object>> propertyLambda, params Expression<Func<T, object>>[] propertyLambdas)
        {
            RaisePropertyChanged(Dispatcher.UDispatcher, propertyLambda, propertyLambdas);
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyName">
        /// </param>
        /// <param name="names">
        /// </param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(string propertyName, params string[] names)
        {
            RaisePropertyChanged(Dispatcher.DirectDispatcher, propertyName, names);
        }

        /// <summary>
        /// </summary>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="propertyName">
        /// </param>
        /// <param name="names">
        /// </param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(IDispatcher dispatcher, string propertyName, params string[] names)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                if (names.Length == 0)
                {
                    dispatcher.Invoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
                }
                else
                {
                    RaisePropertyChanged(dispatcher, names.AddHead(propertyName).Select(z => new PropertyChangedEventArgs(z)));
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="args">
        /// </param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(IDispatcher dispatcher, IEnumerable<PropertyChangedEventArgs> args)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                args.ForEach(z => dispatcher.Invoke(() => handler(this, z)));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="args">
        /// </param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(IDispatcher dispatcher, PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                dispatcher.Invoke(() => handler(this, args));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="propertyLambda">
        /// </param>
        /// <typeparam name="TProperty">
        /// </typeparam>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged<TProperty>(IDispatcher dispatcher, Expression<Func<T, TProperty>> propertyLambda)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                var propName = propertyLambda.GetPropertyName();
                dispatcher.Invoke(() => handler(this, new PropertyChangedEventArgs(propName)));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyLambda">
        /// </param>
        /// <typeparam name="TProperty">
        /// </typeparam>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            RaisePropertyChanged(Dispatcher.DirectDispatcher, propertyLambda);
        }

        /// <summary>
        /// </summary>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="propertyLambda">
        /// </param>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(
            IDispatcher dispatcher, 
            Expression<Func<T, object>> propertyLambda, 
            params Expression<Func<T, object>>[] propertyLambdas)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                RaisePropertyChanged(dispatcher, propertyLambdas.AddHead(propertyLambda).Select(z => new PropertyChangedEventArgs(z.GetPropertyName())));
            }
        }

        /// <summary>
        /// Raise change event for specified property(s)
        /// </summary>
        /// <param name="propertyLambda">
        /// </param>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(Expression<Func<T, object>> propertyLambda, params Expression<Func<T, object>>[] propertyLambdas)
        {
            RaisePropertyChanged(Dispatcher.DirectDispatcher, propertyLambda, propertyLambdas);
        }

        /// <summary>
        /// raise change property notification event
        /// </summary>
        /// <param name="dispatch">
        /// use <see cref="UDispatcher"/> if true and <see cref="DirectDispatcher"/> otherwise
        /// </param>
        /// <param name="propertyLambda">
        /// </param>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(
            bool dispatch, 
            Expression<Func<T, object>> propertyLambda, 
            params Expression<Func<T, object>>[] propertyLambdas)
        {
            RaisePropertyChanged(dispatch ? Dispatcher.UDispatcher : Dispatcher.DirectDispatcher, propertyLambda, propertyLambdas);
        }

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="propertyLambdaToRaise">
        /// </param>
        /// <param name="propertyLambdaToMonitor">
        /// </param>
        /// <param name="propertiesLambdaToMonitor">
        /// </param>
        protected void RaisePropertyChangedOn(
            IDispatcher dispatcher, 
            Expression<Func<T, object>> propertyLambdaToRaise, 
            Expression<Func<T, object>> propertyLambdaToMonitor, 
            params Expression<Func<T, object>>[] propertiesLambdaToMonitor)
        {
            AddActionOn(() => RaisePropertyChanged(dispatcher, propertyLambdaToRaise), propertyLambdaToMonitor, propertiesLambdaToMonitor);
        }

        /// <summary>
        /// </summary>
        /// <param name="dispatch">
        /// </param>
        /// <param name="propertyLambdaToRaise">
        /// </param>
        /// <param name="propertyLambdaToMonitor">
        /// </param>
        /// <param name="propertiesLambdaToMonitor">
        /// </param>
        protected void RaisePropertyChangedOn(
            bool dispatch, 
            Expression<Func<T, object>> propertyLambdaToRaise, 
            Expression<Func<T, object>> propertyLambdaToMonitor, 
            params Expression<Func<T, object>>[] propertiesLambdaToMonitor)
        {
            AddActionOn(() => RaisePropertyChanged(dispatch, propertyLambdaToRaise), propertyLambdaToMonitor, propertiesLambdaToMonitor);
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyLambdaToRaise">
        /// </param>
        /// <param name="propertyLambdaToMonitor">
        /// </param>
        /// <param name="propertiesLambdaToMonitor">
        /// </param>
        protected void RaisePropertyChangedOn(
            Expression<Func<T, object>> propertyLambdaToRaise, 
            Expression<Func<T, object>> propertyLambdaToMonitor, 
            params Expression<Func<T, object>>[] propertiesLambdaToMonitor)
        {
            AddActionOn(() => RaisePropertyChanged(propertyLambdaToRaise), propertyLambdaToMonitor, propertiesLambdaToMonitor);
        }

        /// <summary>
        /// Reset provider status if it is in <b>ready</b> status and raise notification
        /// </summary>
        /// <param name="provider">
        /// The provider to reset
        /// </param>
        /// <param name="dispatch">
        /// dispatch via Uthread or directly
        /// </param>
        /// <param name="propertyLambda">
        /// The property to raise notification on
        /// </param>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected void ResetAndRaise(
            IDataProvider provider, 
            bool dispatch, 
            Expression<Func<T, object>> propertyLambda, 
            params Expression<Func<T, object>>[] propertyLambdas)
        {
            ResetAndRaise(provider, dispatch ? Dispatcher.UDispatcher : Dispatcher.DirectDispatcher, propertyLambda, propertyLambdas);
        }

        /// <summary>
        /// Reset provider status if it is in <b>ready</b> status and raise notification
        /// </summary>
        /// <param name="provider">
        /// The provider to reset
        /// </param>
        /// <param name="propertyLambda">
        /// The property to raise notification on
        /// </param>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected void ResetAndRaise(
            IDataProvider provider, 
            Expression<Func<T, object>> propertyLambda, 
            params Expression<Func<T, object>>[] propertyLambdas)
        {
            ResetAndRaise(provider, Dispatcher.DirectDispatcher, propertyLambda, propertyLambdas);
        }

        /// <summary>
        /// Reset provider status if it is in <b>ready</b> status and raise notification
        /// </summary>
        /// <param name="provider">
        /// The provider to reset
        /// </param>
        /// <param name="dispatcher">
        /// the dispatcher
        /// </param>
        /// <param name="propertyLambda">
        /// The property to raise notification on
        /// </param>
        /// <param name="propertyLambdas">
        /// </param>
        [DebuggerStepThrough]
        protected void ResetAndRaise(
            IDataProvider provider, 
            IDispatcher dispatcher, 
            Expression<Func<T, object>> propertyLambda, 
            params Expression<Func<T, object>>[] propertyLambdas)
        {
            if (provider.Status.Value != DataProviderStatus.NotReady)
            {
                provider.Reset();
                RaisePropertyChanged(dispatcher, propertyLambda, propertyLambdas);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="propertyChangedEventArgs">
        /// </param>
        [DebuggerStepThrough]
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            ISet<Action> set;
            if (_dependencyActions.TryGetValue(propertyChangedEventArgs.PropertyName, out set))
            {
                set.ForEach(z => z());
            }
        }

        #endregion
    }
}