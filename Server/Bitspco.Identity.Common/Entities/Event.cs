using Bitspco.Identity.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class Event
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [Required, StringLength(300)]
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime Time { get; set; } = DateTime.Now;
        [StringLength(500)]
        [DataMember]
        public string JsonInfo { get; set; }
        [DataMember]
        public EventType Type { get; set; } = EventType.None;
        [DataMember]
        public EventLevel Level { get; set; } = EventLevel.None;
        [DataMember]
        public EventStatus Status { get; set; } = EventStatus.NotSeen;
    }
}
