using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class RoleMember
    {
        [DataMember]
        public int Id { get; set; }
        [Index("UN_RoleMember", IsUnique = true, Order = 1)]
        [DataMember]
        public int BaseId { get; set; }
        [DataMember]
        public virtual Role Base { get; set; }
        [Index("UN_RoleMember", IsUnique = true, Order = 2)]
        [DataMember]
        public int MemberId { get; set; }
        [DataMember]
        public virtual Role Member { get; set; }

        public RoleMember() {}
        public RoleMember(int baseId, int memberId)
        {
            BaseId = baseId;
            MemberId = memberId;
        }
    }
}
