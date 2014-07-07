using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System.Collections.Generic;

    /// <summary>
    /// defines converter to <see cref="NullableComparer{T}"/> type
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class NullableConverter<T> : IDataConverter<T, T?>
        where T : struct
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly IEqualityComparer<T> _comparer;

        /// <summary>
        /// </summary>
        private readonly T _defaultValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="defaultValue">
        /// </param>
        /// <param name="comparer">
        /// </param>
        public NullableConverter(T defaultValue, IEqualityComparer<T> comparer = null)
        {
            _defaultValue = defaultValue;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        public T ConvertFrom(T? target)
        {
            return target.HasValue ? target.Value : _defaultValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        public T? ConvertTo(T source)
        {
            return _comparer.Equals(source, _defaultValue) ? (T?)null : source;
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._comparer != null);
        }
    }
}