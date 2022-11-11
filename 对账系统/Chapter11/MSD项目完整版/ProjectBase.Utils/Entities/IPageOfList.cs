//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;

namespace ProjectBase.Utils.Entities
{
    public interface IPageOfList
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }

        int PageTotal { get; }

        long RecordTotal { get; set; }
    }

    public interface IPageOfList<T> : IPageOfList, IList<T>
    {
    }
}
