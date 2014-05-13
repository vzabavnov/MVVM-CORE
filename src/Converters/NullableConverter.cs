namespace Zabavnov.WFMVVM
{
    using System.Collections.Generic;

    /// <summary>
    ///     defines converter to <see cref="NullableComparer{T}" /> type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullableConverter<T> : IDataConverter<T, T?> where T : struct
    {
        private readonly IEqualityComparer<T> _comparer;
        private readonly T _defaultValue;

        public NullableConverter(T defaultValue, IEqualityComparer<T> comparer = null)
        {
            this._defaultValue = defaultValue;
            this._comparer = comparer ?? EqualityComparer<T>.Default;
        }

        #region Implementation of IDataConverter<T,T?>

        public T? ConvertTo(T source)
        {
            return this._comparer.Equals(source, this._defaultValue) ? (T?) null : source;
        }

        public T ConvertFrom(T? target)
        {
            return target.HasValue ? target.Value : this._defaultValue;
        }

        #endregion
    }
}