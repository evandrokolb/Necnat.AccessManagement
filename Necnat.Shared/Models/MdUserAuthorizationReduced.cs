using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Models
{
    public class MdUserAuthorizationReduced
    {
        public string UserId { get; set; }
        public int ApplicationId { get; set; }
        public List<MdAllowedRoleReduced> AllowedRoleReducedList { get; set; }
        public List<MdAllowedFeatureReduced> AllowedFeatureReducedList { get; set; }
        public List<MdAllowedModuleReduced> AllowedModuleReducedList { get; set; }
        public List<MdHierarchyComponentReducedJoin> HierarchyComponentReducedJoinList { get; set; }
    }
}
