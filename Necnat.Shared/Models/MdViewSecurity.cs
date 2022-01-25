using Necnat.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Models
{
    public class MdViewSecurity
    {
        public Security Security { get; set; }
        public string UserName { get; set; }
        public string HierarchyName { get; set; }
        public string HierarchyComponentTypeName { get; set; }
        public string HierarchyComponentName { get; set; }
    }
}
