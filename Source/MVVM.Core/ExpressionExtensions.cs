#region Proprietary  Notice

//  ****************************************************************************
//    Copyright 2014 Vadim Zabavnov
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
//  ****************************************************************************
//  File Name: ExpressionExtensions.cs.
//  Created: 2014/06/10/3:28 PM.
//  Modified: 2014/06/13/11:56 AM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#endregion

namespace Zabavnov.MVVM
{
    public static class ExpressionExtensions
    {
        #region Static Fields

        private static readonly Dictionary<Action, IDictionary<string, AttachedAction>> _attachedActions =
            new Dictionary<Action, IDictionary<string, AttachedAction>>();

        #endregion

        #region Public Methods and Operators

        public static void AttachActionTo(
            this Action actionToAttach,
            Expression<Func<object>> propertyLambda,
            params Expression<Func<object>>[] propertyLambdas)
        {
            Contract.Requires(actionToAttach != null);
            Contract.Requires(propertyLambda != null);

            AttachActionTo(actionToAttach, propertyLambdas.AddHead(propertyLambda));
        }

        public static void AttachActionTo<TProperty>(this Action actionToAttach, Expression<Func<TProperty>> propertyLambda)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Requires(actionToAttach != null);

            MemberInfo mi;
            var instance = GetPropertyContainer<INotifyPropertyChanged, TProperty>(propertyLambda, out mi)();

            lock (_attachedActions)
            {
                IDictionary<string, AttachedAction> actions;
                if(!_attachedActions.TryGetValue(actionToAttach, out actions))
                {
                    actions = new Dictionary<string, AttachedAction>();
                    _attachedActions.Add(actionToAttach, actions);
                }

                Contract.Assume(actions != null);

                AttachedAction attachment;
                if (!actions.TryGetValue(mi.Name, out attachment))
                {
                    attachment = new AttachedAction(mi.Name, actionToAttach);
                    actions.Add(mi.Name, attachment);
                }

                instance .PropertyChanged += attachment.NotyfyPropertyChanged;
            }
        }

        /// <summary>
        ///     attach <paramref name="actionToAttach" /> to event on <see cref="INotifyPropertyChanged" /> for property specified
        ///     in <paramref name="propertyLambdas" />
        /// </summary>
        /// <param name="actionToAttach">The action to attach to</param>
        /// <param name="propertyLambdas">The lambda expression for property to attach action to</param>
        public static void AttachActionTo(this Action actionToAttach, IEnumerable<Expression<Func<object>>> propertyLambdas)
        {
            Contract.Requires(propertyLambdas != null);
            Contract.Requires(actionToAttach != null);

            var infos = propertyLambdas.Select(
                lambda =>
                    {
                        MemberInfo mi;
                        var instanse = GetPropertyContainer<INotifyPropertyChanged, object>(lambda, out mi)();

                        return new { Instance = instanse, MemberInfo = mi };
                    });

            lock(_attachedActions)
            {
                foreach(var info in infos)
                {
                    IDictionary<string, AttachedAction> actions;
                    if(!_attachedActions.TryGetValue(actionToAttach, out actions))
                    {
                        actions = new Dictionary<string, AttachedAction>();
                        _attachedActions.Add(actionToAttach, actions);
                    }

                    Contract.Assume(actions != null);

                    AttachedAction attachment;
                    if(!actions.TryGetValue(info.MemberInfo.Name, out attachment))
                    {
                        attachment = new AttachedAction(info.MemberInfo.Name, actionToAttach);
                        actions.Add(info.MemberInfo.Name, attachment);
                    }

                    info.Instance.PropertyChanged += attachment.NotyfyPropertyChanged;
                }
            }
        }

        public static void DettachActionFrom(
            this Action actionToAttach,
            Expression<Func<object>> propertyLambda,
            params Expression<Func<object>>[] propertyLambdas)
        {
            Contract.Requires(actionToAttach != null);
            Contract.Requires(propertyLambda != null);

            DettachActionFrom(actionToAttach, propertyLambdas.AddHead(propertyLambda));
        }

        public static void DettachActionFrom<TProperty>(this Action actionToAttach, Expression<Func<TProperty>> propertyLambda)
        {
            Contract.Requires(propertyLambda != null);

            MemberInfo mi;
            var instance = GetPropertyContainer<INotifyPropertyChanged, TProperty>(propertyLambda, out mi)();

            lock (_attachedActions)
            {
                IDictionary<string, AttachedAction> actions;
                AttachedAction attachment;
                if(_attachedActions.TryGetValue(actionToAttach, out actions) && actions.TryGetValue(mi.Name, out attachment))
                    instance.PropertyChanged -= attachment.NotyfyPropertyChanged;
            }
        }

        /// <summary>
        ///     detach <paramref name="actionToAttach" /> from properties specified by <paramref name="propertyLambdas" />
        /// </summary>
        /// <param name="actionToAttach"></param>
        /// <param name="propertyLambdas"></param>
        public static void DettachActionFrom(this Action actionToAttach, IEnumerable<Expression<Func<object>>> propertyLambdas)
        {
            Contract.Requires(propertyLambdas != null);
            Contract.Requires(actionToAttach != null);

            var infos = propertyLambdas.Select(
                lambda =>
                    {
                        MemberInfo mi;
                        var instanse = GetPropertyContainer<INotifyPropertyChanged, object>(lambda, out mi)();

                        return new { Instance = instanse, MemberInfo = mi };
                    });

            lock(_attachedActions)
            {
                foreach(var info in infos)
                {
                    IDictionary<string, AttachedAction> actions;
                    AttachedAction attachment;
                    if(_attachedActions.TryGetValue(actionToAttach, out actions) && actions.TryGetValue(info.MemberInfo.Name, out attachment))
                        info.Instance.PropertyChanged -= attachment.NotyfyPropertyChanged;
                }
            }
        }

        public static Func<T> GetPropertyContainer<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda, out MemberInfo memberInfo)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Ensures(Contract.Result<Func<T>>() != null);

            var exp = Expression.Lambda<Func<T>>(GetPropertyExpression(propertyLambda, out memberInfo));
#if DEBUG
            var getter = exp.Compile(DebugInfoGenerator.CreatePdbGenerator());
#else
            var getter = exp.Compile();
#endif
            return getter;
        }

        /// <summary>
        ///     Gets delegate that returns instance of container pro property specified by <paramref name="propertyLambda" />
        /// </summary>
        /// <typeparam name="T">The container class type</typeparam>
        /// <typeparam name="TProperty">The type of property</typeparam>
        /// <param name="propertyLambda">The lambda expression for property</param>
        /// <param name="memberInfo">
        ///     The <see cref="MemberInfo" /> for the property specified by <paramref name="propertyLambda" />
        /// </param>
        /// <returns></returns>
        public static Func<T> GetPropertyContainer<T, TProperty>(Expression<Func<TProperty>> propertyLambda, out MemberInfo memberInfo)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Ensures(Contract.Result<Func<T>>() != null);

            var exp = Expression.Lambda<Func<T>>(GetPropertyExpression(propertyLambda, out memberInfo));
#if DEBUG
            var getter = exp.Compile(DebugInfoGenerator.CreatePdbGenerator());
#else
            var getter = exp.Compile();
#endif
            return getter;
        }

        public static MemberInfo GetMemberInfo<T, TProperty>(this Expression<Func<T, TProperty>> propertyLambda)
        {
            Contract.Requires(propertyLambda != null);

            MemberExpression expression = null;
            switch (propertyLambda.Body.NodeType)
            {
                case ExpressionType.Convert:
                    expression = ((UnaryExpression)propertyLambda.Body).Operand as MemberExpression;
                    break;
                case ExpressionType.MemberAccess:
                    expression = propertyLambda.Body as MemberExpression;
                    break;
            }

            if (expression == null)
                throw new ArgumentException(String.Format("Expression '{0}' refers to a method, not a property or field.", propertyLambda));

            return expression.Member;
        }

        public static MemberExpression GetPropertyExpression<T, TProperty>(
            this Expression<Func<T, TProperty>> propertyLambda,
            out MemberInfo memberInfo)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Ensures(Contract.Result<Expression>() != null);

            MemberExpression expression = null;
            switch(propertyLambda.Body.NodeType)
            {
                case ExpressionType.Convert:
                    expression = ((UnaryExpression)propertyLambda.Body).Operand as MemberExpression;
                    break;
                case ExpressionType.MemberAccess:
                    expression = propertyLambda.Body as MemberExpression;
                    break;
            }

            if(expression == null)
                throw new ArgumentException(String.Format("Expression '{0}' refers to a method, not a property or field.", propertyLambda));

            memberInfo = expression.Member;
            return expression;
        }

        public static Expression GetPropertyExpression<TProperty>(Expression<Func<TProperty>> propertyLambda, out MemberInfo memberInfo)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Ensures(Contract.Result<Expression>() != null);

            var body = propertyLambda.Body;
            MemberExpression expression;
            switch(body.NodeType)
            {
                case ExpressionType.Convert:
                    expression = (MemberExpression)((UnaryExpression)body).Operand;
                    break;
                case ExpressionType.MemberAccess:
                    expression = (MemberExpression)body;
                    break;
                default:
                    throw new ArgumentException("Only property supported");
            }

            memberInfo = expression.Member;

            return expression.Expression;
        }

        /// <summary>
        ///     Get setter action for property specified by <paramref name="propertyLambda" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyLambda"></param>
        /// <returns></returns>
        public static Action<T, TProperty> GetPropertySetter<T, TProperty>(this Expression<Func<T, TProperty>> propertyLambda)
        {
            Contract.Requires(propertyLambda != null);
            Contract.Ensures(Contract.Result<Action<T, TProperty>>() != null);

            var sourceParameter = propertyLambda.Parameters[0];
            var memberParameter = Expression.Parameter(typeof(TProperty), "member");

            var memberInfo = GetMemberInfo(propertyLambda);
            
            if((memberInfo.MemberType == MemberTypes.Property && ((PropertyInfo)memberInfo).CanWrite)
               || (memberInfo.MemberType == MemberTypes.Field && !((FieldInfo)memberInfo).IsInitOnly))
            {
                var assign = Expression.Assign(propertyLambda.Body, memberParameter);
                var newExpression = Expression.Lambda<Action<T, TProperty>>(assign, sourceParameter, memberParameter);

                return newExpression.Compile(DebugInfoGenerator.CreatePdbGenerator());
            }

            throw new ArgumentException("Cannot write to specified member");
        }

        /// <summary>
        ///     raise <see cref="INotifyPropertyChanged.PropertyChanged" /> event on objects specified by
        ///     <paramref name="propertyLambdas" />
        /// </summary>
        /// <param name="propertyLambdas"></param>
        public static void RaisePropertyChangedEvent(params Expression<Func<object>>[] propertyLambdas)
        {
            Contract.Requires(propertyLambdas != null);
            Contract.Requires(propertyLambdas.Length > 0);

            foreach(var lambda in propertyLambdas)
            {
                MemberInfo memberInfo;
                var instanse = GetPropertyContainer<INotifyPropertyChanged, object>(lambda, out memberInfo)();

                // get the internal eventDelegate
                var bindableObjectType = instanse.GetType();

                const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;

                // search the base type, which contains the PropertyChanged event field.
                FieldInfo propChangedFieldInfo = null;
                while(bindableObjectType != null)
                {
                    propChangedFieldInfo = bindableObjectType.GetField("PropertyChanged", BINDING_FLAGS);
                    if(propChangedFieldInfo != null)
                        break;

                    bindableObjectType = bindableObjectType.BaseType;
                }
                if(propChangedFieldInfo == null)
                    return;

                // get prop changed event field value
                var fieldValue = propChangedFieldInfo.GetValue(instanse);
                if(fieldValue == null)
                    return;

                var eventDelegate = fieldValue as MulticastDelegate;
                if(eventDelegate == null)
                    return;

                // get invocation list
                var delegates = eventDelegate.GetInvocationList();

                // invoke each delegate
                foreach(var propertyChangedDelegate in delegates)
                {
                    propertyChangedDelegate.Method.Invoke(
                        propertyChangedDelegate.Target,
                        new object[] { instanse, new PropertyChangedEventArgs(memberInfo.Name) });
                }
            }
        }

        #endregion

        private class AttachedAction
        {
            #region Fields

            private readonly Action _actionToAttach;
            private readonly string _propertyName;

            #endregion

            #region Constructors and Destructors

            public AttachedAction(string propertyName, Action actionToAttach)
            {
                Contract.Requires(!String.IsNullOrEmpty(propertyName));
                Contract.Requires(actionToAttach != null);

                _propertyName = propertyName;
                _actionToAttach = actionToAttach;
            }

            #endregion

            #region Public Methods and Operators

            public void NotyfyPropertyChanged(object sender, PropertyChangedEventArgs args)
            {
                if(args.PropertyName == _propertyName)
                    _actionToAttach();
            }

            #endregion

            #region Methods

            [ContractInvariantMethod]
            private void ObjectInvatiant()
            {
                Contract.Invariant(_actionToAttach != null);
                Contract.Invariant(_propertyName != null);
            }

            #endregion
        }
    }
}