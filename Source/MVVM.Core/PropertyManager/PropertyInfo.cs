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
//  File Name: PropertyInfo.cs.
//  Created: 2014/05/30/5:02 PM.
//  Modified: 2014/06/09/3:59 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

#endregion

namespace Zabavnov.MVVM
{
    public class PropertyInfo<T, TProperty> : IPropertyInfo<TProperty>
    {
        #region Fields

        private IEqualityComparer<TProperty> _comparer;

        private bool _initialized;

        private Func<TProperty, bool> _validator;
        private readonly string _name;

        #endregion

        #region Constructors and Destructors

        public PropertyInfo(Expression<Func<T, TProperty>> propertyLambda)
        {
            Contract.Requires(propertyLambda != null);
            
            _name = propertyLambda.GetMemberInfo().Name;
        }

        #endregion

        #region Public Events

        public event Action<IPropertyInfo> Changed;

        #endregion

        #region Public Properties

        public IEqualityComparer<TProperty> Comparer
        {
            [DebuggerStepThrough]
            get { return _comparer ?? (_comparer = EqualityComparer<TProperty>.Default); }
            [DebuggerStepThrough]
            set { _comparer = value; }
        }

        public Func<TProperty> Getter { get; set; }

        public bool HasChanged => !Comparer.Equals(Value, OriginalValue);

        public string Name => _name;
        
        public TProperty OriginalValue { get; private set; }

        public Action<TProperty> Setter { get; set; }

        public Func<TProperty, bool> Validator
        {
            get { return _validator ?? (_validator = t => true); }
            set { _validator = value; }
        }

        public TProperty Value
        {
            get
            {
                Contract.Assume(Getter != null);

                return Getter();
            }
            set
            {
                Contract.Assume(Getter != null);

                var val = Getter();
                if(!Comparer.Equals(val, value) && Validator(value))
                {
                    if(!_initialized)
                    {
                        _initialized = true;
                        OriginalValue = value;
                    }
                    Setter(value);

                    OnChanged(this);
                }
            }
        }

        #endregion

        #region Methods

        protected virtual void OnChanged(IPropertyInfo obj)
        {
            Changed?.Invoke(obj);
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Comparer != null);
            Contract.Invariant(!string.IsNullOrWhiteSpace(_name));
        }
    }
}