using System;
using System.Runtime.Caching;
using System.Threading;
using Xunit;

using Zabavnov.MVVM;

namespace MVVM.Core.Tests
{
    public class MemoryCacheProviderTests
    {
        public class Test1
        {
            [Fact]
            public void AbsoluteExpitationTest()
            {
                int count = 0;
                Func<DateTime> dataProvider = () =>
                    {
                        count++;
                        return DateTime.Now;
                    };

                IDataProvider<DateTime> provider = new MemoryCacheProvider<DateTime>("DateTime1", dataProvider, TimeSpan.FromSeconds(5), ObjectCache.NoSlidingExpiration);

                var now = DateTime.Now;
                var dt = provider.Data;

                while(true)
                {
                    Thread.Sleep(100);
                    var d = provider.Data;

                    if(d != dt)
                    {
                        Assert.True(DateTime.Now - now <= TimeSpan.FromSeconds(10));
                        break;
                    }

                    Assert.Equal(1, count);
                }
            }
            
        }

        public class Test2
        {
            [Fact]
            public void SlidingExpirationTest()
            {
                int count = 0;
                Func<DateTime> dataProvider = () =>
                    {
                        count++;
                        return DateTime.Now;
                    };

                IDataProvider<DateTime> provider = new MemoryCacheProvider<DateTime>("DateTime2", dataProvider, ObjectCache.NoSlidingExpiration, TimeSpan.FromSeconds(2));

                var dt = provider.Data;

                DateTime d = provider.Data;
                for(int i = 0; i < 10; i++)
                {
                    Thread.Sleep(300);
                    Assert.Equal(dt, d);
                    Assert.Equal(1, count);
                }

                Thread.Sleep(TimeSpan.FromSeconds(3));

                d = provider.Data;
                Assert.Equal(2, count);
                Assert.NotEqual(dt, d);
            }
        }

        public class Test3
        {
            [Fact]
            public void ResetTest()
            {
                Func<DateTime> dataProvider = () => DateTime.Now;

                IDataProvider<DateTime> provider = new MemoryCacheProvider<DateTime>("DateTime3", dataProvider, ObjectCache.NoSlidingExpiration, TimeSpan.FromSeconds(20));
            
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
}
