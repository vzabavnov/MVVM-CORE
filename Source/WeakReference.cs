namespace Zabavnov.WFMVVM
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///     The implementation of strong typed weak referenced instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeakReference<T>
    {
        private WeakReference _reference;
        private readonly Func<T> _provider;

        /// <summary>
        ///     create new instance of <see cref="WeakReference{T}" />
        /// </summary>
        /// <param name="provider">The data provider</param>
        public WeakReference(Func<T> provider)
        {
            Contract.Requires(provider != null);

            this._provider = provider;
        }

        /// <summary>
        ///     Gets the instance of object currently referenced by <see cref="WeakReference" />
        /// </summary>
        public T Target
        {
            get
            {
                if (this._reference == null || !this._reference.IsAlive)
                    this._reference = new WeakReference(this._provider());
                return (T) this._reference.Target;
            }
        }

        /// <summary>
        ///     returns true is the instance is alive
        /// </summary>
        public bool IsAlive
        {
            get { return this._reference != null && this._reference.IsAlive; }
        }

        /// <summary>
        /// force to release reference
        /// </summary>
        public void Release()
        {
            this._reference = null;
        }
    }
}