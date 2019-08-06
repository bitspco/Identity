using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class UserQuestion
    {
        [DataMember]
        public int Id { get; set; }
        [Index("UN_UserQuestion", IsUnique = true, Order = 0)]
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [Index("UN_UserQuestion", IsUnique = true, Order = 1)]
        [DataMember]
        public int QuestionId { get; set; }
        [DataMember]
        public virtual Question Question { get; set; }
        [Required, StringLength(100)]
        [DataMember]
        public string Answer { get; set; }
    }
}
