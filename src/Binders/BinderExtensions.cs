namespace Zabavnov.WFMVVM
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    public static class BinderExtensions
    {
        /// <summary>
        ///     attach action on changes for specified property
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="propertyLambda"></param>
        /// <param name="action">
        ///     the action that attached to the specified property change. The delegate passed as parameter and it
        ///     can be used to get current value of property
        /// </param>
        [DebuggerStepThrough]
        public static void AttachActionOn<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> propertyLambda,
                                                             Action<Func<TProperty>> action) where TModel : INotifyPropertyChanged
        {
            var propName = propertyLambda.GetPropertyName();
            var valueGetter = propertyLambda.Compile();

            model.PropertyChanged += (sender, args) =>
                {
                    if(args.PropertyName == propName)
                        action(() => valueGetter(model));
                };
        }

        [DebuggerStepThrough]
        public static void BindTo<TModel, TControl, TProperty>(this TModel model, TControl control,
                                                               Expression<Func<TModel, TProperty>> modelPropertyLambda, Expression<Func<TControl, TProperty>> controlPropertyLambda,
                                                               BindingDirection direction) where TModel : class, INotifyPropertyChanged where TControl : class
        {
            var propInfo = controlPropertyLambda.GetPropertyInfo();
            var getter = propInfo.CanRead ? controlPropertyLambda.Compile() : null;
            var setter = propInfo.CanWrite ? controlPropertyLambda.GetSetter() : null;
            var prop = new BindableProperty<TControl, TProperty>(control, propInfo.Name, getter, setter);

            BindTo(model, modelPropertyLambda, prop, direction);
        }

        /// <summary>
        ///     bind model to control
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TControl"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="propertyLambda"></param>
        /// <param name="property"></param>
        /// <param name="direction"></param>
        [DebuggerStepThrough]
        public static void BindTo<TModel, TControl, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> propertyLambda,
                                                               IBindableProperty<TControl, TProperty> property, BindingDirection direction = BindingDirection.OneWay)
            where TModel : class, INotifyPropertyChanged where TControl : class
        {
            BindTo(model, propertyLambda, property, DataConverter<TProperty>.EmptyConverter, direction);
        }

        /// <summary>
        ///     bind model to control
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TControl"></typeparam>
        /// <typeparam name="TModelProperty"></typeparam>
        /// <typeparam name="TControlProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="propertyLambda"></param>
        /// <param name="property"></param>
        /// <param name="converter"></param>
        /// <param name="direction"></param>
        [DebuggerStepThrough]
        public static void BindTo<TModel, TControl, TModelProperty, TControlProperty>(this TModel model,
                                                                                      Expression<Func<TModel, TModelProperty>> propertyLambda, IBindableProperty<TControl, TControlProperty> property,
                                                                                      IDataConverter<TModelProperty, TControlProperty> converter, BindingDirection direction) where TModel : class, INotifyPropertyChanged
            where TControl : class
        {
            Contract.Requires(model != null);

            var modelPropertyName = propertyLambda.GetPropertyName();

            var modPropInfo = propertyLambda.GetPropertyInfo();

            if(!modPropInfo.CanRead)
                throw new ArgumentException(string.Format("Model's property {0} doesn't have getter", modelPropertyName));
            if(!property.CanWrite)
                throw new ArgumentException(string.Format("Control's property {0} doesn't have setter", modelPropertyName));

            model.AttachActionOn(propertyLambda, modelProperty => property.Value = converter.ConvertTo(modelProperty()));

            if(direction == BindingDirection.TwoWay)
            {
                if(!modPropInfo.CanWrite)
                    throw new ArgumentException(string.Format("Model's property {0} doesn't have setter", modelPropertyName));
                if(!property.CanRead)
                    throw new ArgumentException(string.Format("Control's property {0} doesn't have getter", modelPropertyName));

                var modelSetter = propertyLambda.GetSetter();

                property.PropertyChanged += (sender, args) => modelSetter(model, converter.ConvertFrom(property.Value));
            }

            var mGetter = propertyLambda.Compile();
            var mValue = converter.ConvertTo(mGetter(model));
            if(!Equals(property.Value, mValue))
                property.Value = mValue;
        }
    }
}