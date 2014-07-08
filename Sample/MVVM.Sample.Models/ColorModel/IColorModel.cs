using System.ComponentModel;

namespace MVVM.Sample.Models.ColorModel
{
    public interface IColorModel: INotifyPropertyChanged
    {
        int R { get; set; }
        int G { get; set; }
        int B { get; set; }
        Color Color { get; }
    }
}
