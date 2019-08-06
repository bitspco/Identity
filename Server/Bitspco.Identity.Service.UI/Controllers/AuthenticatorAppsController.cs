using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bitspco.Identity.Service.UI.Controllers
{
    public class AuthenticatorAppsController : Controller
    {
        // GET: AuthenticationApp
        public ActionResult Index()
        {
            return View();
        }
    }
}