namespace Zabavnov.WFMVVM
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    ///     contains some method related to The Model
    /// </summary>
    public static class Model
    {
        /// <summary>
        ///     add <paramref name="action" /> on property changes specified by <paramref name="propertyLambda" /> and
        ///     <paramref name="propertyLambdas" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <param name="propertyLambda"></param>
        /// <param name="propertyLambdas"></param>
        public static void AddActionOn<T>(this T source, Action action, Expression<Func<T, object>> propertyLambda,
            params Expression<Func<T, object>>[] propertyLambdas) where T : INotifyPropertyChanged
        {
            propertyLambdas.AddHead(propertyLambda).Select(z => z.GetPropertyName()).Distinct().ForEach(name =>
            {
                source.PropertyChanged += (sender, args) =>
                {
                    if(args.PropertyName == name)
                        action();
                };
            });
        }

        /// <summary>
        ///     raise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyLambda"></param>
        /// <param name="propertyLambdas"></param>
        public static void RaisePropertyChangedEvent<T>(this T source, Expression<Func<T, object>> propertyLambda,
            params Expression<Func<T, object>>[] propertyLambdas) where T : INotifyPropertyChanged
        {
            var names = propertyLambdas.AddHead(propertyLambda).Select(z => z.GetPropertyName()).Distinct().ToArray();

            // get the internal eventDelegate
            var bindableObjectType = typeof(T);

            const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;

            // search the base type, which contains the PropertyChanged event field.
            FieldInfo propChangedFieldInfo = null;
            while(bindableObjectType != null)
            {
                propChangedFieldInfo = bindableObjectType.GetField("PropertyChanged", BINDING_FLAGS);
                if(propChangedFieldInfo != null)
                    break;

                bindableObjectType = bindableObjectType.BaseType;
            }
            if(propChangedFieldInfo == null)
                return;

            // get prop changed event field value
            var fieldValue = propChangedFieldInfo.GetValue(source);
            if(fieldValue == null)
                return;

            var eventDelegate = fieldValue as MulticastDelegate;
            if(eventDelegate == null)
                return;

            // get invocation list
            var delegates = eventDelegate.GetInvocationList();

            // invoke each delegate
            foreach(var propertyChangedDelegate in delegates)
                foreach(var name in names)
                    propertyChangedDelegate.Method.Invoke(propertyChangedDelegate.Target,
                        new object[] {source, new PropertyChangedEventArgs(name)});
        }
    }
}