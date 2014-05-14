using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMSample
{
    using System.ComponentModel;

    using Zabavnov.WFMVVM;

    interface IFilterModel: INotifyPropertyChanged
    {
        string Filter { get; set; } 
        FilterDataRecord[] Data { get; set; } 
        FilterDataRecord Current { get; set; }
        ICommand AddCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand EditCommand { get; }
    }
}
