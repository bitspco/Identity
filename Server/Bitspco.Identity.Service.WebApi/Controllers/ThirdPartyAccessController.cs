using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("ThirdPartyAccesses")]
    public class ThirdPartyAccessesController : ApiController
    {
        //---------------------------- Token ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<ThirdPartyAccess> Select() => Controller.GetAllThirdPartyAccesses();
        [Route("{id:int}"), HttpGet]
        public ThirdPartyAccess GetById(int id) => Controller.GetThirdPartyAccess(id);
        [Route(""), HttpPost]
        public OperationResult<ThirdPartyAccess> Add(ThirdPartyAccess obj) => Controller.AddThirdPartyAccess(obj);
        [Route(""), HttpPatch]
        public OperationResult<ThirdPartyAccess> Change(ThirdPartyAccess obj) => Controller.ChangeThirdPartyAccess(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<ThirdPartyAccess> Remove(int id) => Controller.RemoveThirdPartyAccess(id);
        //---------------------------- Usage ----------------------------//
        [Route("{id:int}/Apps"), HttpGet]
        public IQueryable<ThirdPartyAppAccess> GetAllApp(int id) => Controller.GetAllThirdPartyAppAccessesByThirdPartyAccessId(id);
    }
}
