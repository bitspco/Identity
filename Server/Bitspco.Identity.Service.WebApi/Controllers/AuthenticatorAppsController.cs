using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using Bitspco.Identity.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("AuthenticatorApps")]
    public class AuthenticatorAppsController : ApiController
    {
        //---------------------------- AuthenticatorApp ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<AuthenticatorApp> Select() => Controller.GetAllAuthenticatorApps();
        [Route("{id:int}"), HttpGet]
        public AuthenticatorApp GetById(int id) => Controller.GetAuthenticatorApp(id);
        [Route(""), HttpPost]
        public OperationResult<AuthenticatorApp> Add(AuthenticatorApp obj) => Controller.AddAuthenticatorApp(obj);
        [Route(""), HttpPatch]
        public OperationResult<AuthenticatorApp> Change(AuthenticatorApp obj) => Controller.ChangeAuthenticatorApp(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<AuthenticatorApp> Remove(int id) => Controller.RemoveAuthenticatorApp(id);
        [Route("Statuses"), HttpGet]
        public IEnumerable<KeyValuePair<byte, string>> GetStatuses()
        {
            foreach (AuthenticatorAppStatus item in Enum.GetValues(typeof(AuthenticatorAppStatus)))
                yield return new KeyValuePair<byte, string>((byte)item, item.GetDescription());
        }
        //---------------------------- User ----------------------------//
        [Route("{id:int}/Users")]
        public IQueryable<UserApp> GetUsers(int id) => Controller.GetAllUserAppsByAuthenticatorAppId(id);
    }
}
