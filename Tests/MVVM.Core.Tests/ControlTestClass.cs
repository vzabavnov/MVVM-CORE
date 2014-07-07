using System;

namespace MVVM.Core.Tests
{
    public class ControlTestClass
    {
        #region Fields

        private readonly int _readOnly;
        private string _text;
        private int _number;

        #endregion

        #region Constructors and Destructors

        public ControlTestClass()
        {
            _readOnly = 38;
        }

        #endregion

        #region Public Events

        public event EventHandler TextChanged;

        public event EventHandler NumberChanged;

        protected virtual void OnNumberChanged()
        {
            var handler = NumberChanged;
            if(handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion

        #region Public Properties

        public int ReadOnly
        {
            get { return _readOnly; }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if(_text != value)
                {
                    _text = value;
                    OnChanges();
                }
            }
        }

        public int Number
        {
            get { return _number; }
            set
            {
                if(_number != value)
                {
                    _number = value;
                    OnNumberChanged();
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        public virtual void OnChanges()
        {
            var handler = TextChanged;
            if(handler != null)
                handler(this, EventArgs.Empty);
        }

        public void SetValue(int v)
        {
            OnChanges();
        }

        #endregion
    }
}