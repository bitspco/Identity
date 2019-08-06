using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class UserRole
    {
        [DataMember]
        public int Id { get; set; }
        [Index("UN_UserRole", IsUnique = true, Order = 0)]
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [Index("UN_UserRole", IsUnique = true, Order = 1)]
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public virtual Role Role { get; set; }

        public UserRole() {}
        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
