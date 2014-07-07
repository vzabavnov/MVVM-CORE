namespace MVVMSample.YesNoModel
{
    #region Usings

    using System.Windows.Forms;
    using Zabavnov.MVVM;

    #endregion

    /// <summary>
    /// </summary>
    public class YesNoModel : ModelBase<YesNoModel>, IYesNoModel
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly IExternalCommand _closeCommand;

        /// <summary>
        /// </summary>
        private readonly ICommand _noCommand;

        /// <summary>
        /// </summary>
        private readonly ICommand _yesCommand;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public YesNoModel()
        {
            _closeCommand = new ExternalCommand(true, ExecuteFormClose, AllowCloseCommand);
            _yesCommand = new Command(true, ExecuteYesCommand, AllowExecuteYesCommand);
            _noCommand = new Command(true, ExecuteNoCommand);

            BusinessLogic();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public IExternalCommand CloseCommand
        {
            get { return _closeCommand; }
        }

        /// <summary>
        /// </summary>
        public bool HasChanges
        {
            get { return _propertyManager.GetValue(m => m.HasChanges); }
            set { _propertyManager.SetValue(m => m.HasChanges, value); }
        }

        /// <summary>
        /// </summary>
        public string Message
        {
            get { return _propertyManager.GetValue(m => m.Message); }
            set { _propertyManager.SetValue(m => m.Message, value); }
        }

        /// <summary>
        /// </summary>
        public ICommand NoCommand
        {
            get { return _noCommand; }
        }

        /// <summary>
        /// </summary>
        public string NoCommandText
        {
            get { return _propertyManager.GetValue(m => m.NoCommandText); }
            set { _propertyManager.SetValue(m => m.NoCommandText, value); }
        }

        /// <summary>
        /// </summary>
        public DialogResult? Result
        {
            get { return _propertyManager.GetValue(z => z.Result); }
            private set { _propertyManager.SetValue(z => z.Result, value); }
        }

        /// <summary>
        /// </summary>
        public string Title
        {
            get { return _propertyManager.GetValue(m => m.Title); }
            set { _propertyManager.SetValue(m => m.Title, value); }
        }

        /// <summary>
        /// </summary>
        public ICommand YesCommand
        {
            get { return _yesCommand; }
        }

        /// <summary>
        /// </summary>
        public string YesCommandText
        {
            get { return _propertyManager.GetValue(m => m.YesCommandText); }
            set { _propertyManager.SetValue(m => m.YesCommandText, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool AllowCloseCommand()
        {
            return Result == DialogResult.Yes || !HasChanges;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool AllowExecuteYesCommand()
        {
            return HasChanges;
        }

        /// <summary>
        /// </summary>
        private void BusinessLogic()
        {
            CheckCommandOn(YesCommand, () => HasChanges);
        }

        /// <summary>
        /// </summary>
        private void ExecuteFormClose()
        {
            RaisePropertyChanged(z => z.Result);
        }

        /// <summary>
        /// </summary>
        private void ExecuteNoCommand()
        {
            Result = DialogResult.No;
            CloseCommand.Execute();
        }

        /// <summary>
        /// </summary>
        private void ExecuteYesCommand()
        {
            Result = DialogResult.Yes;
            CloseCommand.Execute();
        }

        #endregion
    }
}