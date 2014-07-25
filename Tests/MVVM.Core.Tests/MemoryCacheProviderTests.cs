using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using Xunit;
using Xunit.Sdk;
using Zabavnov.MVVM;

namespace MVVM.Core.Tests
{
    public class MemoryCacheProviderTests
    {
        [Fact]
        public void AbsoluteExpitationTest()
        {
            Func<DateTime> dataProvider = () => DateTime.Now;

            IDataProvider<DateTime> provider = new MemoryCacheProvider<DateTime>("DateTime", dataProvider, TimeSpan.FromSeconds(10), ObjectCache.NoSlidingExpiration);

            var now = DateTime.Now;
            var dt = provider.Data;

            while(true)
            {
                Thread.Sleep(100);
                var d = provider.Data;

                if(d != dt)
                {
                    Assert.True(DateTime.Now - now >= TimeSpan.FromSeconds(10));
                    break;
                }
                Assert.True(DateTime.Now - now < TimeSpan.FromSeconds(10));
            }
        }

        [Fact]
        public void SlidingExpirationTest()
        {
            Func<DateTime> dataProvider = () => DateTime.Now;

            IDataProvider<DateTime> provider = new MemoryCacheProvider<DateTime>("DateTime", dataProvider, ObjectCache.NoSlidingExpiration, TimeSpan.FromSeconds(2));

            bool statusChanged = false;
            provider.Status.Notify += args =>
                {
                    Trace.Write(string.Format("Status changed from {0} to {1}", args.OldValue, args.Value));
                    statusChanged = true;
                };
            
            var dt = provider.Data;
            statusChanged = false;

            for(int i = 0; i < 20; i++)
            {
                Thread.Sleep(100);
                var d = provider.Data;
                Assert.Equal(DataProviderStatus.Ready, provider.Status.Value);
                Assert.Equal(dt, d);
                
            }

            Assert.False(statusChanged);

            Thread.Sleep(MemoryCache.Default.PollingInterval);
            
            Assert.True(statusChanged);

            Assert.Equal(DataProviderStatus.NotReady, provider.Status.Value);
            Assert.NotEqual(dt, provider.Data);
        }

        [Fact]
        public void ResetTest()
        {
            Func<DateTime> dataProvider = () => DateTime.Now;

            IDataProvider<DateTime> provider = new MemoryCacheProvider<DateTime>("DateTime", dataProvider, ObjectCache.NoSlidingExpiration, TimeSpan.FromSeconds(20));
            
            var dt = provider.Data;

            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(500);
                var d = provider.Data;
                Assert.Equal(dt, d);
            }
            Assert.Equal(DataProviderStatus.Ready, provider.Status.Value);
            provider.Reset();
            Assert.Equal(DataProviderStatus.NotReady, provider.Status.Value);

            Assert.NotEqual(dt, provider.Data);
        }
    }
}
