using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Zabavnov.MVVM.Contracts;

namespace Zabavnov.MVVM
{
    [ContractClass(typeof(PropertyConfigContract<>))]
    public interface IPropertyConfig<TProperty>
    {
        IEqualityComparer<TProperty> Comparer { get; set; }
        Func<TProperty, bool> Validator { get; set; }
        Action<TProperty> Setter { get; }
        Func<TProperty> Getter { get; }

        void SetupStorage(TProperty initialValue);

        void SetupStorage(Func<TProperty> getter, Action<TProperty> setter);
    }
}