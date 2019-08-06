using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class UserClaim
    {
        [DataMember]
        public int Id { get; set; }
        [Index("UN_UserClaim", IsUnique = true, Order = 2)]
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [Index("UN_UserClaim", IsUnique = true, Order = 1)]
        [DataMember]
        public int ClaimId { get; set; }
        [DataMember]
        public virtual Claim Claim { get; set; }
        [Required, StringLength(300)]
        [DataMember]
        public string Value { get; set; }
    }
}
