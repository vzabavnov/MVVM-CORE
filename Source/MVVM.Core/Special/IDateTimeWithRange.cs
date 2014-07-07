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
//  File Name: IDateTimeWithRange.cs.
//  Created: 2014/05/30/5:12 PM.
//  Modified: 2014/06/10/3:18 PM.
//  ****************************************************************************

#endregion

#region Usings

using System;
using System.ComponentModel;

#endregion

namespace Zabavnov.MVVM
{
    /// <summary>
    ///     declare date with range
    /// </summary>
    public interface IDateTimeWithRange : INotifyPropertyChanged
    {
        #region Public Properties

        /// <summary>
        ///     specifies End date
        /// </summary>
        DateTime? End { get; set; }

        /// <summary>
        ///     true if value between Start and End
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        ///     specifies Start date
        /// </summary>
        DateTime? Start { get; set; }

        /// <summary>
        ///     The date
        /// </summary>
        DateTime? Value { get; set; }

        #endregion
    }
}