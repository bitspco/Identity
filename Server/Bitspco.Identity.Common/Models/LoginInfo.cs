using Bitspco.Identity.Common.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Models
{
    [DataContract]
    public class LoginInfo
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Token Token { get; set; }
        [DataMember]
        public List<ModuleInfo> Modules { get; set; }
        [DataMember]
        public DateTime LastRequestTime { get; set; } = DateTime.Now;
        [DataMember]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        public bool IsValid()
        {
            if (Token.User.Timeout.HasValue && LastRequestTime < DateTime.Now.AddMinutes(-Token.User.Timeout.Value)) return false;
            if (Token.ExpireTime.HasValue) return false;
            if (Token.Status == Enums.TokenStatus.Expired) return false;
            return true;
        }
    }
}
