using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class RolePermission
    {
        [DataMember]
        public int Id { get; set; }
        [Index("UN_RolePermission", IsUnique = true, Order = 1)]
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public virtual Role Role { get; set; }
        [Index("UN_RolePermission", IsUnique = true, Order = 2)]
        [DataMember]
        public int PermissionId { get; set; }
        [DataMember]
        public virtual Permission Permission { get; set; }

        public RolePermission() {}
        public RolePermission(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}
