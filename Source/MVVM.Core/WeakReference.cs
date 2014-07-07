namespace Zabavnov.MVVM
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The implementation of strong typed weak referenced instance
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class WeakReference<T>
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Func<T> _provider;

        /// <summary>
        /// </summary>
        private WeakReference _reference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// create new instance of <see cref="WeakReference{T}"/>
        /// </summary>
        /// <param name="provider">
        /// The data provider
        /// </param>
        public WeakReference(Func<T> provider)
        {
            Contract.Requires(provider != null);

            _provider = provider;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     returns true is the instance is alive
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return _reference != null && _reference.IsAlive;
            }
        }

        /// <summary>
        ///     Gets the instance of object currently referenced by <see cref="WeakReference" />
        /// </summary>
        public T Target
        {
            get
            {
                if (_reference == null || !_reference.IsAlive)
                {
                    _reference = new WeakReference(_provider());
                }

                return (T)_reference.Target;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     force to release reference
        /// </summary>
        public void Release()
        {
            _reference = null;
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_provider != null);
        }
    }
}