namespace Zabavnov.WFMVVM
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    ///     Binders for <see cref="Control" />
    /// </summary>
    public static class ControlBinders
    {
        #region Visibility helpers

        private static void SetVisibility(Control ctrl, Visibility visibility)
        {
            switch(visibility)
            {
                case Visibility.Visible:
                    ctrl.Visible = true;
                    ctrl.Enabled = true;
                    break;
                case Visibility.Disabled:
                    ctrl.Visible = true;
                    ctrl.Enabled = false;
                    break;
                default:
                    ctrl.Visible = false;
                    break;
            }
        }

        private static Visibility GetVisibility(Control ctrl)
        {
            return ctrl.Visible ? (ctrl.Enabled ? Visibility.Visible : Visibility.Disabled) : Visibility.Hiden;
        }

        private static void SetVisibilityNotification(Control ctrl, Action actionToSet)
        {
            ctrl.EnabledChanged += (sender, args) => actionToSet();
            ctrl.VisibleChanged += (sender, args) => actionToSet();
        }

        #endregion

        #region Binders

        /// <summary>
        ///     the binder for <see cref="Visibility" /> virtual property
        /// </summary>
        public static readonly IPropertyBinder<Control, Visibility> VisibilityBinder = new PropertyBinder<Control, Visibility>("Visibility",
            GetVisibility, SetVisibility, SetVisibilityNotification);

        public static readonly IPropertyBinder<Control, bool> EnabledBinder = new PropertyBinder<Control, bool>(z => z.Enabled,
            (control, action) => control.EnabledChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, bool> VisibleBinder = new PropertyBinder<Control, bool>(z => z.Visible,
            (control, action) => control.VisibleChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Size> SizeBinder = new PropertyBinder<Control, Size>(z => z.Size,
            (control, action) => control.SizeChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Point> LocationBinder = new PropertyBinder<Control, Point>(z => z.Location,
            (control, action) => control.LocationChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, bool> AutoSizeBinder = new PropertyBinder<Control, bool>(z => z.AutoSize,
            (control, action) => control.AutoSizeChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Color> BackColorBinder = new PropertyBinder<Control, Color>(z => z.BackColor,
            (control, action) => control.BackColorChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Image> BackgroundImageBinder = new PropertyBinder<Control, Image>(z => z.BackgroundImage,
            (control, action) => control.BackgroundImageChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, ImageLayout> BackgroundImageLayoutBinder =
            new PropertyBinder<Control, ImageLayout>(z => z.BackgroundImageLayout,
                (control, action) => control.BackgroundImageLayoutChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, BindingContext> BindingContextBinder =
            new PropertyBinder<Control, BindingContext>(z => z.BindingContext,
                (control, action) => control.BindingContextChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Size> ClientSizeBinder = new PropertyBinder<Control, Size>(z => z.ClientSize,
            (control, action) => control.ClientSizeChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, ContextMenu> ContextMenuBinder = new PropertyBinder<Control, ContextMenu>(z => z.ContextMenu,
            (control, action) => control.ContextMenuChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Cursor> CursorBinder = new PropertyBinder<Control, Cursor>(z => z.Cursor,
            (control, action) => control.CursorChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, DockStyle> DockBinder = new PropertyBinder<Control, DockStyle>(z => z.Dock,
            (control, action) => control.DockChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Font> FontBinder = new PropertyBinder<Control, Font>(z => z.Font,
            (control, action) => control.FontChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Color> ForeColorBinder = new PropertyBinder<Control, Color>(z => z.ForeColor,
            (control, action) => control.ForeColorChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Padding> MarginBinder = new PropertyBinder<Control, Padding>(z => z.Margin,
            (control, action) => control.MarginChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Padding> PaddingBinder = new PropertyBinder<Control, Padding>(z => z.Padding,
            (control, action) => control.PaddingChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Control> ParentBinder = new PropertyBinder<Control, Control>(z => z.Parent,
            (control, action) => control.ParentChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, Region> RegionBinder = new PropertyBinder<Control, Region>(z => z.Region,
            (control, action) => control.RegionChanged += (sender, args) => action());

        public static readonly IPropertyBinder<Control, string> TextBinder = new PropertyBinder<Control, string>(z => z.Text,
            (control, action) => control.TextChanged += (sender, args) => action());

        #endregion

        #region Extensions

        public static IBindableProperty<T, string> TextProperty<T>(this T ctrl) where T : Control
        {
            return TextBinder.BindTo(ctrl);
        }

        public static IBindableProperty<T, bool> EnabledProperty<T>(this T ctrl) where T : Control
        {
            return EnabledBinder.BindTo(ctrl);
        }

        public static IBindableProperty<T, Color> BackColorProperty<T>(this T ctrl) where T : Control
        {
            return BackColorBinder.BindTo(ctrl);
        }

        #endregion
    }
}