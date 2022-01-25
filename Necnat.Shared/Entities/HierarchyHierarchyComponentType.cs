using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public class HierarchyHierarchyComponentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int HierarchyId { get; set; }
        public Hierarchy Hierarchy { get; set; }

        [Required]
        public int HierarchyComponentTypeId { get; set; }
        public HierarchyComponentType HierarchyComponentType { get; set; }
    }
}
