#region Proprietary  Notice

//  ****************************************************************************
//    Copyright 2014 Vadim Zabavnov
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
//  ****************************************************************************
//  File Name: Model.cs.
//  Created: 2014/07/07/4:09 PM.
//  Modified: 2014/07/07/4:27 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Zabavnov.MVVM;

#endregion

namespace MVVM.Sample.Models.DataSource
{
    public class Model : ModelBase<Model>, IModel
    {
        #region Static Fields

        private static readonly DataItem[] _items = Enumerable.Range(1, 15).Select(i => new DataItem { ID = i, Name = i.ToString() }).ToArray();

        #endregion

        #region Fields

        private Func<DataItem, bool> _filter = item => true;

        #endregion

        #region Constructors and Destructors

        public Model()
        {
            BusinessLogic();
        }

        #endregion

        #region Public Properties

        public bool AllItems
        {
            get { return _propertyManager.GetValue(z => z.AllItems); }
            set { _propertyManager.SetValue(z => z.AllItems, value); }
        }

        public bool EvenItems
        {
            get { return _propertyManager.GetValue(z => z.EvenItems); }
            set { _propertyManager.SetValue(z => z.EvenItems, value); }
        }

        public bool OddItems
        {
            get { return _propertyManager.GetValue(z => z.OddItems); }
            set { _propertyManager.SetValue(z => z.OddItems, value); }
        }

        public DataItem SelectedItem
        {
            get { return _propertyManager.GetValue(z => z.SelectedItem); }
            set
            {
                if(Values.Contains(value))
                    _propertyManager.SetValue(z => z.SelectedItem, value);
            }
        }

        public IList<DataItem> Values
        {
            get
            {
                return _items.Where(_filter).ToList();
            }
        }

        public Filter Filter
        {
            get { return _propertyManager.GetValue(z => z.Filter); }
            set
            {
                var selectedValue = SelectedItem;
                _propertyManager.SetValue(z => z.Filter, value);
                SelectedItem = selectedValue;
            }
        }

        #endregion

        #region Methods

        private void BusinessLogic()
        {
            AttachActionTo(SetupFilter, () => EvenItems, () => OddItems, () => AllItems);
            AttachActionTo(SetupBullets, () => Filter);
            RaisePropertyChangedOn(z => z.Values, () => Filter);
        }

        private void SetupBullets()
        {
            switch(Filter)
            {
                case Filter.All:
                    _filter = item => true;
                    AllItems = true;
                    EvenItems = false;
                    OddItems = false;
                    break;
                case Filter.Even:
                    _filter = item => (item.ID % 2) == 0;
                    AllItems = false;
                    EvenItems = true;
                    OddItems = false;
                    break;
                case Filter.Odd:
                    _filter = item => (item.ID % 2) != 0;
                    AllItems = false;
                    EvenItems = false;
                    OddItems = true;
                    break;
                default:
                    _filter = item => false;
                    AllItems = false;
                    EvenItems = false;
                    OddItems = false;
                    break;
            }
        }

        private void SetupFilter()
        {
            if(AllItems)
                Filter = Filter.All;
            else if(EvenItems)
                Filter = Filter.Even;
            else if(OddItems)
                Filter = Filter.Odd;
            else
                Filter = Filter.Unselected;
        }

        #endregion
    }
}