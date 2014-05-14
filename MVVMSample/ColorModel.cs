#region Usings

using System.Drawing;
using Zabavnov.WFMVVM;

#endregion

namespace MVVMSample
{

    /// <summary>
    /// </summary>
    internal class ColorModel : ModelBase<ColorModel>, IColorModel
    {
        #region Fields

        /// <summary>
        /// </summary>
        private int _b;

        /// <summary>
        /// </summary>
        private int _g;

        /// <summary>
        /// </summary>
        private int _r;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public ColorModel()
        {
            InitializeBusinessLogic();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public int B
        {
            get
            {
                return _b;
            }

            set
            {
                if (_b != value && (value >= 0 && value <= 255))
                {
                    _b = value;
                    RaisePropertyChanged(z => z.B);
                }
            }
        }

        /// <summary>
        /// </summary>
        public Color Color
        {
            get
            {
                return Color.FromArgb(_r, _g, _b);
            }
        }

        /// <summary>
        /// </summary>
        public int G
        {
            get
            {
                return _g;
            }

            set
            {
                if (_g != value && (value >= 0 && value <= 255))
                {
                    _g = value;
                    RaisePropertyChanged(z => z.G);
                }
            }
        }

        /// <summary>
        /// </summary>
        public int R
        {
            get
            {
                return _r;
            }

            set
            {
                if (_r != value && (value >= 0 && value <= 255))
                {
                    _r = value;
                    RaisePropertyChanged(z => z.R);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void InitializeBusinessLogic()
        {
            RaisePropertyChangedOn(z => z.Color, model => model.R, model => model.G, model => model.B);
        }

        #endregion
    }
}