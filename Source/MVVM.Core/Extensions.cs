#region Proprietary  Notice

//  ****************************************************************************
//    Copyright 2014 Vadim Zabavnov
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
//  ****************************************************************************
//  File Name: Extensions.cs.
//  Created: 2014/05/30/4:59 PM.
//  Modified: 2014/06/09/3:59 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

#endregion

namespace Zabavnov.MVVM
{
    public static class Extensions
    {
        #region Static Fields

        public static readonly TryParse<long> Int64Parser = Int64.TryParse;
        private static readonly IDictionary<Type, DataContractSerializer> _serializers = new Dictionary<Type, DataContractSerializer>();

        #endregion

        #region Delegates

        /// <summary>
        ///     the delegate for parsing the string
        /// </summary>
        /// <typeparam name="T">The type of value to return</typeparam>
        /// <param name="source">The source string to parse</param>
        /// <param name="result">The result value if successes</param>
        /// <returns>true - when parsed and false if not</returns>
        public delegate bool TryParse<T>(string source, out T result);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Add element upfront of sequence
        /// </summary>
        /// <param name="source"></param>
        /// <param name="head"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<T> AddHead<T>(this IEnumerable<T> source, T head)
        {
            yield return head;
            foreach(var item in source)
                yield return item;
        }

        /// <summary>
        ///     Clone object with <see cref="DataContract" /> attribute by using <see cref="DataContractSerializer" />
        /// </summary>
        /// <typeparam name="T">The type of object to apply serializer</typeparam>
        /// <param name="source">the original object. it must be not null for reference type objects</param>
        /// <param name="knownTypes">
        ///     An <see cref="IEnumerable{T}" /> of Type that contains the types that may be present in the
        ///     object graph.
        /// </param>
        /// <returns></returns>
        public static T Clone<T>(this T source, IEnumerable<Type> knownTypes = null)
        {
            Contract.Requires(!ReferenceEquals(source, null));
            Contract.Requires(typeof(T).IsDefined(typeof(DataContractAttribute), true));

            DataContractSerializer serializer;
            lock(_serializers)
            {
                var type = typeof(T);
                if(!_serializers.TryGetValue(type, out serializer))
                {
                    _serializers.Add(
                        type,
                        (serializer = knownTypes == null ? new DataContractSerializer(type) : new DataContractSerializer(type, knownTypes)));
                }
            }

            using(var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, source);
                ms.Position = 0;
                return (T)serializer.ReadObject(ms);
            }
        }

        /// <summary>
        ///     copy sequence items to the array
        /// </summary>
        /// <param name="source"></param>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <typeparam name="T"></typeparam>
        [DebuggerStepThrough]
        public static void CopyTo<T>(this IEnumerable<T> source, T[] array, int arrayIndex)
        {
            Contract.Requires(source != null);
            Contract.Requires(array != null);

            foreach(var item in source)
            {
                if(arrayIndex >= array.Length)
                    break;
                array[arrayIndex++] = item;
            }
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            Contract.Requires(source != null);
            Contract.Requires(keySelector != null);

            return Distinct(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Contract.Requires(source != null);
            Contract.Requires(keySelector != null);
            Contract.Requires(comparer != null);

            var set = new HashSet<TKey>(comparer);
            return source.Where(item => set.Add(keySelector(item)));
        }

        /// <summary>
        ///     compare two sequences and returns true if they contains equal objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool Equals<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return Equals(first, second, EqualityComparer<T>.Default);
        }

        /// <summary>
        ///     compare two sequences and returns true if they contains equal objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static bool Equals<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer)
        {
            return first.ZipAll(second, (a, b) => new { a, b }).All(
                z =>
                    {
                        if(ReferenceEquals(z.a, z.b))
                            return true;
                        if(ReferenceEquals(z.a, null) || ReferenceEquals(z.b, null))
                            return false;
                        return comparer.Equals(z.a, z.b);
                    });
        }

        /// <summary>
        ///     compare strings but if only spaces - ignore them
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreSpaces(this string a, string b)
        {
            if(ReferenceEquals(a, b) || String.IsNullOrWhiteSpace(a) == String.IsNullOrWhiteSpace(b))
                return true;
            return String.Equals(a, b);
        }

        public static bool EqualsNoOrder<T, TKey, TValue>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            Contract.Requires(keySelector != null);
            Contract.Requires(valueSelector != null);

            return EqualsNoOrder(first, second, keySelector, valueSelector, EqualityComparer<TValue>.Default);
        }

        public static bool EqualsNoOrder<T, TKey, TValue>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector,
            IEqualityComparer<TValue> comparer)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            Contract.Requires(keySelector != null);
            Contract.Requires(valueSelector != null);
            Contract.Requires(comparer != null);

            var d2 = second.ToDictionary(keySelector, valueSelector);
            foreach(var item in first)
            {
                var key = keySelector(item);
                TValue val;
                if(d2.TryGetValue(key, out val))
                {
                    if(!comparer.Equals(val, valueSelector(item)))
                        return false;
                }
                else
                    return false;
            }
            return d2.Count == 0;
        }

        public static bool EqualsNoOrder<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            Contract.Requires(comparer != null);

            var d2 = second.ToSet(comparer, false);

            foreach(var item in first)
            {
                if(d2.Contains(item))
                    d2.Remove(item);
                else
                    return false;
            }

            return d2.Count == 0;
        }

        /// <summary>
        ///     Performs the specified action on each element of the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">
        ///     The <see cref="Action{T}" /> delegate to perform on each element of the
        ///     <see cref="IEnumerable{T}" />
        /// </param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Contract.Requires(source != null);
            Contract.Requires(action != null);

            foreach(var item in source)
                action(item);
        }

        /// <summary>
        ///     return index of first item satisfied by predicate or -1 if no item found
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            Contract.Requires(source != null);
            Contract.Requires(predicate != null);

            var idx = 0;
            var en = source.GetEnumerator();
            while(en.MoveNext())
            {
                if(predicate(en.Current))
                    return idx;
                idx++;
            }
            return -1;
        }

        public static long ToInt64(this string source, long defaultValue)
        {
            Contract.Requires(source != null);

            return source.ToValue(Int64Parser, defaultValue);
        }

        public static long ToInt64(this string source)
        {
            Contract.Requires(source != null);

            return source.ToValue(Int64Parser);
        }

        /// <summary>
        ///     convert source sequence to <see cref="ISet{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="excludeDupplications"></param>
        /// <returns></returns>
        public static ISet<T> ToSet<T>(this IEnumerable<T> source, bool excludeDupplications = true)
        {
            Contract.Requires(source != null);

            return ToSet(source, EqualityComparer<T>.Default, excludeDupplications);
        }

        /// <summary>
        ///     convert source sequence to <see cref="ISet{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <param name="excludeDupplications"></param>
        /// <returns></returns>
        public static ISet<T> ToSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer, bool excludeDupplications)
        {
            Contract.Requires(source != null);
            Contract.Requires(comparer != null);

            var set = new HashSet<T>(comparer);

            if(excludeDupplications)
                source = source.Where(item => !set.Contains(item));

            foreach(var item in source)
                set.Add(item);

            return set;
        }

        /// <summary>
        ///     create string from provided sequence
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="delimiter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string ToString<T>(this IEnumerable<T> source, Func<T, string> selector, string delimiter)
        {
            Contract.Requires(source != null);
            Contract.Requires(selector != null);
            Contract.Requires(string.IsNullOrEmpty(delimiter));

            var sb = new StringBuilder();

            var en = source.GetEnumerator();

            if(en.MoveNext())
            {
                sb.Append(selector(en.Current));

                while(en.MoveNext())
                {
                    sb.Append(delimiter);
                    sb.Append(selector(en.Current));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     parser string to the specified type by provided parser
        /// </summary>
        /// <typeparam name="T">The type of object to parse to</typeparam>
        /// <param name="source">The source string. it must be not <b>NULL</b>.</param>
        /// <param name="parser">The parser to do the job</param>
        /// <returns>The result of parser job if succeeded or default value if it is not</returns>
        public static T ToValue<T>(this string source, TryParse<T> parser)
        {
            Contract.Requires(parser != null);

            T result;
            if(parser(source, out result))
                return result;
            throw new FormatException(String.Format("Error parsing value \"{0}\"", source ?? "NULL"));
        }

        /// <summary>
        ///     parser string to the specified type by provided parser
        /// </summary>
        /// <typeparam name="T">The type of object to parse to</typeparam>
        /// <param name="source">The source string. it must be not <b>NULL</b>.</param>
        /// <param name="parser">The parser to do the job</param>
        /// <param name="defaultValue">the value that will return if parser failed</param>
        /// <returns>The result of parser job if succeeded or default value if it is not</returns>
        public static T ToValue<T>(this string source, TryParse<T> parser, T defaultValue)
        {
            Contract.Requires(source != null);
            Contract.Requires(parser != null);

            T result;
            return parser(source, out result) ? result : defaultValue;
        }

        /// <summary>
        ///     create the transformation data provider, which provide the transformation from one provider's type to another
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceProvider"></param>
        /// <param name="transformationFunc"></param>
        /// <param name="providerFactory"></param>
        /// <returns></returns>
        public static IDataProvider<TResult> TransformDataProvider<TSource, TResult>(
            this IDataProvider<TSource> sourceProvider,
            Func<TSource, TResult> transformationFunc,
            Func<Func<TSource, TResult>, IDataProvider<TResult>> providerFactory = null)
        {
            Contract.Requires(sourceProvider != null);
            Contract.Requires(transformationFunc != null);

            var provider = providerFactory != null
                               ? providerFactory(transformationFunc)
                               : new LazyDataProvider<TResult>(() => transformationFunc(sourceProvider.Data));

            sourceProvider.Status.Notify += args =>
                {
                    if(args.Value == DataProviderStatus.NotReady)
                        provider.Reset();
                };

            return provider;
        }

        /// <summary>
        ///     Zip two sequences up to the end
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> ZipAll<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            var en1 = first.GetEnumerator();
            var en2 = second.GetEnumerator();

            while(en1.MoveNext())
            {
                if(en2.MoveNext())
                    yield return resultSelector(en1.Current, en2.Current);
                else
                {
                    do
                        yield return resultSelector(en1.Current, default(TSecond));
                    while(en1.MoveNext());
                }
            }

            while(en2.MoveNext())
                yield return resultSelector(default(TFirst), en2.Current);
        }

        #endregion
    }
}