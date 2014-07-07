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
//  File Name: PropertyBinderTests.cs.
//  Created: 2014/06/06/5:09 PM.
//  Modified: 2014/06/06/5:12 PM.
//  ****************************************************************************

#endregion

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
//  File Name: PropertyBinderTests.cs.
//  Created: 2014/06/06/5:09 PM.
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
    public class PropertyBinderTests
    {
        #region Public Methods and Operators

        [Fact]
        public void TestCreateBinder()
        {
            var binder = new PropertyBinder<ControlTestClass, string>(
                z => z.Text,
                (cls, action) => cls.TextChanged += (sender, args) => action());
            Assert.NotNull(binder);
            Assert.NotNull(binder.Getter);
            Assert.True(binder.CanRead);
            Assert.NotNull(binder.Setter);
            Assert.True(binder.CanWrite);
            Assert.Equal("Text", binder.PropertyName);

            var rdBinder = new PropertyBinder<ControlTestClass, int>(z => z.ReadOnly);
            Assert.NotNull(rdBinder);
            Assert.NotNull(rdBinder.Getter);
            Assert.True(rdBinder.CanRead);
            Assert.Null(rdBinder.Setter);
            Assert.False(rdBinder.CanWrite);
            Assert.Equal("ReadOnly", rdBinder.PropertyName);

        }

        /// <summary>
        /// </summary>
        [Fact]
        public void TestBindTo()
        {
            IPropertyBinder<ControlTestClass, string> binder = new PropertyBinder<ControlTestClass, string>(
                    z => z.Text,
                    (cls, action) => cls.TextChanged += (sender, args) => action());

            var instance = new ControlTestClass { Text = "First" };

            TestBindableProperty(instance, binder.BindTo(instance));

            binder = new PropertyBinder<ControlTestClass, int, string>(
                    z => z.Number,
                    (cls, action) => cls.NumberChanged += (sender, args) => action(),
                    new DataConverter<int, string>(Helper.ToText, Helper.ToInt32));

            instance = new ControlTestClass { Number = 1 };

            var property = binder.BindTo(instance);

            Assert.Equal("First", property.Value);
            Assert.Equal(instance.Number.ToText(), property.Value);

            instance.Number = 2;
            Assert.Equal("Second", property.Value);

            property.Value = "Third";
            Assert.Equal("Third", instance.Number.ToText());

            int numberChangedCount = 0;
            int propertyChangedCount = 0;
            instance.NumberChanged += (sender, args) => numberChangedCount++;
            property.PropertyChanged += (sender, args) =>
            {
                Assert.Equal("Number", args.PropertyName);
                propertyChangedCount++;
            };

            instance.Number = 4;
            Assert.Equal(1, numberChangedCount);
            Assert.Equal(1, propertyChangedCount);

            property.Value = "Fifth";
            Assert.Equal(2, numberChangedCount);
            Assert.Equal(2, propertyChangedCount);
        }

        #endregion

        internal static void TestBindableProperty(ControlTestClass instance, IBindableProperty<ControlTestClass, string> property)
        {
            Assert.Equal("First", property.Value);
            Assert.Equal(instance.Text, property.Value);

            instance.Text = "Second";
            Assert.Equal("Second", property.Value);

            property.Value = "Third";
            Assert.Equal( "Third", instance.Text);

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
        }
    }
}