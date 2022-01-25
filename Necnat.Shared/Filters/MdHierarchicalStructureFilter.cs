using Necnat.Shared.Domains;

namespace Necnat.Shared.Filters
{
    public class MdHierarchicalStructureFilter : MdFilter<int>
	{
		public int? HierarchyIdFilter { get; set; }
		public int? ComponentTypeIdFilter { get; set; }
		public int? ComponentIdFilter { get; set; }
		public int? ParentHierarchicalStructureIdFilter { get; set; }
		public int ParentHierarchicalStructureIdFilterType { get; set; } = (int)EnFilterType.DISREGARD_NULL_EQUALS;

	}
}
