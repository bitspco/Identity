using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class ThirdPartyAppAccess
    {
        [DataMember]
        public int Id { get; set; }
        [Index("UN_ThirdPartyAppAccess", IsUnique = true, Order = 0)]
        [DataMember]
        public int ThirdPartyAppId { get; set; }
        [DataMember]
        public virtual ThirdPartyApp ThirdPartyApp { get; set; }
        [Index("UN_ThirdPartyAppAccess", IsUnique = true, Order = 1)]
        [DataMember]
        public int ThirdPartyAccessId { get; set; }
        [DataMember]
        public virtual ThirdPartyAccess ThirdPartyAccess { get; set; }
        [Index("UN_ThirdPartyAppAccess", IsUnique = true, Order = 2)]
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
    }
}
