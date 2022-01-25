using Necnat.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Models
{
    public class MdViewHierarchyComponent
    {
        public int Id { get; set; }

        public int HierarchyComponentTypeId { get; set; }
        public HierarchyComponentType HierarchyComponentType { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool IsFristHierarchy { get; set; }
    }
}
