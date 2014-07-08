using System;
using System.ComponentModel;
using System.Windows.Forms;
using Zabavnov.MVVM;

namespace Zabavnov.Windows.Forms.MVVM
{
    public static class FormBinders
    {
        public static void BindCommandToClose<TModel, TControl>(this TModel model,
            Func<TModel, IExternalCommand> commandSelector, TControl control)
            where TControl : Form
        {
            var data = new CloseCommandBindingData<TControl>(control, commandSelector(model));
            control.Closing += data.OnClosing;
        }

        private class CloseCommandBindingData<T> where T : Form
        {
            private readonly IExternalCommand _command;
            private readonly T _control;

            private bool _fromAction;

            public CloseCommandBindingData(T control, IExternalCommand command)
            {
                _control = control;
                _command = command;
                _command.Action = ExtAction;
            }

            private bool ExtAction()
            {
                bool old = _fromAction;
                try
                {
                    _fromAction = true;
                    _control.Close();
                    return true;
                }
                finally
                {
                    _fromAction = old;
                }
            }

            public void OnClosing(object obj, CancelEventArgs args)
            {
                if (!_fromAction)
                    args.Cancel = !_command.CanExecute();
            }
        }
    }
}