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
//  File Name: ProviderTests.cs.
//  Created: 2014/06/27/10:13 AM.
//  Modified: 2014/06/27/10:14 AM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Linq;
using Xunit;

#endregion

namespace Zabavnov.MVVM.Tests
{
    internal class ProviderTests
    {
        [Fact]
        public void TestTransformationProvider()
        {
            var source1 = new[] { 1, 2, 3, 4, 5 };

            var source2 = new[] { 6, 7, 8, 9, 10 };

            Func<int[], int[]> transFunc = ints => ints.Where(z => z % 2 == 0).ToArray();

            var currentSource = source1;
            DataProviderStatus? providerStatus = null;
            DataProviderStatus? trProviderStatus = null;
            var provider = new LazyDataProvider<int[]>(() => currentSource);
            provider.Status.Notify += args => providerStatus = args.Value;

            var trProvider = provider.TransformDataProvider(transFunc);
            trProvider.Status.Notify += args => trProviderStatus = args.Value;

            Assert.Equal(DataProviderStatus.NotReady, provider.Status.Value);
            Assert.Equal(DataProviderStatus.NotReady, trProvider.Status.Value);
            Assert.Null(providerStatus);
            Assert.Null(trProviderStatus);

            var result = trProvider.Data;
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(DataProviderStatus.Ready, provider.Status.Value);
            Assert.Equal(DataProviderStatus.Ready, trProvider.Status.Value);
            Assert.Equal(DataProviderStatus.Ready, providerStatus);
            Assert.Equal(DataProviderStatus.Ready, trProviderStatus);

            currentSource = source2;
            result = trProvider.Data;
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(DataProviderStatus.Ready, providerStatus);
            Assert.Equal(DataProviderStatus.Ready, trProviderStatus);

            trProviderStatus = null;
            providerStatus = null;
            trProvider.Reset();

            result = trProvider.Data;
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(DataProviderStatus.Ready, trProviderStatus);
            Assert.Equal(null, providerStatus);

            trProviderStatus = null;
            providerStatus = null;

            provider.Reset();
            result = trProvider.Data;
            Assert.Equal(3, result.Length);
            Assert.Equal(DataProviderStatus.Ready, trProviderStatus);
            Assert.Equal(DataProviderStatus.Ready, providerStatus);
        }
    }
}