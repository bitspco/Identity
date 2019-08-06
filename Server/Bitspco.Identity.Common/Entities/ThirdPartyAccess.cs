using Bitspco.Identity.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class ThirdPartyAccess
    {
        private List<ThirdPartyAppAccess> apps;

        [DataMember]
        public int Id { get; set; }
        [Index("UN_ThirdPartyAccess", IsUnique = true, Order = 1)]
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public virtual Module Module { get; set; }
        [Required, StringLength(100)]
        [DataMember]
        public string Name { get; set; }
        [Required, StringLength(50), Index("UN_ThirdPartyAccess", IsUnique = true, Order = 2)]
        [DataMember]
        public string Symbol { get; set; }
        [StringLength(300)]
        [DataMember]
        public string Description { get; set; }
        [StringLength(50)]
        [DataMember]
        public string Icon { get; set; }
        [DataMember]
        public ThirdPartyAccessStatus Status { get; set; }
        
        public virtual List<ThirdPartyAppAccess> Apps { get { if (apps == null) apps = new List<ThirdPartyAppAccess>(); return apps; } set { apps = value; } }
    }
}
