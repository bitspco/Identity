using Bitspco.Identity.Common.Entities;
using Bitspco.Identity.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitspco.Identity.Common.Interfaces
{
    public interface IIdentityAuthClient
    {
        //===================================================
        LoginInfo GetLoginInfo(string key);
        LoginInfo Login(string username, string password);
        Token Logout(string key);
    }
}
