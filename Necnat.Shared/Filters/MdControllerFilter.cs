using Necnat.Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Filters
{
	public class MdControllerFilter : MdFilter<int>
	{
		public int? ApplicationIdFilter { get; set; }
		public int? ModuleIdFilter { get; set; }
		public string NameFilter { get; set; }
		public int NameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string DescriptionFilter { get; set; }
		public int DescriptionFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public bool? IsActiveFilter { get; set; }

	}
}
