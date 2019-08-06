using Bitspco.Identity.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class UserContact
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [DataMember]
        public UserContactType Type { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool IsVerify { get; set; }
    }
}
