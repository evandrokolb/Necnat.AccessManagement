using Necnat.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Models
{
    public class MdHierarchicalStructureTree
    {
        public MdHierarchicalStructureTree()
        {
            HierarchicalStructureTreeAllowedList = new List<MdHierarchicalStructureTreeAllowed>();
        }

        public HierarchicalStructure HierarchicalStructure { get; set; }
        public MdViewHierarchyComponent HierarchyComponent { get; set; }
        public List<MdHierarchicalStructureTreeAllowed> HierarchicalStructureTreeAllowedList { get; set; }
    }
}
