using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class Controller
    {
        public Controller()
        {
            ApiList = new HashSet<Api>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Api> ApiList { get; set; }
    }
}
