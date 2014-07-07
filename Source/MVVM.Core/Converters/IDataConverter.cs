namespace Zabavnov.MVVM
{
    /// <summary>
    ///     Defines interface for converting data from <typeparamref name="TSource" /> to <typeparamref name="TTarget" />
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public interface IDataConverter<TSource, TTarget>
    {
        /// <summary>
        ///     Convert date from type <typeparamref name="TSource" /> to <typeparamref name="TTarget" />
        /// </summary>
        /// <param name="source">Source data</param>
        /// <returns></returns>
        TTarget ConvertTo(TSource source);

        /// <summary>
        ///     Convert back data from <typeparamref name="TTarget" /> to <typeparamref name="TSource" />
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        TSource ConvertFrom(TTarget target);
    }
}