using System.Windows.Forms;
using Zabavnov.MVVM;

namespace Zabavnov.Windows.Forms.MVVM
{
    public static class RadioButtonBinders
    {
        public static readonly IPropertyBinder<RadioButton, bool> CheckedBinder = new PropertyBinder<RadioButton, bool>(
            ConstDef.CHECKED,
            box => box.Checked,
            (box, b) => box.Checked = b,
            (box, action) => box.CheckedChanged += (sender, args) => action());

        public static IBindableProperty<RadioButton, bool> CheckedProperty(this RadioButton ctrl)
        {
            return CheckedBinder.BindTo(ctrl);
        }
    }
}
