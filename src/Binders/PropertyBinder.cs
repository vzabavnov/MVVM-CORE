namespace Zabavnov.WFMVVM
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    public class PropertyBinder<TControl, TControlProperty, TValueProperty> : IPropertyBinder<TControl, TValueProperty>
    {
        private readonly IDataConverter<TControlProperty, TValueProperty> _converter;
        private readonly Action<TControl, Action> _notifyActionSetter;

        public PropertyBinder(string propertyName, Func<TControl, TControlProperty> getter, Action<TControl, TControlProperty> setter,
                              IDataConverter<TControlProperty, TValueProperty> convert, Action<TControl, Action> notifyActionSetter = null)
        {
            Contract.Requires(!String.IsNullOrEmpty(propertyName));
            Contract.Requires(getter != null);
            Contract.Requires(setter != null);
            Contract.Requires(convert != null);

            this.PropertyName = propertyName;
            this.Getter = getter;
            this.Setter = setter;
            this._notifyActionSetter = notifyActionSetter;
            this._converter = convert;
        }

        public PropertyBinder(Expression<Func<TControl, TControlProperty>> propertyLambda, IDataConverter<TControlProperty, TValueProperty> converter)
            : this(propertyLambda, null, converter)
        {
        }

        public PropertyBinder(Expression<Func<TControl, TControlProperty>> propertyLambda, Action<TControl, Action> notifyActionSetter,
                              IDataConverter<TControlProperty, TValueProperty> converter)
        {
            this._notifyActionSetter = notifyActionSetter;
            this._converter = converter;

            this.PropertyName = propertyLambda.GetPropertyName();
            var info = propertyLambda.GetPropertyInfo();
            if(info.CanRead)
                this.Getter = propertyLambda.Compile();
            if(info.CanWrite)
                this.Setter = propertyLambda.GetSetter();
        }

        public Action<TControl, TControlProperty> Setter { get; set; }

        public bool CanWrite
        {
            get { return this.Setter != null; }
        }

        public bool CanRead
        {
            get { return this.Getter != null; }
        }

        public Func<TControl, TControlProperty> Getter { get; set; }

        #region IPropertyBinder<TControl,TValueProperty> Members

        public string PropertyName { get; private set; }

        public IBindableProperty<T, TValueProperty> BindTo<T>(T control) where T : TControl
        {
            var prop = new BindableProperty<T, TControlProperty, TValueProperty>(control, this.PropertyName, ctrl => this.Getter(ctrl),
                (ctrl, value) => this.Setter(ctrl, value), this._converter);

            if(this._notifyActionSetter != null)
                this._notifyActionSetter(control, prop.NotifyPropertyChanged);
            return prop;
        }

        #endregion
    }

    public class PropertyBinder<TControl, TProperty> : PropertyBinder<TControl, TProperty, TProperty>
    {
        public PropertyBinder(Expression<Func<TControl, TProperty>> propertyLambda)
            : this(propertyLambda, null)
        {
        }

        public PropertyBinder(Expression<Func<TControl, TProperty>> propertyLambda, Action<TControl, Action> notifyActionSetter)
            : base(propertyLambda, notifyActionSetter, DataConverter<TProperty>.EmptyConverter)
        {
        }

        public PropertyBinder(string propertyName, Func<TControl, TProperty> getter, Action<TControl, TProperty> setter,
                              Action<TControl, Action> notifyActionSetter = null)
            : base(propertyName, getter, setter, DataConverter<TProperty>.EmptyConverter, notifyActionSetter)
        {
        }
    }
}