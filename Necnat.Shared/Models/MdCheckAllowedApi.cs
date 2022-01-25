using System;
using System.Collections.Generic;
using System.Text;

namespace Necnat.Shared.Models
{
    public class MdCheckAllowedApi
    {
        public string ModuleCodeName { get; set; }
        public string ControllerName { get; set; }
        public string ApiName { get; set; }
        public int HttpMethodId { get; set; }
        public string ApiVersion { get; set; }
    }
}
