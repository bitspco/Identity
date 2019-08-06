using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Claims")]
    public class ClaimsController : ApiController
    {
        //---------------------------- Claim ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<Claim> Select() => Controller.GetAllClaims();
        [Route("{id:int}"), HttpGet]
        public Claim GetById(int id) => Controller.GetClaim(id);
        [Route(""), HttpPost]
        public OperationResult<Claim> Add(Claim obj) => Controller.AddClaim(obj);
        [Route(""), HttpPatch]
        public OperationResult<Claim> Change(Claim obj) => Controller.ChangeClaim(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<Claim> Remove(int id) => Controller.RemoveClaim(id);
        //---------------------------- ClaimValue ----------------------------//
        [Route("{id:int}/Users"), HttpGet]
        public IQueryable<UserClaim> GetAllUserClaim(int id) => Controller.GetAllUserClaimsByClaimId(id);
        [Route("{id:int}/Users"), HttpPost]
        public OperationResult<UserClaim> AddUserClaim(int id, [FromBody]UserClaim obj)
        {
            obj.ClaimId = id;
            return Controller.AddUserClaim(obj);
        }
        [Route("{id:int}/Users"), HttpPatch]
        public OperationResult<UserClaim> ChangeUserClaim(int id, [FromBody]UserClaim obj) => Controller.ChangeUserClaim(obj);
        [Route("{id:int}/Users/{userId:int}"), HttpDelete]
        public OperationResult<UserClaim> RemoveUserClaim(int id, int userId) => Controller.RemoveUserClaim(userId, id);
    }
}
