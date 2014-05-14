using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMSample
{
    using System.ComponentModel;
    using System.Drawing;

    public interface IColorModel: INotifyPropertyChanged
    {
        int R { get; set; }
        int G { get; set; }
        int B { get; set; }
        Color Color { get; }
    }
}
