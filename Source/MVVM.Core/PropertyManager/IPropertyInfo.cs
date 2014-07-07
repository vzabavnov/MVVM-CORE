namespace Zabavnov.MVVM
{
    using System;
    using System.Collections.Generic;

    public interface IPropertyInfo
    {
        string Name { get; }
        bool HasChanged { get; }
    }

    public interface IPropertyInfo<T> : IPropertyInfo
    {
        IEqualityComparer<T> Comparer { get; set; }
        T Value { get; set; }
        Func<T> Getter { get; set; }
        Action<T> Setter { get; set; }
        T OriginalValue { get; }
        Func<T, bool> Validator { get; set; }
    }
}