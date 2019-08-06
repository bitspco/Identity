using Bitspco.Identity.Common.Models;
using System.Web.Http;
using Bitspco.Identity.Service.WebApi.Models;
using Bitspco.Identity.Common.Entities;
using Bitspco.Framework.Common;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Auth")]
    public class AuthController : ApiController
    {
        [Route(""), HttpGet]
        public OperationResult<LoginInfo> GetLoginInfo(string key)
        {
            return Controller.GetLoginInfo(key);
        }
        [Route(""), HttpPost]
        public OperationResult<LoginInfo> Login(VM_Login obj)
        {
            return Controller.Login(obj.Username, obj.Password);
        }
        [Route(""), HttpDelete]
        public OperationResult<Token> Logout(string key)
        {
            return Controller.Logout(key);
        }
    }
}
