using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class Api
    {
        public Api()
        {
            FeatureApiList = new HashSet<FeatureApi>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ControllerId { get; set; }
        public Controller Controller { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public int HttpMethodId { get; set; }

        [Required]
        [StringLength(54)]
        public string Version { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<FeatureApi> FeatureApiList { get; set; }
    }
}
