﻿namespace Zabavnov.MVVM
{
    public class SimpleDataProviderStatus : SimpleNotifiable<DataProviderStatus>
    {
        public SimpleDataProviderStatus(DataProviderStatus initialValue, object syncObj = null) : base(initialValue, syncObj)
        {
        }
    }
}