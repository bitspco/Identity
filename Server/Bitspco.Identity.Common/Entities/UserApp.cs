using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class UserApp
    {
        [DataMember]
        public int Id { get; set; }
        [Index("UN_UserApp", IsUnique = true, Order = 1)]
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [Index("UN_UserApp", IsUnique = true, Order = 2)]
        [DataMember]
        public int AuthenticatorAppId { get; set; }
        [DataMember]
        public virtual AuthenticatorApp AuthenticatorApp { get; set; }
        [Required, StringLength(100)]
        [DataMember]
        public string SecureKey { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
        [DataMember]
        public bool IsVerify { get; set; }
    }
}
