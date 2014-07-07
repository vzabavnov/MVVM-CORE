using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Zabavnov.MVVM;

namespace MVVM.Core.Tests
{
    public class BinderTests
    {
        [Fact]
        public void TestOneTimeBinding()
        {
            var model = new ModelTestClass
                            {
                                Number = 1,
                                Text = "First"
                            };
            var control = new ControlTestClass
                              {
                                  Text = "Wrong",
                                  Number = -1
                              };

            var propInfo = typeof(ModelTestClass).GetProperty("Text");
            var textProp = new BindableProperty<ControlTestClass, string>(control, o => o.Text, (o, action) => o.TextChanged += (sender, args) => action());

            int controlCount = 0;
            textProp.PropertyChanged += (sender, args) => controlCount++;
            int modelCount = 0;
            model.PropertyChanged += (sender, args) => modelCount ++;

            Assert.Equal("Wrong", control.Text);
            Assert.Equal("First", model.Text);

            var binding = new BindingOneTime<ModelTestClass, ControlTestClass, string, string>(model, propInfo, o => o.Text, textProp, DataConverter<string>.EmptyConverter);
            
            TestOneTimeBindingInfo(binding, model, control, ref modelCount, ref controlCount);
        }

        private static void TestOneTimeBindingInfo(IBindingInfo<ModelTestClass, ControlTestClass, string, string> binding, ModelTestClass model, ControlTestClass control, ref int modelCount, ref int controlCount)
        {
            Assert.Equal(BindingMode.OneTime, binding.Direction);
            Assert.Equal("First", control.Text);
            Assert.Equal(1, controlCount);
            Assert.Equal(0, modelCount);

            model.Text = 2.ToText();
            Assert.Equal(1, modelCount);
            Assert.Equal(1, controlCount);
            Assert.Equal("First", control.Text);

            binding.NotifyModelPropertyChanged();
            Assert.Equal("Second", control.Text);
            Assert.Equal(2, controlCount);

            binding.Model = null;
            model.Text = 3.ToText();
            Assert.Equal(2, modelCount);
            binding.Model = model;
            Assert.Equal("Third", control.Text);
            Assert.Equal(3, controlCount);

            control.Text = "Forth";
            Assert.Equal(4, controlCount);
            Assert.Equal(2, modelCount);
        }

        [Fact]
        public void TestOneWayBinding()
        {
            var model = new ModelTestClass
            {
                Number = 1,
                Text = "First"
            };
            var control = new ControlTestClass
            {
                Text = "Wrong",
                Number = -1
            };

            var propInfo = typeof(ModelTestClass).GetProperty("Text");
            var textProp = new BindableProperty<ControlTestClass, string>(control, o => o.Text, (o, action) => o.TextChanged += (sender, args) => action());

            int controlCount = 0;
            textProp.PropertyChanged += (sender, args) => controlCount++;
            int modelCount = 0;
            model.PropertyChanged += (sender, args) => modelCount++;

            Assert.Equal("Wrong", control.Text);
            Assert.Equal("First", model.Text);

            var binding = new BindingOneWay<ModelTestClass, ControlTestClass, string, string>(
                model,
                propInfo,
                o => o.Text,
                textProp,
                DataConverter<string>.EmptyConverter);

            TestOneWayBindingInfo(binding, model, control, ref modelCount, ref controlCount);
        }

        private static void TestOneWayBindingInfo(IBindingInfo<ModelTestClass, ControlTestClass, string, string> binding, ModelTestClass model, ControlTestClass control, ref int modelCount, ref int controlCount)
        {
            Assert.Equal(BindingMode.OneWay, binding.Direction);
            Assert.Equal("First", control.Text);
            Assert.Equal(1, controlCount);
            Assert.Equal(0, modelCount);

            model.Text = 2.ToText();
            Assert.Equal(1, modelCount);
            Assert.Equal(2, controlCount);
            Assert.Equal("Second", control.Text);

            binding.Model = null;
            model.Text = 3.ToText();
            Assert.Equal(2, modelCount);
            binding.Model = model;
            Assert.Equal("Third", control.Text);
            Assert.Equal(3, controlCount);

            control.Text = "Forth";
            Assert.Equal(4, controlCount);
            Assert.Equal(2, modelCount);
        }

        [Fact]
        public void TestOneWayToSourceBinding()
        {
            var model = new ModelTestClass
            {
                Number = 1,
                Text = "Wrong"
            };
            var control = new ControlTestClass
            {
                Text = "First",
                Number = -1
            };

            var propInfo = typeof(ModelTestClass).GetProperty("Text");
            var textProp = new BindableProperty<ControlTestClass, string>(control, o => o.Text, (o, action) => o.TextChanged += (sender, args) => action());

            int[] controlCounts = { 0 };
            textProp.PropertyChanged += (sender, args) => controlCounts[0]++;
            int[] modelCounts = { 0 };
            model.PropertyChanged += (sender, args) => modelCounts[0]++;

            Assert.Equal("First", control.Text);
            Assert.Equal("Wrong", model.Text);

            var binding = new BindingOneWayToSource<ModelTestClass, ControlTestClass, string, string>(
               model,
               propInfo,
               (o, s) => o.Text = s,
               textProp,
               DataConverter<string>.EmptyConverter);
            TestOneWayToSourceBindingInfo(binding, model, control, modelCounts, controlCounts);
        }

        private static void TestOneWayToSourceBindingInfo(IBindingInfo<ModelTestClass, ControlTestClass, string, string> binding, ModelTestClass model, ControlTestClass control, int[] modelCounts, int[] controlCounts)
        {
            Assert.Equal(BindingMode.OneWayToSource, binding.Direction);
            Assert.Equal("First", model.Text);
            Assert.Equal(0, controlCounts[0]);
            Assert.Equal(1, modelCounts[0]);

            control.Text = 2.ToText();
            Assert.Equal(2, modelCounts[0]);
            Assert.Equal(1, controlCounts[0]);
            Assert.Equal("Second", model.Text);

            binding.Model = null;
            control.Text = 3.ToText();
            Assert.Equal(2, controlCounts[0]);

            binding.Model = model;
            Assert.Equal("Third", model.Text);
            Assert.Equal(3, modelCounts[0]);
            Assert.Equal(2, controlCounts[0]);

            model.Text = "Forth";
            Assert.Equal(4, modelCounts[0]);
            Assert.Equal(2, controlCounts[0]);
        }

        [Fact]
        public void TestTwoWayBinding()
        {
            var model = new ModelTestClass
            {
                Number = 1,
                Text = "First"
            };
            var control = new ControlTestClass
            {
                Text = "Wrong",
                Number = -1
            };

            var propInfo = typeof(ModelTestClass).GetProperty("Text");
            var textProp = new BindableProperty<ControlTestClass, string>(control, o => o.Text, (o, action) => o.TextChanged += (sender, args) => action());

            int controlCount = 0;
            textProp.PropertyChanged += (sender, args) => controlCount++;
            int modelCount = 0;
            model.PropertyChanged += (sender, args) => modelCount++;

            Assert.Equal("Wrong", control.Text);
            Assert.Equal("First", model.Text);

            var binding = new BindingTwoWay<ModelTestClass, ControlTestClass, string, string>(
                model,
                propInfo,
                o => o.Text,
                (o, s) => o.Text = s,
                textProp,
                DataConverter<string>.EmptyConverter);

            TestTwoWayBindingInfo(binding, model, control, ref modelCount, ref controlCount);
        }

        private static void TestTwoWayBindingInfo(IBindingInfo<ModelTestClass, ControlTestClass, string, string> binding, ModelTestClass model, ControlTestClass control, ref int modelCount, ref int controlCount)
        {
            Assert.Equal(BindingMode.TwoWay, binding.Direction);
            Assert.Equal("First", control.Text);
            Assert.Equal(1, controlCount);
            Assert.Equal(0, modelCount);

            model.Text = 2.ToText();
            Assert.Equal(1, modelCount);
            Assert.Equal(2, controlCount);
            Assert.Equal("Second", control.Text);

            binding.Model = null;
            model.Text = 3.ToText();
            Assert.Equal(2, modelCount);

            binding.Model = model;
            Assert.Equal("Third", control.Text);
            Assert.Equal(3, controlCount);

            control.Text = "Forth";
            Assert.Equal(4, controlCount);
            Assert.Equal(3, modelCount);
            Assert.Equal("Forth", model.Text);
        }

        [Fact]
        public void TestBinder()
        {
            var model = new ModelTestClass
            {
                Number = 1,
                Text = "First"
            };
            var control = new ControlTestClass
            {
                Text = "Wrong",
                Number = -1
            };

            var textProp = new BindableProperty<ControlTestClass, string>(control, o => o.Text, (o, action) => o.TextChanged += (sender, args) => action());

            int[] controlCounts = { 0 };
            textProp.PropertyChanged += (sender, args) => controlCounts[0]++;
            int[] modelCounts = { 0 };
            model.PropertyChanged += (sender, args) => modelCounts[0]++;

            var binder = new Binder<ModelTestClass, ControlTestClass, string, string>(m => m.Text, () => DataConverter<string>.EmptyConverter);

            var bInfo = binder.Bind(model, textProp, BindingMode.OneTime);
            TestOneTimeBindingInfo(bInfo, model, control, ref modelCounts[0], ref controlCounts[0]);
            bInfo.Unbind();

            model.Text = "First";
            modelCounts[0] = 0;
            control.Text = "Wrong";
            controlCounts[0] = 0;
            textProp = new BindableProperty<ControlTestClass, string>(control, o => o.Text, (o, action) => o.TextChanged += (sender, args) => action());
            bInfo = binder.Bind(model, textProp, BindingMode.OneWay);
            TestOneWayBindingInfo(bInfo, model, control, ref modelCounts[0], ref controlCounts[0]);
            bInfo.Unbind();

            model.Text = "Wrong";
            control.Text = "First";
            modelCounts[0] = 0;
            controlCounts[0] = 0;
            textProp = new BindableProperty<ControlTestClass, string>(control, o => o.Text, (o, action) => o.TextChanged += (sender, args) => action());
            bInfo = binder.Bind(model, textProp, BindingMode.OneWayToSource);
            TestOneWayToSourceBindingInfo(bInfo, model, control, modelCounts, controlCounts);
            bInfo.Unbind();

            model.Text = "First";
            modelCounts[0] = 0;
            control.Text = "Wrong";
            controlCounts[0] = 0;
            textProp = new BindableProperty<ControlTestClass, string>(control, o => o.Text, (o, action) => o.TextChanged += (sender, args) => action());
            bInfo = binder.Bind(model, textProp, BindingMode.TwoWay);
            TestTwoWayBindingInfo(bInfo, model, control, ref modelCounts[0], ref controlCounts[0]);
            bInfo.Unbind();
        }
    }
}
