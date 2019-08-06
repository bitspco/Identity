using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Questions")]
    public class QuestionsController : ApiController
    {
        //---------------------------- Question ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<Question> Select() => Controller.GetAllQuestions();
        [Route("{id:int}"), HttpGet]
        public Question GetById(int id) => Controller.GetQuestion(id);
        [Route(""), HttpPost]
        public OperationResult<Question> Add(Question obj) => Controller.AddQuestion(obj);
        [Route(""), HttpPatch]
        public OperationResult<Question> Change(Question obj) => Controller.ChangeQuestion(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<Question> Remove(int id) => Controller.RemoveQuestion(id);
        //---------------------------- UserSecurityQuestion ----------------------------//
        [Route("{id:int}/Users"), HttpGet]
        public IQueryable<UserQuestion> GetAllUserSecurityQuestion(int id) => Controller.GetAllUserQuestionsByQuestionId(id);
        [Route("{id:int}/Users"), HttpPost]
        public OperationResult<UserQuestion> AddUserSecurityQuestion(int id, [FromBody]UserQuestion obj)
        {
            obj.QuestionId = id;
            return Controller.AddUserQuestion(obj);
        }
        [Route("{id:int}/Users/{userId:int}"), HttpDelete]
        public OperationResult<UserQuestion> RemoveUserSecurityQuestion(int id, int userId) => Controller.RemoveUserQuestion(id, userId);
    }
}
