using Necnat.Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Filters
{
	public class MdModuleFilter : MdFilter<int>
	{
		public string NameFilter { get; set; }
		public int NameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string CodeNameFilter { get; set; }
		public int CodeNameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string DescriptionFilter { get; set; }
		public int DescriptionFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public int? ApplicationIdFilter { get; set; }
		public bool? IsActiveFilter { get; set; }

	}
}
