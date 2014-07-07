namespace MVVMSample.ColorModel
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
