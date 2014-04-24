namespace Zabavnov.WFMVVM
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    ///     Binders for <see cref="DateTimePicker" /> control
    /// </summary>
    public static class DateTimePickerBinders
    {
        #region binders

        /// <summary>
        ///     binder for <see cref="DateTimePicker.MinDate" />
        /// </summary>
        /// <remarks>
        ///     because property control doesn't have null value, the setter set it to
        ///     <see cref="DateTimePicker.MinDateTime" /> when value is null
        /// </remarks>
        public static readonly IPropertyBinder<DateTimePicker, DateTime?> MinDateBinder =
            new PropertyBinder<DateTimePicker, DateTime, DateTime?>(z => z.MinDate, new NullableConverter<DateTime>(DateTimePicker.MinDateTime));

        /// <summary>
        ///     binder for <see cref="DateTimePicker.MaxDate" />
        /// </summary>
        /// <remarks>
        ///     because property control doesn't have null value, the setter set it to
        ///     <see cref="DateTimePicker.MaxDateTime" /> when value is null
        /// </remarks>
        public static readonly IPropertyBinder<DateTimePicker, DateTime?> MaxDateBinder =
            new PropertyBinder<DateTimePicker, DateTime, DateTime?>(z => z.MaxDate, new NullableConverter<DateTime>(DateTimePicker.MaxDateTime));

        /// <summary>
        ///     binder for <see cref="DateTimePicker.Value" />
        /// </summary>
        public static readonly IPropertyBinder<DateTimePicker, DateTime?> DateBinder =
            new PropertyBinder<DateTimePicker, DateTime, DateTime?>(z => z.Value, (ctrl, action) => ctrl.ValueChanged += (sender, args) => action(),
                new NullableConverter<DateTime>(DateTimePicker.MinDateTime));

        /// <summary>
        ///     binder for <see cref="DateTimePicker.CustomFormat" />
        /// </summary>
        public static readonly PropertyBinder<DateTimePicker, string> FormatBinder = new PropertyBinder<DateTimePicker, string>(z => z.CustomFormat,
            (ctrl, action) => ctrl.FormatChanged += (sender, args) => action())
                                                                                         {
                                                                                             Setter = (ctrl, val) =>
                                                                                                 {
                                                                                                     ctrl.Format = DateTimePickerFormat.Custom;
                                                                                                     ctrl.CustomFormat = val;
                                                                                                 }
                                                                                         };

        #endregion

        #region extensions

        /// <summary>
        ///     returns bindable property for MinDate value
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static IBindableProperty<DateTimePicker, DateTime?> MinDateProperty(this DateTimePicker ctrl)
        {
            return MinDateBinder.BindTo(ctrl);
        }

        /// <summary>
        ///     returns bindable property for MaxDate
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static IBindableProperty<DateTimePicker, DateTime?> MaxDateProperty(this DateTimePicker ctrl)
        {
            return MaxDateBinder.BindTo(ctrl);
        }

        #endregion
    }
}