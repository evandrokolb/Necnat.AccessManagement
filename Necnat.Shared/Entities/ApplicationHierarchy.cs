using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class ApplicationHierarchy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        [Required]
        public int HierarchyId { get; set; }
        public Hierarchy Hierarchy { get; set; }
    }
}