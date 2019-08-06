using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bitspco.Identity.Common.Entities
{
    [Table("ISP"), DataContract]
    public class ISP
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int LocationId { get; set; }
        [DataMember]
        public Location Location { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
