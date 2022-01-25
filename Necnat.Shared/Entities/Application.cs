using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Necnat.Shared.Entities
{
    public partial class Application
    {
        public Application()
        {
            ModuleList = new HashSet<Module>();
            RoleList = new HashSet<Role>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Acronym { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Module> ModuleList { get; set; }
        public ICollection<Role> RoleList { get; set; }
        public ICollection<ApplicationHierarchy> ApplicationHierarchyList { get; set; }
    }
}
