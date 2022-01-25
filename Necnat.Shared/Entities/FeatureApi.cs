using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class FeatureApi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }

        [Required]
        public int ApiId { get; set; }
        public Api Api { get; set; }
    }
}
