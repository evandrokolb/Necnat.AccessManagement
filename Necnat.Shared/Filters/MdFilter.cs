using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Filters
{
    public abstract class MdFilter<TKey>
    {
        public bool IsPaging { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string OrderBy { get; set; }
        public List<TKey> WithoutIdList { get; set; }
    }
}
