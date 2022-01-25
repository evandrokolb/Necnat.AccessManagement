using System.Collections.Generic;

namespace Necnat.Shared.Models
{
    public class MdSearchTree
    {
        public MdSearchTree()
        {
            SearchTreeAllowedList = new List<MdSearchTreeAllowed>();
        }

        public int HierarchyId { get; set; }
        public List<MdSearchTreeAllowed> SearchTreeAllowedList { get; set; }
    }
}
