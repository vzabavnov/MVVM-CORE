namespace Zabavnov.WFMVVM
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    public class DateTimeWithRange : DateTimeWithRange<DateTimeWithRange>
    {
        public DateTimeWithRange()
        {
            this.Initialize(new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "Start"),
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "Value"),
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "End"));
        }

        public DateTimeWithRange(DateTime? start, DateTime value, DateTime? end)
        {
            Contract.Requires(!start.HasValue || !end.HasValue || !(end > start), "Start value must be before or equal End value");

            this.Initialize(new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "Start") {Value = start},
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "Value") {Value = value},
                new SimpleBindableProperty<DateTimeWithRange, DateTime?>(this, "End") {Value = end});

            this.Validate();
        }

        /// <summary>
        ///     validate if date in range from <paramref name="start" /> to <paramref name="end" />
        /// </summary>
        /// <param name="start"></param>
        /// <param name="date">The value to check</param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool Validate(DateTime? start, DateTime date, DateTime? end)
        {
            return (!start.HasValue || date >= start) && (!end.HasValue || date <= end);
        }

        /// <summary>
        ///     Bind boundaries of two <see cref="DateTimeWithRange" /> values
        /// </summary>
        /// <param name="lower">The lower date. the value of this will control start value od upper range</param>
        /// <param name="upper">The upper date. the value of this will control end value of lower range</param>
        /// <param name="updateValue">if set to true then value of <see cref="DateTimeWithRange" /> will be change to be in range</param>
        public static BoundaryBinder BoundaryBind(IDateTimeWithRange lower, IDateTimeWithRange upper, BoundaryBinderStrategy updateValue)
        {
            var binder = new BoundaryBinder(lower, upper) {UpdateStrategy = updateValue};
            binder.Bind();
            return binder;
        }
    }

    public class DateTimeWithRange<TControl> : ModelBase<DateTimeWithRange<TControl>>, IDateTimeWithRange
    {
        protected static readonly string START_NAME = Extensions.GetPropertyName<DateTimeWithRange<TControl>>(z => z.Start);
        protected static readonly string END_NAME = Extensions.GetPropertyName<DateTimeWithRange<TControl>>(z => z.End);
        protected static readonly string VALUE_NAME = Extensions.GetPropertyName<DateTimeWithRange<TControl>>(z => z.Value);

        protected IBindableProperty<TControl, DateTime?> _endProperty;
        private bool _isValid = true;
        protected IBindableProperty<TControl, DateTime?> _startProperty;
        protected IBindableProperty<TControl, DateTime?> _valueProperty;

        public DateTimeWithRange(IBindableProperty<TControl, DateTime?> startProperty, IBindableProperty<TControl, DateTime?> valueProperty,
                                 IBindableProperty<TControl, DateTime?> endProperty)
            : this()
        {
            this.Initialize(startProperty, valueProperty, endProperty);
        }

        public DateTimeWithRange(IBindableProperty<TControl, DateTime?> valueProperty)
            : this()
        {
            this.Initialize(new SimpleBindableProperty<TControl, DateTime?>(valueProperty.Control, "Start"), valueProperty,
                new SimpleBindableProperty<TControl, DateTime?>(valueProperty.Control, "End"));

            this.Validate();
        }

        protected DateTimeWithRange()
        {
            this.BusinessLogic();
        }

        #region IDateTimeWithRange Members

        public DateTime? Start
        {
            [DebuggerStepThrough]
            get
            {
                return this._startProperty.Value;
            }
            [DebuggerStepThrough]
            set
            {
                this._startProperty.Value = value;
            }
        }

        public DateTime? End
        {
            [DebuggerStepThrough]
            get
            {
                return this._endProperty.Value;
            }
            [DebuggerStepThrough]
            set
            {
                this._endProperty.Value = value;
            }
        }

        public DateTime? Value
        {
            get { return this._valueProperty.Value; }
            set { this._valueProperty.Value = value; }
        }

        public bool IsValid
        {
            [DebuggerStepThrough]
            get
            {
                return this._isValid;
            }
            [DebuggerStepThrough]
            private set
            {
                if(this._isValid != value)
                {
                    this._isValid = value;
                    this.RaisePropertyChanged(z => z.IsValid);
                }
            }
        }

        #endregion

        [ContractInvariantMethod]
        private void InvariantObject()
        {
            Contract.Invariant(this._valueProperty != null);
            Contract.Invariant(this._startProperty != null);
            Contract.Invariant(this._endProperty != null);
        }

        protected void Initialize(IBindableProperty<TControl, DateTime?> startProperty, IBindableProperty<TControl, DateTime?> valueProperty,
                                  IBindableProperty<TControl, DateTime?> endProperty)
        {
            this._startProperty = startProperty;
            this._startProperty.PropertyChanged += this.OnPropertyChanged;

            this._valueProperty = valueProperty;
            this._valueProperty.PropertyChanged += this.OnPropertyChanged;

            this._endProperty = endProperty;
            this._endProperty.PropertyChanged += this.OnPropertyChanged;

            this.Validate();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            this.Validate();
            this.RaisePropertyChanged(args);
        }

        private void BusinessLogic()
        {
            this.AddActionOn(this.Validate, z => z.Start, z => z.End, z => z.Value);
        }

        public void Validate()
        {
            this.IsValid = !this.Value.HasValue || DateTimeWithRange.Validate(this.Start, this.Value.Value, this.End);
        }

        #region operators

        public static implicit operator DateTime(DateTimeWithRange<TControl> dwr)
        {
            return dwr.Value.HasValue ? dwr.Value.Value : DateTime.MinValue;
        }

        public static implicit operator DateTimeWithRange<TControl>(DateTime date)
        {
            return new DateTimeWithRange<TControl> {Value = date == DateTime.MinValue ? (DateTime?) null : date};
        }

        public static implicit operator DateTime?(DateTimeWithRange<TControl> dwr)
        {
            return dwr.Value;
        }

        public static implicit operator DateTimeWithRange<TControl>(DateTime? date)
        {
            return new DateTimeWithRange<TControl> {Value = date};
        }

        #endregion
    }
}