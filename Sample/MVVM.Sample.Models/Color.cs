namespace MVVM.Sample.Models
{
    public struct Color
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public static Color FromArgb(int r, int g, int b)
        {
            return new Color
            {
                R = (byte) r,
                G = (byte) g,
                B = (byte) b,
                A = 0xff
            };
        }
    }
}