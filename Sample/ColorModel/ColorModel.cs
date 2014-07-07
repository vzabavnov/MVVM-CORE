namespace MVVMSample.ColorModel
{
    using System.Drawing;

    using Zabavnov.MVVM;

    /// <summary>
    /// </summary>
    internal class ColorModel : ModelBase<ColorModel>, IColorModel
    {
        static bool CheckValue(int value)
        {
            return value >= 0 && value <= 255;
        }

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public ColorModel()
        {
            InitializeBusinessLogic();
            _propertyManager.RegisterProperty(z=>z.B, SetupProperty);
            _propertyManager.RegisterProperty(z => z.G, SetupProperty);
            _propertyManager.RegisterProperty(z => z.R, SetupProperty);
        }

        private void SetupProperty(IPropertyConfig<int> info)
        {
            info.Validator = CheckValue;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public int B
        {
            get
            {
                return _propertyManager.GetValue(m => m.B);
            }

            set
            {
                _propertyManager.SetValue(m=>m.B, value);
            }
        }

        /// <summary>
        /// </summary>
        public Color Color
        {
            get
            {
                return Color.FromArgb(R, G, B);
            }
        }

        /// <summary>
        /// </summary>
        public int G
        {
            get
            {
                return _propertyManager.GetValue(m => m.G);
            }

            set
            {
                _propertyManager.SetValue(m => m.G, value);
            }
        }

        /// <summary>
        /// </summary>
        public int R
        {
            get
            {
                return _propertyManager.GetValue(m => m.R);
            }

            set
            {
                _propertyManager.SetValue(m => m.R, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void InitializeBusinessLogic()
        {
            RaisePropertyChangedOn(z => z.Color, () => R, () => G, () => B);
        }

        #endregion
    }
}