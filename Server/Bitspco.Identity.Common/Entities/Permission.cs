using Bitspco.Identity.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class Permission
    {
        private List<UserPermission> users;
        private List<RolePermission> roles;

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public virtual Module Module { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [Required, StringLength(50), Index("UN_Permission", IsUnique = true)]
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public PermissionStatus Status { get; set; } = PermissionStatus.Active;

        [ForeignKey("PermissionId")]
        public virtual List<UserPermission> Users { get { if (users == null) users = new List<UserPermission>(); return users; } set { users = value; } }
        [ForeignKey("PermissionId")]
        public virtual List<RolePermission> Roles { get { if (roles == null) roles = new List<RolePermission>(); return roles; } set { roles = value; } }
    }
}
