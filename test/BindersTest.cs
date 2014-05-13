namespace WinFormsMVVM.Tests
{
    using System;
    using System.ComponentModel;

    using Xunit;

    using Zabavnov.WFMVVM;

    /// <summary>
    /// Summary description for BindersTest
    /// </summary>
    public class BindersTest
    {
        public class EventClass
        {
            private string _value;
            private readonly int _readonly;

            public string Value
            {
                get { return _value; }
                set
                {
                    if(_value != value)
                    {
                        _value = value;
                        OnChanges();
                    }
                }
            }

            public event EventHandler ValueChanged;

            public virtual void OnChanges()
            {
                var handler = ValueChanged;
                if(handler != null)
                    handler(this, EventArgs.Empty);
            }

            public int Readonly
            {
                get { return _readonly; }
            }

            public void SetValue(int v)
            {
                OnChanges();
            }

            public EventClass()
            {
                 _readonly = 38;
            }
        }

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
        public void TestCreateBinder()
        {
            var binder = new PropertyBinder<EventClass, string>(z => z.Value, (cls, action) => cls.ValueChanged += (sender, args) => action());
            Assert.NotNull(binder);
            Assert.NotNull(binder.Getter);
            Assert.True(binder.CanRead);
            Assert.NotNull(binder.Setter);
            Assert.True(binder.CanWrite);
            Assert.Equal("Value", binder.PropertyName);

            var rdBinder = new PropertyBinder<EventClass, int>(z => z.Readonly);
            Assert.NotNull(rdBinder);
            Assert.NotNull(rdBinder.Getter);
            Assert.True(rdBinder.CanRead);
            Assert.Null(rdBinder.Setter);
            Assert.False(rdBinder.CanWrite);
            Assert.Equal("Readonly", rdBinder.PropertyName);
        }

        [Fact]
        public void TestCreateBindableProperty()
        {
            var binder = new PropertyBinder<EventClass, string>(z => z.Value, (cls, action) => cls.ValueChanged += (sender, args) => action());
            var obj = new EventClass {Value = "0"};

            var vProp = binder.BindTo(obj);
            Assert.Equal("Value", vProp.PropertyName);
            Assert.True(binder.CanRead);
            Assert.True(binder.CanWrite);

            Assert.Equal("0", vProp.Value);
            vProp.Value = "1";
            Assert.Equal("1", vProp.Value);

            string name = null;
            var counter = new int[1];
            vProp.PropertyChanged += (sender, args) =>
            {
                name = args.PropertyName;
                counter[0]++;
            };

            Assert.Equal(0, counter[0]);
            Assert.Null(name);

            vProp.Value = "2";
            Assert.Equal("Value", name);
            Assert.Equal(1, counter[0]);


            name = null;
            vProp.Value = "3";
            Assert.Equal("Value", name);
            Assert.Equal(2, counter[0]);
            Assert.Equal("3", vProp.Value);

            name = null;
            vProp.Value = "3";
            Assert.Null(name);
            Assert.Equal(2, counter[0]);
            Assert.Equal("3", vProp.Value);

            IPropertyBinder<EventClass, int> b = new PropertyBinder<EventClass, string, int>(z => z.Value, (cls, action) => cls.ValueChanged += (sender, args) => action(),
                new DataConverter<string, int>(int.Parse, n => n.ToString()));


            var nProp = b.BindTo(obj);
            name = null;
            counter[0] = 0;

            Assert.Equal(3, nProp.Value);

            nProp.Value = 4;
            Assert.Equal("Value", name);
            Assert.Equal(1, counter[0]);
            Assert.Equal(4, nProp.Value);

        }

        [Fact]
        public void TestDataRange()
        {
            int isValidCounter = 0;
            int startCounter = 0;
            int endCounter = 0;
            int valueCounter = 0;
            var dr = new DateTimeWithRange();
            dr.AttachActionOn(z=>z.IsValid, b => isValidCounter++);
            dr.AttachActionOn(z => z.Start, b => startCounter++);
            dr.AttachActionOn(z => z.End, b => endCounter++);
            dr.AttachActionOn(z => z.Value, b => valueCounter++);

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
    }
}
