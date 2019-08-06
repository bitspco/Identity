using Bitspco.Identity.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bitspco.Identity.Common.Entities
{
    [DataContract]
    public class Question
    {
        private List<UserQuestion> userSecurityQuestions;

        [DataMember]
        public int Id { get; set; }
        [Required, StringLength(300)]
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public QuestionStatus Status { get; set; } = QuestionStatus.Show;

        public virtual List<UserQuestion> UserSecurityQuestions { get { if (userSecurityQuestions == null) userSecurityQuestions = new List<UserQuestion>(); return userSecurityQuestions; } set { userSecurityQuestions = value; } }
    }
}
