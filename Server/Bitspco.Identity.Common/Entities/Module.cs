using Bitspco.Identity.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class Module
    {
        private List<Role> roles;
        private List<Permission> permissions;
        private List<Claim> claims;

        [DataMember]
        public int Id { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [Required, StringLength(50), Index("UN_Permission", IsUnique = true)]
        [DataMember]
        public string Symbol { get; set; }
        [StringLength(50)]
        [DataMember]
        public string Icon { get; set; }
        [StringLength(150)]
        [DataMember]
        public string Link { get; set; }
        [StringLength(150)]
        [DataMember]
        public string Api { get; set; }
        [DataMember]
        public ModuleStatus Status { get; set; } = ModuleStatus.Active;


        public virtual List<Role> Roles { get { if (roles == null) roles = new List<Role>(); return roles; } set { roles = value; } }
        public virtual List<Permission> Permissions { get { if (permissions == null) permissions = new List<Permission>(); return permissions; } set { permissions = value; } }
        public virtual List<Claim> Claims { get { if (claims == null) claims = new List<Claim>(); return claims; } set { claims = value; } }
    }
}
