using Necnat.Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Filters
{
    public class MdSecurityFilter : MdFilter<int>
    {
		public int? ApplicationIdFilter { get; set; }
		public int? RoleIdFilter { get; set; }
		public bool? IsActiveFilter { get; set; }
		public int? HierarchicalStructureIdFilter { get; set; }
		public int HierarchicalStructureIdFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_EQUALS;
		public string UserIdFilter { get; set; }
		public int UserIdFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
	}
}
