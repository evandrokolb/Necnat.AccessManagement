using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class HierarchicalStructure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ParentHierarchicalStructureId { get; set; }
        public HierarchicalStructure ParentHierarchicalStructure { get; set; }

        [Required]
        public int HierarchyId { get; set; }
        public Hierarchy Hierarchy { get; set; }

        public int ComponentTypeId { get; set; }

        public int ComponentId { get; set; }

        public ICollection<Security> SecurityList { get; set; }
    }
}
