namespace Zabavnov.Windows.Forms.MVVM
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Zabavnov.MVVM;

    public static class FormBinders
    {
        class CloseCommandBindingData<T> where T: Form
        {
            private readonly T _control;
            private readonly IExternalCommand _command;

            public CloseCommandBindingData(T control, IExternalCommand command)
            {
                _control = control;
                _command = command;
                _command.Action = ExtAction;
            }

            bool _fromAction;

            private bool ExtAction()
            {
                var old = _fromAction;
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

        public static void BindCommandToClose<TModel, TControl>(this TModel model, Func<TModel, IExternalCommand> commandSelector, TControl control)
            where TControl: Form
        {
            var data = new CloseCommandBindingData<TControl>(control, commandSelector(model));
            control.Closing += data.OnClosing;
        }
    }
}
