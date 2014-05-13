namespace Zabavnov.WFMVVM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    /// <summary>
    ///     Implements <see cref="IBindableProperty{TControl,TProperty}" /> where type of control property and value property
    ///     is not the same
    /// </summary>
    /// <typeparam name="TControl">The control type to bind to</typeparam>
    /// <typeparam name="TControlProperty">The type of control property</typeparam>
    /// <typeparam name="TValueProperty">The type of value property</typeparam>
    //[DebuggerStepThrough]
    public class BindableProperty<TControl, TControlProperty, TValueProperty> : IBindableProperty<TControl, TValueProperty>
    {
        private readonly IDataConverter<TControlProperty, TValueProperty> _converter;
        private readonly PropertyChangedEventArgs _eventArg;
        private readonly Func<TControl, TControlProperty> _getter;
        private readonly Action<TControl, TControlProperty> _setter;
        private IEqualityComparer<TValueProperty> _comparer;
        private bool _notificationEnabled = true;

        /// <summary>
        ///     create new instance of bindable property
        /// </summary>
        /// <param name="control">The control to bind to</param>
        /// <param name="propertyName">
        ///     The name of property. this name will be used to raise event on
        ///     <see cref="INotifyPropertyChanged" />
        /// </param>
        /// <param name="getter">The method to get value from control's property</param>
        /// <param name="setter">The method to set value to control's property</param>
        /// <param name="converter">The converter between control's property and value</param>
        public BindableProperty(TControl control, string propertyName, Func<TControl, TControlProperty> getter,
                                Action<TControl, TControlProperty> setter, IDataConverter<TControlProperty, TValueProperty> converter)
        {
            Contract.Requires(!ReferenceEquals(control, null));
            Contract.Requires(getter != null || setter != null);

            this.Control = control;
            this._getter = getter;
            this._setter = setter;
            this._converter = converter;
            this._eventArg = new PropertyChangedEventArgs(propertyName);
        }

        public IEqualityComparer<TValueProperty> Comparer
        {
            get { return this._comparer ?? (this._comparer = EqualityComparer<TValueProperty>.Default); }
            set { this._comparer = value ?? EqualityComparer<TValueProperty>.Default; }
        }

        #region Implementation of IBindableProperty<out TControl,TProperty>

        /// <summary>
        ///     The property of control is readable
        /// </summary>
        [Pure]
        public bool CanRead
        {
            get { return this._getter != null; }
        }

        /// <summary>
        ///     The property of control is writable
        /// </summary>
        [Pure]
        public bool CanWrite
        {
            get { return this._setter != null; }
        }

        /// <summary>
        ///     the control tha property binded to
        /// </summary>
        public TControl Control { get; private set; }

        /// <summary>
        ///     The value of property. it ises onverter to get or set value to the control
        /// </summary>
        public TValueProperty Value
        {
            get
            {
                Contract.Requires(this.CanRead);
                return this._converter.ConvertTo(this._getter(this.Control));
            }
            set
            {
                Contract.Requires(this.CanWrite);
                if(!this.Comparer.Equals(this.Value, value))
                {
                    this._notificationEnabled = false;
                    this._setter(this.Control, this._converter.ConvertFrom(value));
                    this._notificationEnabled = true;

                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        ///     The name of property
        /// </summary>
        public string PropertyName
        {
            get { return this._eventArg.PropertyName; }
        }

        public virtual void NotifyPropertyChanged()
        {
            if(this._notificationEnabled)
            {
                var handler = this.PropertyChanged;
                if(handler != null)
                    handler(this.Control, this._eventArg);
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    /// <summary>
    ///     Implements <see cref="IBindableProperty{TControl,TProperty}" />
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    /// <typeparam name="TControlProperty"></typeparam>
    [DebuggerStepThrough]
    public class BindableProperty<TControl, TControlProperty> : BindableProperty<TControl, TControlProperty, TControlProperty>
    {
        public BindableProperty(TControl control, string propertyName, Func<TControl, TControlProperty> getter,
                                Action<TControl, TControlProperty> setter)
            : base(control, propertyName, getter, setter, DataConverter<TControlProperty>.EmptyConverter)
        {
        }

        public BindableProperty(TControl control, Expression<Func<TControl, object>> propertyLambda, Func<TControl, TControlProperty> getter,
                                Action<TControl, TControlProperty> setter)
            : base(control, propertyLambda.GetPropertyName(), getter, setter, DataConverter<TControlProperty>.EmptyConverter)
        {
        }
    }
}