namespace Zabavnov.WFMVVM
{
    using System;
    using System.ComponentModel;

    /// <summary>
    ///     declare date with range
    /// </summary>
    public interface IDateTimeWithRange : INotifyPropertyChanged
    {
        /// <summary>
        ///     specifies Start date
        /// </summary>
        DateTime? Start { get; set; }

        /// <summary>
        ///     specifies End date
        /// </summary>
        DateTime? End { get; set; }

        /// <summary>
        ///     The date
        /// </summary>
        DateTime? Value { get; set; }

        /// <summary>
        ///     true if value between Start and End
        /// </summary>
        bool IsValid { get; }
    }
}