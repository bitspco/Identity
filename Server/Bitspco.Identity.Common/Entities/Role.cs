using Bitspco.Identity.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class Role
    {
        private List<UserRole> users;
        private List<RolePermission> permissions;
        private List<RoleMember> members;
        private List<RoleMember> parents;

        [DataMember]
        public int Id { get; set; }
        [Index("UN_Role", IsUnique = true, Order = 1)]
        [DataMember]
        public int ModuleId { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [Required, StringLength(50), Index("UN_Role", IsUnique = true, Order = 2)]
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public RoleStatus Status { get; set; } = RoleStatus.Active;


        [DataMember]
        public virtual Module Module { get; set; }

        [ForeignKey("RoleId")]
        public virtual List<UserRole> Users { get { if (users == null) users = new List<UserRole>(); return users; } set { users = value; } }
        [ForeignKey("RoleId")]
        public virtual List<RolePermission> Permissions { get { if (permissions == null) permissions = new List<RolePermission>(); return permissions; } set { permissions = value; } }
        [ForeignKey("BaseId")]
        public virtual List<RoleMember> Members { get { if (members == null) members = new List<RoleMember>(); return members; } set { members = value; } }
        [ForeignKey("MemberId")]
        public virtual List<RoleMember> Parents { get { if (parents == null) parents = new List<RoleMember>(); return parents; } set { parents = value; } }
    }
}
