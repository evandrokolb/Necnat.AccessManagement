using Necnat.Shared.Domains;
using Necnat.Shared.Models;
using System;

namespace Necnat.Shared.Filters
{
	public class MdHierarchyFilter : MdFilter<int>
	{
		public bool? IsActiveFilter { get; set; }
		public string NameFilter { get; set; }
		public int NameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string DescriptionFilter { get; set; }
		public int DescriptionFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;

	}
}