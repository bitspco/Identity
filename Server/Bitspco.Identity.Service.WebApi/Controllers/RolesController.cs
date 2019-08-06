using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Roles")]
    public class RolesController : ApiController
    {
        //---------------------------- Role ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<Role> Select() => Controller.GetAllRoles();
        [Route("{id:int}"), HttpGet]
        public Role GetById(int id) => Controller.GetRole(id);
        [Route(""), HttpPost]
        public OperationResult<Role> Add(Role obj) => Controller.AddRole(obj);
        [Route(""), HttpPatch]
        public OperationResult<Role> Change(Role obj) => Controller.ChangeRole(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<Role> Remove(int id) => Controller.RemoveRole(id);
        //---------------------------- User ----------------------------//
        [Route("{id:int}/Users"), HttpGet]
        public IQueryable<UserRole> GetAllUser(int id) => Controller.GetAllUserRolesByRoleId(id);
        [Route("{id:int}/Users"), HttpPost]
        public OperationResult<UserRole> AddUserRole(int id, UserRole obj)
        {
            obj.RoleId = id;
            return Controller.AddUserRoleByRoleId(obj);
        }
        [Route("{id:int}/Users/{userId:int}"), HttpDelete]
        public OperationResult<UserRole> RemoveUserRole(int id, int userId) => Controller.RemoveUserRoleByRoleId(id, userId);
        //---------------------------- Permission ----------------------------//
        [Route("{id:int}/Permissions"), HttpGet]
        public IQueryable<RolePermission> GetAllPermission(int id) => Controller.GetAllRolePermissionsByRoleId(id);
        [Route("{id:int}/Permissions"), HttpPost]
        public OperationResult<RolePermission> AddPermission(int id, [FromBody]RolePermission obj)
        {
            obj.RoleId = id;
            return Controller.AddRolePermissionByRoleId(obj);
        }
        [Route("{id:int}/Permissions/{permissionId:int}"), HttpDelete]
        public OperationResult<RolePermission> RemovePermission(int id, int permissionId) => Controller.RemoveRolePermission(id, permissionId);
        //---------------------------- Member ----------------------------//
        [Route("{id:int}/Members"), HttpGet]
        public IQueryable<RoleMember> GetAllMember(int id) => Controller.GetAllRoleMembersByRoleId(id);
        [Route("{id:int}/Members"), HttpPost]
        public OperationResult<RoleMember> AddMember(int id, [FromBody]RoleMember obj)
        {
            obj.BaseId = id;
            return Controller.AddRoleMember(obj);
        }
        [Route("{id:int}/Members/{memberId:int}"), HttpDelete]
        public OperationResult<RoleMember> RemoveMember(int id, int memberId) => Controller.RemoveRoleMember(id, memberId);
        //---------------------------- Parent ----------------------------//
        [Route("{id:int}/Parents"), HttpGet]
        public IQueryable<RoleMember> GetAllParent(int id) => Controller.GetAllRoleParentsByRoleId(id);
        [Route("{id:int}/Parents"), HttpPost]
        public OperationResult<RoleMember> AddParent(int id, [FromBody]RoleMember obj)
        {
            obj.MemberId = id;
            return Controller.AddRoleParent(obj);
        }
        [Route("{id:int}/Parents/{parentId:int}"), HttpDelete]
        public OperationResult<RoleMember> RemoveParent(int id, int parentId) => Controller.RemoveRoleParent(id, parentId);
    }
}
