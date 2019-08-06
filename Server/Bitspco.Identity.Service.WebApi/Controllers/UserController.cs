using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Users")]
    public class UsersController : ApiController
    {
        //---------------------------- User ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<User> Select() => Controller.GetAllUsers();
        [Route("{id:int}"), HttpGet]
        public User GetById(int id) => Controller.GetUser(id);
        [Route(""), HttpPost]
        public OperationResult<User> Add(User obj) => Controller.AddUser(obj);
        [Route(""), HttpPatch]
        public OperationResult<User> Change(User obj) => Controller.ChangeUser(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<User> Remove(int id) => Controller.RemoveUser(id);
        //---------------------------- Role ----------------------------//
        [Route("{id:int}/Roles"), HttpGet]
        public IQueryable<UserRole> GetAllRole(int id) => Controller.GetAllUserRolesByUserId(id);
        [Route("{id:int}/Roles"), HttpPost]
        public OperationResult<UserRole> AddRole(int id, [FromBody]UserRole obj)
        {
            obj.UserId = id;
            return Controller.AddUserRoleByUserId(obj);
        }
        [Route("{id:int}/Roles/{roleId:int}"), HttpDelete]
        public OperationResult<UserRole> RemoveRole(int id, int roleId) => Controller.RemoveUserRole(id, roleId);
        //---------------------------- Permission ----------------------------//
        [Route("{id:int}/Permissions"), HttpGet]
        public IQueryable<UserPermission> GetAllPermission(int id) => Controller.GetAllUserPermissionsByUserId(id);
        [Route("{id:int}/Permissions"), HttpPost]
        public OperationResult<UserPermission> AddPermission(int id, [FromBody]UserPermission obj)
        {
            obj.UserId = id;
            return Controller.AddUserPermissionByUserId(obj);
        }
        [Route("{id:int}/Permissions/{permissionId:int}"), HttpDelete]
        public OperationResult<UserPermission> RemovePermission(int id, int permissionId) => Controller.RemoveUserPermission(id, permissionId);
        //---------------------------- Claim ----------------------------//
        [Route("{id:int}/Claims"), HttpGet]
        public IQueryable<UserClaim> GetAllClaim(int id) => Controller.GetAllUserClaimsByUserId(id);
        [Route("{id:int}/Claims"), HttpPost]
        public OperationResult<UserClaim> AddClaim(int id, [FromBody]UserClaim obj)
        {
            obj.UserId = id;
            return Controller.AddUserClaim(obj);
        }
        [Route("{id:int}/Claims/{claimId:int}"), HttpDelete]
        public OperationResult<UserClaim> RemoveClaim(int id, int claimId) => Controller.RemoveUserClaim(id, claimId);
        //---------------------------- Token ----------------------------//
        [Route("{id:int}/Tokens"), HttpGet]
        public IQueryable<Token> GetAllToken(int id) => Controller.GetAllTokensByUserId(id);
        [Route("{id:int}/Tokens"), HttpPost]
        public OperationResult<Token> AddToken(int id) => Controller.AddTokenByUserId(id);
        [Route("{id:int}/Tokens"), HttpDelete]
        public OperationResult<Token> RemoveToken(int id, string key) => Controller.RemoveTokenByUserId(id, key);
        //---------------------------- Member ----------------------------//
        [Route("{id:int}/Members"), HttpGet]
        public IQueryable<UserMember> GetAllMember(int id) => Controller.GetAllUserMembersByUserId(id);
        [Route("{id:int}/Members"), HttpPost]
        public OperationResult<UserMember> AddMember(int id, [FromBody]UserMember obj)
        {
            obj.BaseId = id;
            return Controller.AddUserMember(obj);
        }
        [Route("{id:int}/Members/{memberId:int}"), HttpDelete]
        public OperationResult<UserMember> RemoveMember(int id, int memberId) => Controller.RemoveUserMember(id, memberId);
        //---------------------------- Parent ----------------------------//
        [Route("{id:int}/Parents"), HttpGet]
        public IQueryable<UserMember> GetAllParent(int id) => Controller.GetAllUserParentsByUserId(id);
        [Route("{id:int}/Parents"), HttpPost]
        public OperationResult<UserMember> AddParent(int id, [FromBody]UserMember obj)
        {
            obj.MemberId = id;
            return Controller.AddUserParent(obj);
        }
        [Route("{id:int}/Parents/{parentId:int}"), HttpDelete]
        public OperationResult<UserMember> RemoveParent(int id, int parentId) => Controller.RemoveUserParent(id, parentId);
        //---------------------------- Contact ----------------------------//
        [Route("{id:int}/Contacts"), HttpGet]
        public IQueryable<UserContact> GetAllContact(int id) => Controller.GetAllContactsByUserId(id);
        [Route("{id:int}/Contacts"), HttpPost]
        public OperationResult<UserContact> AddContact(int id, [FromBody]UserContact obj)
        {
            obj.UserId = id;
            return Controller.AddContact(obj);
        }
        [Route("{id:int}/Contacts/{contactId:int}/Approve"), HttpPut]
        public OperationResult<UserContact> ApproveContact(int id, int contactId) => Controller.ApproveContact(id, contactId);
        [Route("{id:int}/Contacts/{contactId:int}/Reject"), HttpPut]
        public OperationResult<UserContact> RejectContact(int id, int contactId) => Controller.RejectContact(id, contactId);
        [Route("{id:int}/Contacts/{contactId:int}"), HttpDelete]
        public OperationResult<UserContact> RemoveContact(int id, int contactId) => Controller.RemoveContact(id, contactId);
        //---------------------------- Apps ----------------------------//
        [Route("{id:int}/Apps"), HttpGet]
        public IQueryable<UserApp> GetAllApps(int id) => Controller.GetAllApps(id);
        [Route("{id:int}/Apps"), HttpPost]
        public OperationResult<UserApp> AddApp(int id, [FromBody]UserApp obj)
        {
            obj.UserId = id;
            return Controller.AddApp(obj);
        }
        [Route("{id:int}/Apps/{appId:int}"), HttpDelete]
        public OperationResult<UserApp> RemoveApp(int id, int appId) => Controller.RemoveApp(id, appId);
        //---------------------------- Events ----------------------------//
        [Route("{id:int}/Events"), HttpGet]
        public IQueryable<Event> GetAllEvents(int id) => Controller.GetAllEvents(id);
        [Route("{id:int}/Events"), HttpPost]
        public OperationResult<Event> AddEvent(int id, [FromBody]Event obj)
        {
            obj.UserId = id;
            return Controller.AddEvent(obj);
        }
        [Route("{id:int}/Events/{eventId:int}"), HttpDelete]
        public OperationResult<Event> RemoveEvent(int id, int eventId) => Controller.RemoveEvent(id, eventId);
        //---------------------------- Questions ----------------------------//
        [Route("{id:int}/Questions"), HttpGet]
        public IQueryable<UserQuestion> GetAllQuestions(int id) => Controller.GetAllQuestions(id);
        [Route("{id:int}/Questions"), HttpPost]
        public OperationResult<UserQuestion> AddQuestion(int id, [FromBody]UserQuestion obj)
        {
            obj.UserId = id;
            return Controller.AddUserQuestion(obj);
        }
        [Route("{id:int}/Questions/{questionId:int}"), HttpDelete]
        public OperationResult<UserQuestion> RemoveQuestion(int id, int questionId) => Controller.RemoveUserQuestion(id, questionId);
    }
}
