using System.Windows.Forms;
using Zabavnov.MVVM;

namespace Zabavnov.Windows.Forms.MVVM
{
    /// <summary>
    ///     The binder for <see cref="TrackBar" />
    /// </summary>
    public static class TrackBarBinder
    {
        /// <summary>
        ///     binder for <see cref="TrackBar.Value" /> property
        /// </summary>
        public static readonly IPropertyBinder<TrackBar, int> ValueBinder = new PropertyBinder<TrackBar, int>("Value",
            ctrl => ctrl.Value, (ctrl, value) => ctrl.Value = value,
            (bar, action) => bar.ValueChanged += (sender, args) => action());

        /// <summary>
        ///     Get <see cref="IBindableProperty{TControl,TProperty}" /> for <see cref="TrackBar.Value" />
        /// </summary>
        /// <typeparam name="T">The control type</typeparam>
        /// <param name="ctrl">The control to get property from</param>
        /// <returns>The <see cref="IBindableProperty{TControl,TProperty}" /></returns>
        public static IBindableProperty<T, int> ValueProperty<T>(this T ctrl) where T : TrackBar
        {
            return ValueBinder.BindTo(ctrl);
        }
    }
}