using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM
{
    using System;

    /// <summary>
    /// implements <see cref="IDataConverter{TSource,TTarget}"/> for same target and source types
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class DataConverter<T>
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        public static readonly IDataConverter<T, T> EmptyConverter = new DataConverter<T, T>(arg => arg, arg => arg);

        #endregion
    }

    /// <summary>
    /// implements <see cref="IDataConverter{TSource,TTarget}"/>
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of source element. this type will be use as input for method <see cref="ConvertTo"/>
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The type of target element. This type will be used as input for method <see cref="ConvertFrom"/>
    /// </typeparam>
    public class DataConverter<TSource, TTarget> : IDataConverter<TSource, TTarget>
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Func<TTarget, TSource> _convertFrom;

        /// <summary>
        /// </summary>
        private readonly Func<TSource, TTarget> _convertTo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="convertTo">
        /// </param>
        /// <param name="convertFrom">
        /// </param>
        public DataConverter(Func<TSource, TTarget> convertTo, Func<TTarget, TSource> convertFrom)
        {
            Contract.Requires(convertFrom != null);
            Contract.Requires(convertTo != null);

            _convertTo = convertTo;
            _convertFrom = convertFrom;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        public TSource ConvertFrom(TTarget target)
        {
            return _convertFrom(target);
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        public TTarget ConvertTo(TSource source)
        {
            return _convertTo(source);
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._convertFrom != null);
            Contract.Invariant(this._convertTo != null);
        }
    }
}