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
//  File Name: BindersTests.cs.
//  Created: 2014/06/04/5:07 PM.
//  Modified: 2014/06/06/5:11 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.ComponentModel;
using Xunit;
using Zabavnov.MVVM;

#endregion

namespace MVVM.Core.Tests
{
    /// <summary>
    ///     Summary description for BindersTest
    /// </summary>
    public class SpecialTests
    {
        #region Public Methods and Operators

        [Fact]
        [Description("Test binding of boundaries of two Dates")]
        public void TestBoundaryBinder()
        {
            var lower = new DateTimeWithRange();
            var upper = new DateTimeWithRange();

            var binder = new BoundaryBinder(lower, upper);
            binder.Bind();

            Assert.Null(lower.End);
            Assert.Null(upper.Start);

            lower.Value = DateTime.Today.AddDays(-2);
            Assert.NotNull(upper.Start);
            Assert.Equal(DateTime.Today.AddDays(-2), upper.Start.Value);

            lower.Value = DateTime.Today.AddDays(-3);
            Assert.NotNull(upper.Start);
            Assert.Equal(DateTime.Today.AddDays(-3), upper.Start.Value);

            lower.Value = null;
            Assert.Null(upper.Start);

            upper.Value = DateTime.Today.AddDays(2);
            Assert.NotNull(lower.End);
            Assert.Equal(DateTime.Today.AddDays(2), lower.End.Value);

            upper.Value = DateTime.Today.AddDays(3);
            Assert.NotNull(lower.End);
            Assert.Equal(DateTime.Today.AddDays(3), lower.End.Value);

            upper.Value = null;
            Assert.Null(lower.Start);
        }


        [Fact]
        public void TestDataRange()
        {
            int isValidCounter = 0;
            int startCounter = 0;
            int endCounter = 0;
            int valueCounter = 0;
            var dr = new DateTimeWithRange();
            ExpressionExtensions.AttachActionTo(() => isValidCounter++, () => dr.IsValid);
            ExpressionExtensions.AttachActionTo(() => startCounter++, () => dr.Start);
            ExpressionExtensions.AttachActionTo(() => endCounter++, () => dr.End);
            ExpressionExtensions.AttachActionTo(() => valueCounter++, () => dr.Value);

            Assert.Equal(DateTime.MinValue, dr);
            Assert.True(dr.IsValid);
            Assert.Equal(0, valueCounter);

            dr.Value = DateTime.Today;
            Assert.True(dr.IsValid);
            Assert.Equal(0, isValidCounter);
            Assert.Equal(1, valueCounter);

            dr.Start = DateTime.Today + TimeSpan.FromDays(1);
            Assert.False(dr.IsValid);
            Assert.Equal(1, valueCounter);
            Assert.Equal(1, isValidCounter);
            Assert.Equal(1, startCounter);

            dr.Value = DateTime.Today + TimeSpan.FromDays(3);
            Assert.True(dr.IsValid);
            Assert.Equal(2, valueCounter);
            Assert.Equal(2, isValidCounter);

            dr.End = DateTime.Today + TimeSpan.FromDays(2);
            Assert.False(dr.IsValid);
            Assert.Equal(1, endCounter);
            Assert.Equal(3, isValidCounter);

            dr.Value = DateTime.Today + TimeSpan.FromDays(2);
            Assert.True(dr.IsValid);
            Assert.Equal(4, isValidCounter);
            Assert.Equal(3, valueCounter);
        }

        #endregion
    }
}