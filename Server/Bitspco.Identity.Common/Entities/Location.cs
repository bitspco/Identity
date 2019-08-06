using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bitspco.Identity.Common.Entities
{
    [Table("Location"), DataContract]
    public class Location
    {
        private List<Location> children;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int Id { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Code { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int? ParentId { get; set; }

        public virtual Location Parent { get; set; }
        public List<Location> Children { get { if (children == null) children = new List<Location>(); return children; } set { children = value; } }
    }
}
