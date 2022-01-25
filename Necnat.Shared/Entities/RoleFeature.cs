using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class RoleFeature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
