using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class TokenUsage
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int TokenId { get; set; }
        [StringLength(100), DataMember]
        public string Location { get; set; }
        [StringLength(100), DataMember]
        public string Ip { get; set; }
        [StringLength(100), DataMember]
        public string Origin { get; set; }
        [StringLength(100), DataMember]
        public string Device { get; set; }
        [StringLength(100), DataMember]
        public string Agent { get; set; }
        [StringLength(100), DataMember]
        public string UserAgentFamily { get; set; }
        [StringLength(100), DataMember]
        public string UserAgentMajor { get; set; }
        [StringLength(100), DataMember]
        public string UserAgentMinor { get; set; }
        [StringLength(100), DataMember]
        public string UserAgentPatch { get; set; }
        [StringLength(100), DataMember]
        public string DeviceBrand { get; set; }
        [StringLength(100), DataMember]
        public string DeviceFamily { get; set; }
        [StringLength(100), DataMember]
        public string DeviceModel { get; set; }
        [StringLength(100), DataMember]
        public string OSFamily { get; set; }
        [StringLength(100), DataMember]
        public string OSMinor { get; set; }
        [StringLength(100), DataMember]
        public string OSMajor { get; set; }
        [StringLength(100), DataMember]
        public string OSPatch { get; set; }
        [StringLength(100), DataMember]
        public string OSPatchMinor { get; set; }
        [DataMember]
        public DateTime LastRequestTime { get; set; } = DateTime.Now;
        [DataMember]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        [DataMember]
        public virtual Token Token { get; set; }
    }
}
