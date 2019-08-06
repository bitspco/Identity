using Bitspco.Identity.Facade;
using System;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        private IdentityController controller;
        public IdentityController Controller
        {
            get
            {
                if (controller == null)
                {
                    controller = new IdentityController();
                    try
                    {
                        var token = "";
                        var queryString = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                        if (queryString.ContainsKey("api_key")) token = queryString["api_key"];
                        else if (Request.Headers.Contains("Authorization"))
                        {
                            token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                            if (token.ToLower().IndexOf("bearer") == 0) token = token.Substring(7);
                        }
                        if (!string.IsNullOrEmpty(token)) controller.RegisterAuthenticator(token);
                    }
                    catch (Exception) { }
                    try
                    {
                        var context = new HttpContextWrapper(HttpContext.Current);
                        HttpRequestBase request = context.Request;
                        controller.RegisterSecurity(request);
                    }
                    catch (Exception) { }
                }
                return controller;
            }
        }
    }
}
