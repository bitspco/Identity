using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Modules")]
    public class ModulesController : ApiController
    {
        //---------------------------- Module ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<Module> Select() => Controller.GetAllModules();
        [Route("{id:int}"), HttpGet]
        public Module GetById(int id) => Controller.GetModule(id);
        [Route(""), HttpPost]
        public OperationResult<Module> Add(Module obj) => Controller.AddModule(obj);
        [Route(""), HttpPatch]
        public OperationResult<Module> Change(Module obj) => Controller.ChangeModule(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<Module> Remove(int id) => Controller.RemoveModule(id);
        //---------------------------- Role ----------------------------//
        [Route("{id:int}/Roles"), HttpGet]
        public IQueryable<Role> GetAllRole(int id) => Controller.GetAllRolesByModuleId(id);
        //---------------------------- Permission ----------------------------//
        [Route("{id:int}/Permissions"), HttpGet]
        public IQueryable<Permission> GetAllPermission(int id) => Controller.GetAllPermissionsByModuleId(id);
        //---------------------------- Claim ----------------------------//
        [Route("{id:int}/Claims"), HttpGet]
        public IQueryable<Claim> GetAllClaim(int id) => Controller.GetAllClaimsByModuleId(id);
        //---------------------------- Claim ----------------------------//
        [Route("{id:int}/ThirdPartyAccesses"), HttpGet]
        public IQueryable<ThirdPartyAccess> GetAllThirdPartyAccesses(int id) => Controller.GetAllThirdPartyAccesses(id);
    }
}
