using Bitspco.Identity.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class ThirdPartyApp
    {
        private List<ThirdPartyAppAccess> accesses;

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
        [Required, StringLength(50)]
        [DataMember]
        public string Name { get; set; }
        [StringLength(50)]
        [DataMember]
        public string Icon { get; set; }
        [StringLength(200)]
        [DataMember]
        public string HomePage { get; set; }
        [StringLength(200)]
        [DataMember]
        public string AccessGivenTo { get; set; }
        [DataMember]
        public DateTime AccessGivenTime { get; set; } = DateTime.Now;
        [DataMember]
        public ThirdPartyAppStatus Status { get; set; } = ThirdPartyAppStatus.Active;
        
        public virtual List<ThirdPartyAppAccess> Accesses { get { if (accesses == null) accesses = new List<ThirdPartyAppAccess>(); return accesses; } set { accesses = value; } }
    }
}
