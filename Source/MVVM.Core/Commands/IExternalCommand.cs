namespace Zabavnov.MVVM
{
    using System;

    public interface IExternalCommand: ICommand
    {
        /// <summary>
        /// The <see cref="Action"/> with will be executed during command's Execute
        /// </summary>
        Func<bool> Action { get; set; }
    }

    public interface IExternalCommand<T> : ICommand<T>
    {
        /// <summary>
        /// The <see cref="Action"/> with will be executed during command's Execute
        /// </summary>
        Func<T, bool> Action { get; set; }
    }

    public interface IExternalCommand<T1, T2> : ICommand<T1, T2>
    {
        /// <summary>
        /// The <see cref="Action"/> with will be executed during command's Execute
        /// </summary>
        Func<T1, T2, bool> Action { get; set; }
    }
}
