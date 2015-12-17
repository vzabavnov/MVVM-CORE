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
//  File Name: MemoryCacheProvider.cs.
//  Created: 2014/07/22/4:17 PM.
//  Modified: 2014/07/23/10:29 AM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Diagnostics.Contracts;
using System.Runtime.Caching;

#endregion

namespace Zabavnov.MVVM
{
    public class MemoryCacheProvider<T> : IDataProvider<T>
    {
        #region Fields

        private readonly TimeSpan _absoluteExpiration;
        private readonly string _cacheKey;
        private readonly string _cacheRegionKey;
        private readonly Func<T> _dataProvider;
        private readonly string _monitorUniqueID;
        private readonly TimeSpan _slidingExpiration;
        private readonly SimpleNotifiable<DataProviderStatus> _status = new SimpleNotifiable<DataProviderStatus>(DataProviderStatus.NotReady);
        private readonly object _syncObj = new object();
        private Action _resetAction;
        private ObjectCache _cache;
        private Func<ObjectCache> _cacheProvider;

        #endregion

        public Func<ObjectCache> CacheProvider
        {
            get { return _cacheProvider ?? (_cacheProvider = () => MemoryCache.Default); }
            set { _cacheProvider = value; }
        }

        public ObjectCache Cache => _cache ?? (_cache = CacheProvider());

        #region Constructors and Destructors

        public MemoryCacheProvider(string cacheKey, Func<T> dataProvider, TimeSpan absoluteExpiration, TimeSpan slidingExpiration)
            : this(cacheKey, null, dataProvider, absoluteExpiration, slidingExpiration)
        {
            Contract.Requires(!String.IsNullOrEmpty(cacheKey));
            Contract.Requires(dataProvider != null);
        }

        public MemoryCacheProvider(
            string cacheKey,
            string cacheRegionKey,
            Func<T> dataProvider,
            TimeSpan absoluteExpiration,
            TimeSpan slidingExpiration)
        {
            Contract.Requires(!String.IsNullOrEmpty(cacheKey));
            Contract.Requires(dataProvider != null);

            _cacheKey = cacheKey;
            _cacheRegionKey = cacheRegionKey;
            _dataProvider = dataProvider;
            _absoluteExpiration = absoluteExpiration;
            _slidingExpiration = slidingExpiration;
            _monitorUniqueID = typeof(MemoryCacheProvider<>).Name + (_cacheRegionKey == null ? _cacheKey : _cacheRegionKey + ":" + _cacheKey);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The value that <see cref="IDataProvider{T}" /> provides. The data should be always ready
        /// </summary>
        /// <remarks>
        ///     If data is not ready than provider must retrieve data and set status as <see cref="DataProviderStatus.Ready" />
        /// </remarks>
        public T Data
        {
            get
            {
                lock(_syncObj)
                {
                    var cacheItem = Cache.GetCacheItem(_cacheKey, _cacheRegionKey);
                    if(cacheItem == null)
                    {
                        Contract.Assert(Status.Value != DataProviderStatus.Ready);

                        _status.Value = DataProviderStatus.Updating;

                        cacheItem = new CacheItem(_cacheKey, _dataProvider(), _cacheRegionKey);

                        var cachePolicy = new CacheItemPolicy
                                              {
                                                  SlidingExpiration = _slidingExpiration,
                                                  RemovedCallback = OnItemRemoved,
                                                  AbsoluteExpiration =
                                                      _absoluteExpiration != ObjectCache.NoSlidingExpiration
                                                          ? DateTimeOffset.Now + _absoluteExpiration
                                                          : ObjectCache.InfiniteAbsoluteExpiration,
                                                  ChangeMonitors = { new ResetMonitor(this) }
                                              };

                        Cache.Set(cacheItem, cachePolicy);
                        
                        _status.Value = DataProviderStatus.Ready;
                    }

                    return (T)cacheItem.Value;
                }
            }
        }

        /// <summary>
        ///     The status of provider
        /// </summary>
        public INotifiable<DataProviderStatus> Status => _status;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Notify the provider that data must be refreshed
        /// </summary>
        public void Reset()
        {
            lock(_syncObj)
            {
                _resetAction?.Invoke();
            }
        }

        #endregion

        private void OnItemRemoved(CacheEntryRemovedArguments args)
        {
            lock(_syncObj)
            {
                _status.Value = DataProviderStatus.NotReady;
            }
        }

        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(this._status != null);
        }

        class ResetMonitor : ChangeMonitor
        {
            private readonly MemoryCacheProvider<T> _memoryCacheProvider;

            public ResetMonitor(MemoryCacheProvider<T> memoryCacheProvider)
            {
                Contract.Requires(memoryCacheProvider != null);

                _memoryCacheProvider = memoryCacheProvider;

                lock(_memoryCacheProvider._syncObj)
                {
                    _memoryCacheProvider._resetAction = () =>
                        {
                            OnChanged(null);
                            _memoryCacheProvider._status.Value = DataProviderStatus.NotReady;
                        };
                }

                InitializationComplete();
            }

            /// <summary>
            ///     Gets a value that represents the <see cref="T:System.Runtime.Caching.ChangeMonitor" /> class instance.
            /// </summary>
            /// <returns>
            ///     The identifier for a change-monitor instance.
            /// </returns>
            public override string UniqueId => _memoryCacheProvider._monitorUniqueID;

            /// <summary>
            ///     Releases all managed and unmanaged resources and any references to the
            ///     <see cref="T:System.Runtime.Caching.ChangeMonitor" /> instance. This overload must be implemented by derived
            ///     change-monitor classes.
            /// </summary>
            /// <param name="disposing">
            ///     true to release managed and unmanaged resources and any references to a
            ///     <see cref="T:System.Runtime.Caching.ChangeMonitor" /> instance; false to release only unmanaged resources. When
            ///     false is passed, the <see cref="M:System.Runtime.Caching.ChangeMonitor.Dispose(System.Boolean)" /> method is called
            ///     by a finalizer thread and any external managed references are likely no longer valid because they have already been
            ///     garbage collected.
            /// </param>
            protected override void Dispose(bool disposing)
            {
                if(disposing)
                {
                    lock(_memoryCacheProvider._syncObj)
                    {
                        _memoryCacheProvider._resetAction = null;
                    }
                }
            }

            [ContractInvariantMethod]
            void ObjectInvariant()
            {
                Contract.Invariant(this._memoryCacheProvider != null);
            }
        }
    }
}