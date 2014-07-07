#region Proprietary  Notice

//  ****************************************************************************
//    Copyright 2014 Vadim Zabavnov
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  ****************************************************************************
//  File Name: BindablePropertyTests.cs.
//  Created: 2014/06/06/10:30 AM.
//  Modified: 2014/06/06/5:10 PM.
//  ****************************************************************************
#endregion

#region Usings

using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;
using Zabavnov.MVVM;

#endregion

namespace MVVM.Core.Tests
{
    /// <summary>
    /// </summary>
    public class BindablePropertyTests
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public static IEnumerable<object[]> CreateDataSource
        {
            get
            {
                var instance = new ControlTestClass { Text = "First" };
                var property = new BindableProperty<ControlTestClass, string>(
                    instance, 
                    o => o.Text, 
                    (o, action) => o.TextChanged += (sender, args) => action());

                yield return new object[] { instance, property };

                instance = new ControlTestClass { Number = 1 };
                property = new BindableProperty<ControlTestClass, string>(
                    instance, 
                    "Text", 
                    o => o.Number.ToText(), 
                    (o, s) => o.Number = s.ToInt32(), 
                    (o, action) => o.NumberChanged += (sender, args) => action());

                yield return new object[] { instance, property };
            }
        }

        #endregion

        #region Public Methods and Operators

        [Fact]
        public void TestCreate()
        {
            var instance = new ControlTestClass { Text = "First" };
            var property = new BindableProperty<ControlTestClass, string>(
                instance,
                o => o.Text,
                (o, action) => o.TextChanged += (sender, args) => action());


            Assert.Equal("First", property.Value);
            Assert.Equal(instance.Text, property.Value);

            instance.Text = "Second";
            Assert.Equal("Second", property.Value);

            property.Value = "Third";
            Assert.Equal("Third", instance.Text);

            int textChangedCount = 0;
            int propertyChangedCount = 0;
            instance.TextChanged += (sender, args) => textChangedCount++;
            property.PropertyChanged += (sender, args) =>
            {
                Assert.Equal("Text", args.PropertyName);
                propertyChangedCount++;
            };

            instance.Text = "Forth";
            Assert.Equal(1, textChangedCount);
            Assert.Equal(1, propertyChangedCount);

            property.Value = "Fifth";
            Assert.Equal(2, textChangedCount);
            Assert.Equal(2, propertyChangedCount);

            instance = new ControlTestClass { Number = 1 };
            property = new BindableProperty<ControlTestClass, string>(
                instance,
                "Text",
                o => o.Number.ToText(),
                (o, s) => o.Number = s.ToInt32(),
                (o, action) => o.NumberChanged += (sender, args) => action());

            Assert.Equal("First", property.Value);
            Assert.Equal(instance.Number, property.Value.ToInt32());

            instance.Number = 2;
            Assert.Equal("Second", property.Value);

            property.Value = "Third";
            Assert.Equal("Third", instance.Number.ToText());

            textChangedCount = 0;
            propertyChangedCount = 0;

            instance.NumberChanged += (sender, args) => textChangedCount++;
            property.PropertyChanged += (sender, args) =>
            {
                Assert.Equal("Text", args.PropertyName);
                propertyChangedCount++;
            };

            instance.Number = 4;
            Assert.Equal(1, textChangedCount);
            Assert.Equal(1, propertyChangedCount);

            property.Value = "Fifth";
            Assert.Equal(2, textChangedCount);
            Assert.Equal(2, propertyChangedCount);
        }

        #endregion

    }
}