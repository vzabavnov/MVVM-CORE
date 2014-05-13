namespace Zabavnov.WFMVVM
{
    using System;

    /// <summary>
    ///     implements <see cref="IDataConverter{TSource,TTarget}" /> for same target and source types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataConverter<T>
    {
        public static readonly IDataConverter<T, T> EmptyConverter = new DataConverter<T, T>(arg => arg, arg => arg);
    }

    /// <summary>
    ///     implements <see cref="IDataConverter{TSource,TTarget}" />
    /// </summary>
    /// <typeparam name="TSource">
    ///     The type of source element. this type will be use as input for method <see cref="ConvertTo" />
    /// </typeparam>
    /// <typeparam name="TTarget">
    ///     The type of target element. This type will be used as input for method <see cref="ConvertFrom" />
    /// </typeparam>
    public class DataConverter<TSource, TTarget> : IDataConverter<TSource, TTarget>
    {
        private readonly Func<TTarget, TSource> _convertFrom;
        private readonly Func<TSource, TTarget> _convertTo;

        public DataConverter(Func<TSource, TTarget> convertTo, Func<TTarget, TSource> convertFrom)
        {
            this._convertTo = convertTo;
            this._convertFrom = convertFrom;
        }

        #region Implementation of IDataConverter<TSource,TTarget>

        public TTarget ConvertTo(TSource source)
        {
            return this._convertTo(source);
        }

        public TSource ConvertFrom(TTarget target)
        {
            return this._convertFrom(target);
        }

        #endregion
    }
}