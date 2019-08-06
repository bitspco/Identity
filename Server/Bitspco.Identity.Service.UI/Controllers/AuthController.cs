using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bitspco.Identity.Service.UI.Controllers
{
    public class AuthController : System.Web.Mvc.Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Lock()
        {
            return View();
        }
    }
}