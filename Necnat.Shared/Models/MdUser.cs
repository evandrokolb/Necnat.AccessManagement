using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Necnat.Shared.Models
{
    public class MdUser
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
