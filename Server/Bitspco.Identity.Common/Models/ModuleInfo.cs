using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitspco.Identity.Common.Models
{
    public class ModuleInfo
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
        public Dictionary<string, string> Claims { get; set; }
    }
}
