namespace Zabavnov.MVVM
{
    /// <summary>
    /// Describes the direction of the data flow in a binding.
    /// </summary>
    /// <remarks>this is very similar with <see cref="System.Windows.Data.BindingMode"/></remarks>
    public enum BindingMode
    {
        /// <summary>
        /// Causes changes to either the source property or the target property to automatically update the other. 
        /// </summary>
        TwoWay,
        
        /// <summary>
        /// Updates the binding target (target) property when the binding source (source) changes. 
        /// If there is no need to monitor the changes of the target property, using the OneWay binding mode avoids the overhead of the TwoWay binding mode
        /// </summary>
        OneWay,
        
        /// <summary>
        /// Updates the binding target when the application starts. 
        /// </summary>
        OneTime,

        /// <summary>
        /// Updates the source property when the target property changes.
        /// </summary>
        OneWayToSource,

        /// <summary>
        /// Uses the <see cref="IBindableProperty{TControl,TProperty}.DefaultBindingMode"/>
        /// </summary>
        Default,
    }
}
