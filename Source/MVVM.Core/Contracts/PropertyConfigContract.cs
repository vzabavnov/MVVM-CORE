using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Zabavnov.MVVM.Contracts
{
    [ContractClassFor(typeof(IPropertyConfig<>))]
    internal abstract class PropertyConfigContract<T>: IPropertyConfig<T>
    {
        #region Implementation of IPropertyConfig<T>

        public IEqualityComparer<T> Comparer
        {
            get
            {
                Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);
                throw new NotSupportedException();
            }
            set { throw new NotSupportedException(); }
        }

        public Func<T, bool> Validator
        {
            get
            {
                Contract.Ensures(Contract.Result<Func<T, bool>>() != null);
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public Action<T> Setter
        {
            get { throw new NotSupportedException(); }
        }

        public Func<T> Getter
        {
            get { throw new NotSupportedException(); }
        }

        public void SetupStorage(T initialValue)
        {
            throw new NotSupportedException();
        }

        public void SetupStorage(Func<T> getter, Action<T> setter)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}