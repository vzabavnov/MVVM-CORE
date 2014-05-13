namespace Zabavnov.WFMVVM
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    ///     Bind boundaries two <see cref="DateTimeWithRange" />
    /// </summary>
    public class BoundaryBinder
    {
        private readonly IDateTimeWithRange _lower;
        private readonly IDateTimeWithRange _upper;

        public BoundaryBinder(IDateTimeWithRange lower, IDateTimeWithRange upper)
        {
            Contract.Requires(lower != null);
            Contract.Requires(upper != null);
            Contract.Requires(lower.Start.HasValue && lower.End.HasValue && lower.Start <= lower.End);
            Contract.Requires(upper.Start.HasValue && upper.End.HasValue && upper.Start <= upper.End);

            this._lower = lower;
            this._upper = upper;
        }

        public BoundaryBinderStrategy UpdateStrategy { get; set; }

        public IDateTimeWithRange Lower
        {
            get { return this._lower; }
        }

        public IDateTimeWithRange Upper
        {
            get { return this._upper; }
        }

        public void Bind()
        {
            if(this.Lower.Value.HasValue)
                this.Upper.Start = this.Lower.Value;

            if(this.Upper.Value.HasValue)
                this.Lower.End = this.Upper.Value;

            this.Lower.AttachActionOn(z => z.Value, this.OnLowerValueChanged);
            this.Upper.AttachActionOn(z => z.Value, this.OnUpperValueChanged);
        }

        private void OnUpperValueChanged(Func<DateTime?> selector)
        {
            if(this.Upper.Value.HasValue)
                if(!this.Lower.Start.HasValue || this.Upper.Value >= this.Lower.Start)
                    this.Lower.End = this.Upper.Value;
                else
                    this.Lower.End = this.Lower.Start;
            else
                this.Lower.End = null;

            if(this.Lower.Value.HasValue && this.Lower.Value > this.Lower.End)
                switch(this.UpdateStrategy)
                {
                    case BoundaryBinderStrategy.Clear:
                        this.Lower.Value = null;
                        break;
                    case BoundaryBinderStrategy.Update:
                        this.Lower.Value = this.Lower.End;
                        break;
                }
        }

        private void OnLowerValueChanged(Func<DateTime?> selector)
        {
            if(this.Lower.Value.HasValue)
                if(!this.Upper.End.HasValue || this.Lower.Value <= this.Upper.End)
                    this.Upper.Start = this.Lower.Value;
                else
                    this.Upper.Start = this.Upper.End;
            else
                this.Upper.Start = null;

            if(this.Upper.Value.HasValue && this.Upper.Value < this.Upper.Start)
                switch(this.UpdateStrategy)
                {
                    case BoundaryBinderStrategy.Clear:
                        this.Upper.Value = null;
                        break;
                    case BoundaryBinderStrategy.Update:
                        this.Upper.Value = this.Lower.End;
                        break;
                }
        }
    }
}