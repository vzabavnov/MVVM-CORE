namespace MVVMSample
{
    using System;
    using System.Collections.Generic;

    using Zabavnov.WFMVVM;

    internal class FilterModel : ModelBase<FilterModel>, IFilterModel
    {
        private string _filter;

        private FilterDataRecord[] _data;

        private FilterDataRecord _current;

        private ICommand _addCommand;

        private ICommand _deleteCommand;

        private ICommand _editCommand;

        #region Implementation of IFilterModel

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    RaisePropertyChanged(model=>model.Filter);
                }
            }
        }

        public FilterDataRecord[] Data
        {
            get
            {
                return _data;
            }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    RaisePropertyChanged(model=>model.Data);
                }
            }
        }

        public FilterDataRecord Current
        {
            get
            {
                return _current;
            }
            set
            {
                if (value != _current)
                {
                    if (value == null || Array.FindIndex(Data, r => r == value) != -1)
                    {
                        _current = value;
                        RaisePropertyChanged(model => model.Current);
                    }
                }
            }
        }

        public ICommand AddCommand
        {
            get
            {
               return _addCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return _editCommand;
            }
        }

        #endregion
    }
}