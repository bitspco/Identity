using Bitspco.Identity.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class Claim
    {
        private List<UserClaim> values;

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ModuleId { get; set; }
        [Index("UN_Claim", IsUnique = true, Order = 1)]
        [DataMember]
        public virtual Module Module { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [Required, StringLength(50), Index("UN_Claim", IsUnique = true, Order = 2)]
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public ClaimType Type { get; set; }
        [StringLength(500)]
        [DataMember]
        public string Template { get; set; }
        [DataMember]
        public ClaimStatus Status { get; set; } = ClaimStatus.Active;

        public virtual List<UserClaim> Values { get { if (values == null) values = new List<UserClaim>(); return values; } set { values = value; } }
    }
}
