using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class Security
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int? HierarchicalStructureId { get; set; }
        public HierarchicalStructure HierarchicalStructure { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
