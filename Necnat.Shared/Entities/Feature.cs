using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class Feature
    {
        public Feature()
        {
            FeatureApiList = new HashSet<FeatureApi>();
            RoleFeatureList = new HashSet<RoleFeature>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string CodeName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<FeatureApi> FeatureApiList { get; set; }
        public ICollection<RoleFeature> RoleFeatureList { get; set; }
    }
}
