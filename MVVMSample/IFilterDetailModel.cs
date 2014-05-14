using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMSample
{
    interface IFilterDetailModel
    {
        string Name { get; set; }
        int Age { get; set; }
        bool IsReadOnly { get; set; }
        bool IsValid { get; set; }
    }
}
