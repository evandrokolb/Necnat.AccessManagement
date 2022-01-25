using Necnat.Shared.Domains;
using Necnat.Shared.Models;
using System;

namespace Necnat.Shared.Filters
{
	public class MdHierarchyComponentTypeFilter : MdFilter<int>
	{
		public string NameFilter { get; set; }
		public int NameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public bool? IsActiveFilter { get; set; }
	}
}