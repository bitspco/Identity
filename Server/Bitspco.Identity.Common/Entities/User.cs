using Bitspco.Identity.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class User
    {
        private List<Token> tokens;
        private List<Event> events;
        private List<UserApp> apps;
        private List<UserRole> roles;
        private List<UserClaim> claims;
        private List<UserMember> parents;
        private List<UserMember> members;
        private List<UserContact> contacts;
        private List<UserQuestion> questions;
        private List<UserPermission> permissions;

        [DataMember]
        public int Id { get; set; }
        [Required, StringLength(75)]
        [DataMember]
        public string Name { get; set; }
        [Required, StringLength(75)]
        public string SecretKey { get; set; }
        [Required, StringLength(32)]
        [DataMember]
        public string Username { get; set; }
        [Required, StringLength(75)]
        [DataMember]
        public string Password { get; set; }
        [StringLength(200)]
        [DataMember]
        public string Image { get; set; }
        [StringLength(10)]
        [DataMember]
        public string NationalId { get; set; }
        [DataMember]
        public DateTime? Birthday { get; set; }
        [DataMember]
        public Gender? Gender { get; set; }
        [DataMember]
        public int? PositionId { get; set; }
        [StringLength(300)]
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime? LastPasswordChange { get; set; }
        [DataMember]
        public int? Timeout { get; set; }
        [DataMember]
        public int MaxTokenCount { get; set; } = 1;
        [DataMember]
        public bool IsNeedChangePassword { get; set; } = true;
        [DataMember]
        public bool IsAppAuthenticatorEnable { get; set; } = false;
        [DataMember]
        public bool IsContactAuthenticatorEnable { get; set; } = false;
        [DataMember]
        public UserStatus Status { get; set; } = UserStatus.Active;


        [DataMember]
        public virtual Position Position { get; set; }

        [ForeignKey("BaseId")]
        public virtual List<UserMember> Members { get { if (members == null) members = new List<UserMember>(); return members; } set { members = value; } }
        [ForeignKey("MemberId")]
        public virtual List<UserMember> Parents { get { if (parents == null) parents = new List<UserMember>(); return parents; } set { parents = value; } }
        public virtual List<UserRole> Roles { get { if (roles == null) roles = new List<UserRole>(); return roles; } set { roles = value; } }
        public virtual List<UserPermission> Permissions { get { if (permissions == null) permissions = new List<UserPermission>(); return permissions; } set { permissions = value; } }
        public virtual List<UserClaim> Claims { get { if (claims == null) claims = new List<UserClaim>(); return claims; } set { claims = value; } }
        public virtual List<Token> Tokens { get { if (tokens == null) tokens = new List<Token>(); return tokens; } set { tokens = value; } }
        public virtual List<UserContact> Contacts { get { if (contacts == null) contacts = new List<UserContact>(); return contacts; } set { contacts = value; } }
        public virtual List<UserApp> Apps { get { if (apps == null) apps = new List<UserApp>(); return apps; } set { apps = value; } }
        public virtual List<UserQuestion> Questions { get { if (questions == null) questions = new List<UserQuestion>(); return questions; } set { questions = value; } }
        public virtual List<Event> Events { get { if (events == null) events = new List<Event>(); return events; } set { events = value; } }
    }
}
