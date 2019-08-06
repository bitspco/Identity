using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("ThirdPartyApps")]
    public class ThirdPartyAppsController : ApiController
    {
        //---------------------------- Token ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<ThirdPartyApp> Select() => Controller.GetAllThirdPartyApps();
        [Route("{id:int}"), HttpGet]
        public ThirdPartyApp GetById(int id) => Controller.GetThirdPartyApp(id);
        [Route(""), HttpPost]
        public OperationResult<ThirdPartyApp> Add(ThirdPartyApp obj) => Controller.AddThirdPartyApp(obj);
        [Route(""), HttpPatch]
        public OperationResult<ThirdPartyApp> Change(ThirdPartyApp obj) => Controller.ChangeThirdPartyApp(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<ThirdPartyApp> Remove(int id) => Controller.RemoveThirdPartyApp(id);
        //---------------------------- Usage ----------------------------//
        [Route("{id:int}/Accesses"), HttpGet]
        public IQueryable<ThirdPartyAppAccess> GetAllAccess(int id) => Controller.GetAllThirdPartyAppAccessesByThirdPartyAppId(id);
    }
}
