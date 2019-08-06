using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Permissions")]
    public class PermissionsController : ApiController
    {
        //---------------------------- Permission ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<Permission> Select() => Controller.GetAllPermissions();
        [Route("{id:int}"), HttpGet]
        public Permission GetById(int id) => Controller.GetPermission(id);
        [Route(""), HttpPost]
        public OperationResult<Permission> Add(Permission obj) => Controller.AddPermission(obj);
        [Route(""), HttpPatch]
        public OperationResult<Permission> Change(Permission obj) => Controller.ChangePermission(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<Permission> Remove(int id) => Controller.RemovePermission(id);
        //---------------------------- Role ----------------------------//
        [Route("{id:int}/Roles"), HttpGet]
        public IQueryable<RolePermission> GetAllRole(int id) => Controller.GetAllRolePermissionsByPermissionId(id);
        [Route("{id:int}/Roles"), HttpPost]
        public OperationResult<RolePermission> AddRolePermission(int id, RolePermission obj)
        {
            obj.PermissionId = id;
            return Controller.AddRolePermissionByPermissionId(obj);
        }
        [Route("{id:int}/Roles/{roleId:int}"), HttpDelete]
        public OperationResult<RolePermission> RemoveRolePermission(int id, int roleId) => Controller.RemoveRolePermissionByPermissionId(id, roleId);
        //---------------------------- User ----------------------------//
        [Route("{id:int}/Users"), HttpGet]
        public IQueryable<UserPermission> GetAllUser(int id) => Controller.GetAllUserPermissionsByPermissionId(id);
        [Route("{id:int}/Users"), HttpPost]
        public OperationResult<UserPermission> AddUserPermission(int id, UserPermission obj)
        {
            obj.PermissionId = id;
            return Controller.AddUserPermissionByPermissionId(obj);
        }
        [Route("{id:int}/Users/{userId:int}"), HttpDelete]
        public OperationResult<UserPermission> RemoveUserRoleByPermissionId(int id, int userId) => Controller.RemoveUserRoleByPermissionId(id, userId);
    }
}
