using Bitspco.Identity.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class AuthenticatorApp
    {
        private List<UserApp> users;

        [DataMember]
        public int Id { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [StringLength(50)]
        [DataMember]
        public string Icon { get; set; }
        [DataMember]
        public AuthenticatorAppStatus Status { get; set; } = AuthenticatorAppStatus.Allow;

        public virtual List<UserApp> Users { get { if (users == null) users = new List<UserApp>(); return users; } set { users = value; } }
    }
}
