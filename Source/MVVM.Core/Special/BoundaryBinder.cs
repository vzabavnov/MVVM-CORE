namespace Zabavnov.MVVM
{
    using System.Diagnostics.Contracts;

    /// <summary>
    ///     Bind boundaries two <see cref="DateTimeWithRange" />
    /// </summary>
    public class BoundaryBinder
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly IDateTimeWithRange _lower;

        /// <summary>
        /// </summary>
        private readonly IDateTimeWithRange _upper;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="lower">
        /// </param>
        /// <param name="upper">
        /// </param>
        public BoundaryBinder(IDateTimeWithRange lower, IDateTimeWithRange upper)
        {
            Contract.Requires(lower != null);
            Contract.Requires(upper != null);
            Contract.Requires(lower.Start.HasValue && lower.End.HasValue && lower.Start <= lower.End);
            Contract.Requires(upper.Start.HasValue && upper.End.HasValue && upper.Start <= upper.End);

            _lower = lower;
            _upper = upper;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public IDateTimeWithRange Lower
        {
            get
            {
                return _lower;
            }
        }

        /// <summary>
        /// </summary>
        public BoundaryBinderStrategy UpdateStrategy { get; set; }

        /// <summary>
        /// </summary>
        public IDateTimeWithRange Upper
        {
            get
            {
                return _upper;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void Bind()
        {
            if (Lower.Value.HasValue)
            {
                Upper.Start = Lower.Value;
            }

            if (Upper.Value.HasValue)
            {
                Lower.End = Upper.Value;
            }

            ExpressionExtensions.AttachActionTo(OnLowerValueChanged, () => Lower.Value);
            ExpressionExtensions.AttachActionTo(OnUpperValueChanged, () => Upper.Value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void OnLowerValueChanged()
        {
            if (Lower.Value.HasValue)
            {
                if (!Upper.End.HasValue || Lower.Value <= Upper.End)
                {
                    Upper.Start = Lower.Value;
                }
                else
                {
                    Upper.Start = Upper.End;
                }
            }
            else
            {
                Upper.Start = null;
            }

            if (Upper.Value.HasValue && Upper.Value < Upper.Start)
            {
                switch (UpdateStrategy)
                {
                    case BoundaryBinderStrategy.Clear:
                        Upper.Value = null;
                        break;
                    case BoundaryBinderStrategy.Update:
                        Upper.Value = Lower.End;
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        private void OnUpperValueChanged()
        {
            if (Upper.Value.HasValue)
            {
                if (!Lower.Start.HasValue || Upper.Value >= Lower.Start)
                {
                    Lower.End = Upper.Value;
                }
                else
                {
                    Lower.End = Lower.Start;
                }
            }
            else
            {
                Lower.End = null;
            }

            if (Lower.Value.HasValue && Lower.Value > Lower.End)
            {
                switch (UpdateStrategy)
                {
                    case BoundaryBinderStrategy.Clear:
                        Lower.Value = null;
                        break;
                    case BoundaryBinderStrategy.Update:
                        Lower.Value = Lower.End;
                        break;
                }
            }
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvaiant()
        {
            Contract.Invariant(_lower != null);
            Contract.Invariant(_upper != null);
        }
    }
}