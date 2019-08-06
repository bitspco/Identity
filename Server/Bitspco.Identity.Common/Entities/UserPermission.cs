using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class UserPermission
    {
        [DataMember]
        public int Id { get; set; }
        [Index("UN_UserPermission", IsUnique = true, Order = 0)]
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [Index("UN_UserPermission", IsUnique = true, Order = 1)]
        [DataMember]
        public int PermissionId { get; set; }
        [DataMember]
        public virtual Permission Permission { get; set; }

        public UserPermission()
        {

        }
        public UserPermission(int userId, int permissionId)
        {
            UserId = userId;
            PermissionId = permissionId;
        }
    }
}
