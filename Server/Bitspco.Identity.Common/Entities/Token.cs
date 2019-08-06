using Bitspco.Identity.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class Token
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [Required, StringLength(100), Index("UN_Token", IsUnique = true, Order = 1)]
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public DateTime CreationTime { get; set; } = DateTime.Now;
        [DataMember]
        public DateTime? ExpireTime { get; set; }
        [StringLength(300)]
        [DataMember]
        public string ExpireDescription { get; set; }
        public int? VerficationCode { get; set; }
        [DataMember]
        public DateTime? VerficationTime { get; set; }
        [DataMember]
        public bool IsNeedVerfication { get; set; }
        [DataMember]
        public TokenStatus Status { get; set; } = TokenStatus.Active;
        
        public virtual List<TokenUsage> Usages { get; set; }
    }
}
