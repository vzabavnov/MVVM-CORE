namespace Zabavnov.WFMVVM
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;

    public class SimpleBindableProperty<TControl, TProperty> : IBindableProperty<TControl, TProperty>
    {
        private readonly TControl _control;
        private readonly string _propertyName;
        private TProperty _value;

        public SimpleBindableProperty(TControl control, string propertyName)
        {
            this._control = control;
            this._propertyName = propertyName;
        }

        public SimpleBindableProperty(TControl control, Expression<Func<TControl, object>> propertyLambda)
        {
            this._control = control;
            this._propertyName = propertyLambda.GetPropertyName();
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of IBindableProperty<out SimpleBindableProperty<TProperty>,TProperty>

        /// <summary>
        ///     the property ca be read
        /// </summary>
        public bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        ///     The property can be write
        /// </summary>
        public bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        ///     The control tha property binded to
        /// </summary>
        public TControl Control
        {
            get { return this._control; }
        }

        /// <summary>
        ///     The value of property
        /// </summary>
        public TProperty Value
        {
            [DebuggerStepThrough]
            get
            {
                return this._value;
            }
            set
            {
                if(!Equals(this._value, value))
                {
                    this._value = value;
                    if(this.PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs(this._propertyName));
                }
            }
        }

        /// <summary>
        ///     The property name
        /// </summary>
        public string PropertyName
        {
            get { return this._propertyName; }
        }

        #endregion
    }
}