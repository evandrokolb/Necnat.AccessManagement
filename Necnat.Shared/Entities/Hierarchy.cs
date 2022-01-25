using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class Hierarchy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<ApplicationHierarchy> ApplicationHierarchyList { get; set; }
        public ICollection<HierarchyHierarchyComponentType> HierarchyHierarchyComponentTypeList { get; set; }
    }
}
