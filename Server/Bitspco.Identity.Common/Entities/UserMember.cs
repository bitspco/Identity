using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class UserMember
    {
        [DataMember]
        public int Id { get; set; }
        [Index("UN_UserMember", IsUnique = true, Order = 0)]
        [DataMember]
        public int BaseId { get; set; }
        [DataMember]
        public virtual User Base { get; set; }
        [Index("UN_UserMember", IsUnique = true, Order = 1)]
        [DataMember]
        public int MemberId { get; set; }
        [DataMember]
        public virtual User Member { get; set; }

        public UserMember() {}
        public UserMember(int baseId, int memberId)
        {
            BaseId = baseId;
            MemberId = memberId;
        }
    }
}
