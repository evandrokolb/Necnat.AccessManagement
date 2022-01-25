using Necnat.Shared.Domains;

namespace Necnat.Shared.Filters
{
    public class MdRoleFilter : MdFilter<int>
	{
		public int? ApplicationIdFilter { get; set; }
		public bool? IsActiveFilter { get; set; }
		public string NameFilter { get; set; }
		public int NameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string CodeNameFilter { get; set; }
		public int CodeNameFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;
		public string DescriptionFilter { get; set; }
		public int DescriptionFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_CONTAINS;

	}
}