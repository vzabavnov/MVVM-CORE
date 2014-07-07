using System.Diagnostics.Contracts;
using System.Reflection;

namespace Zabavnov.MVVM
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;

    /// <summary>
    /// </summary>
    /// <typeparam name="TControl">
    /// </typeparam>
    /// <typeparam name="TProperty">
    /// </typeparam>
    public class SimpleBindableProperty<TControl, TProperty> : IBindableProperty<TControl, TProperty>
        where TControl : class
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly TControl _control;

        /// <summary>
        /// </summary>
        private readonly string _propertyName;

        /// <summary>
        /// </summary>
        private TProperty _value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="control">
        /// </param>
        /// <param name="propertyName">
        /// </param>
        public SimpleBindableProperty(TControl control, string propertyName)
        {
            Contract.Requires(control != null);
            Contract.Requires(!String.IsNullOrEmpty(propertyName));

            _control = control;
            _propertyName = propertyName;
        }

        /// <summary>
        /// </summary>
        /// <param name="control">
        /// </param>
        /// <param name="propertyLambda">
        /// </param>
        public SimpleBindableProperty(TControl control, Expression<Func<TControl, object>> propertyLambda)
        {
            Contract.Requires(control != null);
            Contract.Requires(propertyLambda != null);

            _control = control;
            _propertyName =  propertyLambda.GetMemberInfo().Name;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///     the property ca be read
        /// </summary>
        public bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///     The property can be write
        /// </summary>
        public bool CanWrite
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///     The control that property bound to
        /// </summary>
        public TControl Control
        {
            get
            {
                return _control;
            }
        }

        /// <summary>
        ///     The property name
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }

        /// <summary>
        /// use this value when <see cref="BindingMode"/> is equal to default
        /// </summary>
        public BindingMode DefaultBindingMode
        {
            get { return BindingMode.TwoWay;}
        }

        /// <summary>
        ///     The value of property
        /// </summary>
        public TProperty Value
        {
            [DebuggerStepThrough]
            get
            {
                return _value;
            }

            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(_propertyName));
                    }
                }
            }
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_control != null);
            Contract.Invariant(!string.IsNullOrWhiteSpace(_propertyName));

        }
    }
}