using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Models
{
    public class MdAllowedModuleFeature
    {
        public int ModuleId { get; set; }
        public string ModuleCodeName { get; set; }
        public int FeatureId { get; set; }
        public string FeatureCodeName { get; set; }
        public List<MdHierarchyComponent> HierarchyComponentList { get; set; }
    }
}
