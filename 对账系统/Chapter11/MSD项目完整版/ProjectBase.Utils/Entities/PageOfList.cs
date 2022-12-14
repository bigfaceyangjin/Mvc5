//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace ProjectBase.Utils.Entities
{
    public class PageOfList<T> : List<T>, IList<T>, IPageOfList,IPageOfList<T>
    {
        public PageOfList(IEnumerable<T> items, int pageIndex, int pageSize, long recordTotal)
        {
            if (items != null)
                AddRange(items);
            PageIndex = pageIndex;
            PageSize = pageSize;
            RecordTotal = recordTotal;
        }

        public PageOfList(int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentException("pageSize must gart 0", "pageSize");
            }
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageTotal
        {
            get
            {
                return (int)RecordTotal / PageSize + (RecordTotal % PageSize > 0 ? 1 : 0);
            }
        }

        public long RecordTotal { get; set; }

        public long CurrentStart
        {
            get
            {
                return PageIndex * PageSize + 1;
            }
        }

        public long CurrentEnd
        {
            get
            {
                return (PageIndex + 1) * PageSize > RecordTotal ? RecordTotal : (PageIndex + 1) * PageSize;
            }
        }
    }
}
