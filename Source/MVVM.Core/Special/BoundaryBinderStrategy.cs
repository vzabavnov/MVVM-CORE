namespace Zabavnov.MVVM
{
    public enum BoundaryBinderStrategy
    {
        /// <summary>
        ///     Set to invalid state
        /// </summary>
        Invalid,

        /// <summary>
        ///     update value to be in range
        /// </summary>
        Update,

        /// <summary>
        ///     clear value
        /// </summary>
        Clear
    }
}