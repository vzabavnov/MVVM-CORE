using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
namespace MVVM.Core.Tests
{
    public class ModelTestClass: INotifyPropertyChanged
    {
        private string _text;
        private int _number;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if(handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if(_text != value)
                {
                    _text = value;
                    OnPropertyChanged("Text");
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
                    OnPropertyChanged("Number");
                }
            }
        }
    }
}
