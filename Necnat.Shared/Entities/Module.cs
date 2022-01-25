using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class Module
    {
        public Module()
        {
            ControllerList = new HashSet<Controller>();
            FeatureList = new HashSet<Feature>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string CodeName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Controller> ControllerList { get; set; }
        public ICollection<Feature> FeatureList { get; set; }
    }
}
