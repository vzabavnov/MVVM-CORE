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
//  File Name: ExpressionExtensionsTests.cs.
//  Created: 2014/06/10/3:33 PM.
//  Modified: 2014/06/10/3:33 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.ComponentModel;
using System.Reflection;
using MVVM.Core.Tests.Annotations;
using Xunit;
using Zabavnov.MVVM;

#endregion

namespace MVVM.Core.Tests
{
    public class ExpressionExtensionsTests
    {
        public class TestClass: INotifyPropertyChanged
        {
            private string _property;

            public string Writable;

            public readonly string Readonly = "default";

            public string Property
            {
                get { return _property; }
                set
                {
                    if(value == _property)
                        return;
                    _property = value;
                    OnPropertyChanged("Property");
                }
            }

            public TestClass GetInnerClass()
            {
                return InnerClass;
            }

            public TestClass InnerClass { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if(handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Public Methods and Operators

        [Fact]
        public void GetPropertySetTest()
        {
            var tc = new TestClass { InnerClass = new TestClass { InnerClass = new TestClass() } };

            var setter = ExpressionExtensions.GetPropertySetter<TestClass, string>(t => t.Property);
            Assert.NotEqual("1", tc.Property);
            setter(tc, "1");
            Assert.Equal("1", tc.Property);

            setter = ExpressionExtensions.GetPropertySetter<TestClass, string>(t => t.InnerClass.Property);
            Assert.NotEqual("2", tc.InnerClass.Property);
            setter(tc, "2");
            Assert.Equal("2", tc.InnerClass.Property);

            setter = ExpressionExtensions.GetPropertySetter<TestClass, string>(t => t.InnerClass.GetInnerClass().Property);
            Assert.NotEqual("2", tc.InnerClass.GetInnerClass().Property);
            setter(tc, "2");
            Assert.Equal("2", tc.InnerClass.GetInnerClass().Property);

            tc = new TestClass { InnerClass = new TestClass { InnerClass = new TestClass() } };

            setter = ExpressionExtensions.GetPropertySetter<TestClass, string>(t => t.Writable);
            Assert.NotEqual("1", tc.Writable);
            setter(tc, "1");
            Assert.Equal("1", tc.Writable);

            setter = ExpressionExtensions.GetPropertySetter<TestClass, string>(t => t.InnerClass.Writable);
            Assert.NotEqual("2", tc.InnerClass.Writable);
            setter(tc, "2");
            Assert.Equal("2", tc.InnerClass.Writable);

            setter = ExpressionExtensions.GetPropertySetter<TestClass, string>(t => t.InnerClass.GetInnerClass().Writable);
            Assert.NotEqual("2", tc.InnerClass.GetInnerClass().Writable);
            setter(tc, "2");
            Assert.Equal("2", tc.InnerClass.GetInnerClass().Writable);
        }

        [Fact]
        public void GetPropertyContainerTest()
        {
            var tc = new TestClass { InnerClass = new TestClass { InnerClass = new TestClass()} };
            MemberInfo memberInfo;
            var getter = ExpressionExtensions.GetPropertyContainer<TestClass, string>(() => tc.Property, out memberInfo);
            Assert.True(ReferenceEquals(tc, getter()));
            Assert.Equal("Property", memberInfo.Name);

            getter = ExpressionExtensions.GetPropertyContainer<TestClass, object>(() => tc.Property, out memberInfo);
            Assert.True(ReferenceEquals(tc, getter()));
            Assert.Equal("Property", memberInfo.Name);

            ExpressionExtensions.GetPropertyContainer<TestClass, object>(() => tc.InnerClass.Property, out memberInfo);
            Assert.True(ReferenceEquals(tc, getter()));
            Assert.Equal("Property", memberInfo.Name);

            ExpressionExtensions.GetPropertyContainer<TestClass, object>(() => tc.InnerClass.GetInnerClass().Property, out memberInfo);
            Assert.True(ReferenceEquals(tc, getter()));
            Assert.Equal("Property", memberInfo.Name);
        }

        [Fact]
        public void AttachActionOnTest()
        {
            var tc = new TestClass { InnerClass = new TestClass { InnerClass = new TestClass()} };

            int ct = 0;

            Action action = () => ct++;

            action.AttachActionTo(() => tc.Property);
            tc.Property = "1";
            Assert.Equal(1, ct);

            action.DettachActionFrom(() => tc.Property);
            tc.Property = "-";
            Assert.Equal(1, ct);


            action.AttachActionTo(() => tc.InnerClass.Property);
            tc.InnerClass.Property = "2";
            Assert.Equal(2, ct);

            action.DettachActionFrom(() => tc.InnerClass.Property);
            tc.InnerClass.Property = "-";
            Assert.Equal(2, ct);

            action.AttachActionTo(() => tc.InnerClass.GetInnerClass().Property);
            tc.InnerClass.GetInnerClass().Property = "3";
            Assert.Equal(3, ct);

            action.DettachActionFrom(() => tc.InnerClass.GetInnerClass().Property);
            tc.InnerClass.GetInnerClass().Property = "-";
            Assert.Equal(3, ct);
        }

       
        #endregion
    }
}