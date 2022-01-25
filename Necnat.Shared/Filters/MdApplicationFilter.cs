using Necnat.Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Filters
{
	public class MdApplicationFilter : MdFilter<int>
	{
		public string AcronymFilter { get; set; }
		public int AcronymFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string NameFilter { get; set; }
		public int NameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string DescriptionFilter { get; set; }
		public int DescriptionFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public bool? IsActiveFilter { get; set; }
	}
}
