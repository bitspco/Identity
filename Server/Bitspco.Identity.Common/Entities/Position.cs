using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class Position
    {
        private List<User> users;

        [DataMember]
        public int Id { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int? ParentId { get; set; }
        [DataMember]
        public virtual Position Parent { get; set; }

        public virtual List<User> Users { get { if (users == null) users = new List<User>(); return users; } set { users = value; } }
    }
}
