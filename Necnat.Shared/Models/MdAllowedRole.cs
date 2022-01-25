using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Models
{
    public class MdAllowedRole
    {
        public int RoleId { get; set; }
        public string RoleCodeName { get; set; }
        public List<MdHierarchyComponent> HierarchyComponentList { get; set; }
    }
}
