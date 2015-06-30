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
//  File Name: DateTimeWithRange.cs.
//  Created: 2014/05/30/5:11 PM.
//  Modified: 2014/06/10/3:18 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

#endregion

namespace Zabavnov.MVVM
{
    /// <summary>
    /// </summary>
    public class DateTimeWithRange : DateTimeWithRange<DateTimeWithRange>
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public DateTimeWithRange()
        {
            Initialize(
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "Start"),
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "Value"),
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "End"));
        }

        /// <summary>
        /// </summary>
        /// <param name="start">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <param name="end">
        /// </param>
        public DateTimeWithRange(DateTime? start, DateTime value, DateTime? end)
        {
            Contract.Requires(!start.HasValue || !end.HasValue || !(end > start), "Start value must be before or equal End value");

            Initialize(
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "Start") { Value = start },
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "Value") { Value = value },
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "End") { Value = end });

            Validate();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Bind boundaries of two <see cref="DateTimeWithRange" /> values
        /// </summary>
        /// <param name="lower">
        ///     The lower date. the value of this will control start value od upper range
        /// </param>
        /// <param name="upper">
        ///     The upper date. the value of this will control end value of lower range
        /// </param>
        /// <param name="updateValue">
        ///     if set to true then value of <see cref="DateTimeWithRange" /> will be change to be in range
        /// </param>
        /// <returns>
        /// </returns>
        public static BoundaryBinder BoundaryBind(IDateTimeWithRange lower, IDateTimeWithRange upper, BoundaryBinderStrategy updateValue)
        {
            Contract.Requires(lower != null);
            Contract.Requires(upper != null);

            var binder = new BoundaryBinder(lower, upper) { UpdateStrategy = updateValue };
            binder.Bind();
            return binder;
        }

        /// <summary>
        ///     validate if date in range from <paramref name="start" /> to <paramref name="end" />
        /// </summary>
        /// <param name="start">
        /// </param>
        /// <param name="date">
        ///     The value to check
        /// </param>
        /// <param name="end">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool Validate(DateTime? start, DateTime date, DateTime? end)
        {
            return (!start.HasValue || date >= start) && (!end.HasValue || date <= end);
        }

        #endregion
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TControl">
    /// </typeparam>
    public class DateTimeWithRange<TControl> : ModelBase<DateTimeWithRange<TControl>>, IDateTimeWithRange
        where TControl : class
    {
        #region Fields

        /// <summary>
        /// </summary>
        protected IBindableProperty<TControl, DateTime?> _endProperty;

        /// <summary>
        /// </summary>
        protected IBindableProperty<TControl, DateTime?> _startProperty;

        /// <summary>
        /// </summary>
        protected IBindableProperty<TControl, DateTime?> _valueProperty;

        /// <summary>
        /// </summary>
        private bool _isValid = true;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="startProperty">
        /// </param>
        /// <param name="valueProperty">
        /// </param>
        /// <param name="endProperty">
        /// </param>
        public DateTimeWithRange(
            IBindableProperty<TControl, DateTime?> startProperty,
            IBindableProperty<TControl, DateTime?> valueProperty,
            IBindableProperty<TControl, DateTime?> endProperty)
            : this()
        {
            Contract.Requires(endProperty != null);
            Contract.Requires(startProperty != null);
            Contract.Requires(valueProperty != null);

            Initialize(startProperty, valueProperty, endProperty);
        }

        /// <summary>
        /// </summary>
        /// <param name="valueProperty">
        /// </param>
        public DateTimeWithRange(IBindableProperty<TControl, DateTime?> valueProperty)
            : this()
        {
            Contract.Requires(valueProperty != null);

            Initialize(
                new SimpleBindableProperty<TControl, DateTime?>(valueProperty.Control, "Start"),
                valueProperty,
                new SimpleBindableProperty<TControl, DateTime?>(valueProperty.Control, "End"));

            Validate();
        }

        /// <summary>
        /// </summary>
        protected DateTimeWithRange()
        {
            BusinessLogic();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public DateTime? Start
        {
            [DebuggerStepThrough]
            get
            {
                Contract.Assume(this._startProperty.CanRead);

                return this._startProperty.Value;
            }

            [DebuggerStepThrough]
            set
            {
                Contract.Assume(this._startProperty.CanWrite);
                this._startProperty.Value = value;
            }
        }

        /// <summary>
        /// </summary>
        public DateTime? Value
        {
            [DebuggerStepThrough]
            get
            {
                Contract.Requires(this._valueProperty.CanRead);

                return this._valueProperty.Value;
            }
            [DebuggerStepThrough]
            set
            {
                Contract.Requires(this._valueProperty.CanWrite);

                this._valueProperty.Value = value;
            }
        }

        /// <summary>
        /// </summary>
        public DateTime? End
        {
            [DebuggerStepThrough]
            get
            {
                Contract.Requires(_endProperty.CanRead);

                return _endProperty.Value;
            }

            [DebuggerStepThrough]
            set
            {
                Contract.Requires(_endProperty.CanWrite);

                _endProperty.Value = value;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsValid
        {
            [DebuggerStepThrough]
            get
            {
                return _isValid;
            }

            [DebuggerStepThrough]
            private set
            {
                if(_isValid != value)
                {
                    _isValid = value;
                    RaisePropertyChanged(z => z.IsValid);
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="dwr">
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator DateTime(DateTimeWithRange<TControl> dwr)
        {
            Contract.Requires(dwr != null);

            return dwr.Value.HasValue ? dwr.Value.Value : DateTime.MinValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="dwr">
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator DateTime?(DateTimeWithRange<TControl> dwr)
        {
            Contract.Requires(dwr != null);

            return dwr.Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="date">
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator DateTimeWithRange<TControl>(DateTime date)
        {
            return new DateTimeWithRange<TControl> { Value = date == DateTime.MinValue ? (DateTime?)null : date };
        }

        /// <summary>
        /// </summary>
        /// <param name="date">
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator DateTimeWithRange<TControl>(DateTime? date)
        {
            return new DateTimeWithRange<TControl> { Value = date };
        }

        /// <summary>
        /// </summary>
        public void Validate()
        {
            IsValid = !Value.HasValue || DateTimeWithRange.Validate(Start, Value.Value, End);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="startProperty">
        /// </param>
        /// <param name="valueProperty">
        /// </param>
        /// <param name="endProperty">
        /// </param>
        protected void Initialize(
            IBindableProperty<TControl, DateTime?> startProperty,
            IBindableProperty<TControl, DateTime?> valueProperty,
            IBindableProperty<TControl, DateTime?> endProperty)
        {
            Contract.Requires(startProperty != null);
            Contract.Requires(valueProperty != null);
            Contract.Requires(endProperty != null);

            _startProperty = startProperty;
            _startProperty.PropertyChanged += (sender, args) => RaisePropertyChanged(() => Start);

            _valueProperty = valueProperty;
            _valueProperty.PropertyChanged += (sender, args) => RaisePropertyChanged(() => Value);

            _endProperty = endProperty;
            _endProperty.PropertyChanged += (sender, args) => RaisePropertyChanged(() => End);

            Validate();
        }

        /// <summary>
        /// </summary>
        private void BusinessLogic()
        {
            AttachActionTo(Validate, ()=> Start, () => End, () => Value);
        }

        /// <summary>
        /// </summary>
        [ContractInvariantMethod]
        private void InvariantObject()
        {
            Contract.Invariant(_valueProperty != null);
            Contract.Invariant(_startProperty != null);
            Contract.Invariant(_endProperty != null);
        }

        #endregion
    }
}