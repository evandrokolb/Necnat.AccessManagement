using Necnat.Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Filters
{
	public class MdApiFilter : MdFilter<int>
	{
		public int? ApplicationIdFilter { get; set; }
		public int? ModuleIdFilter { get; set; }
		public int? ControllerIdFilter { get; set; }
		public string NameFilter { get; set; }
		public int NameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string VersionFilter { get; set; }
		public int VersionFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string DescriptionFilter { get; set; }
		public int DescriptionFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public int? HttpMethodIdFilter { get; set; }
		public bool? IsActiveFilter { get; set; }
	}
}
