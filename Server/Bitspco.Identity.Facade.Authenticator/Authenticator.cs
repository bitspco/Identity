using Sigma.Facade.Filters.Security.Authenticate;
using Sigma.Identity.Common;
using Sigma.Identity.Common.Enums;
using Sigma.Identity.Common.Interfaces;
using Sigma.Identity.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma.Identity.Facade.Authenticator
{
    public class Authenticator : IAuthenticator
    {
        private static Dictionary<string, LoginInfo> LoginInfos = new Dictionary<string, LoginInfo>();
        private static DateTime ClearCacheTime = DateTime.Now;

        private IIdentityClient client;
        private LoginInfo loginInfo;

        public string Token { get; set; }
        public string Symbol { get; set; }
        public LoginInfo LoginInfo
        {
            get
            {
                if (loginInfo == null)
                {
                    lock (LoginInfos)
                    {
                        if (ClearCacheTime < DateTime.Now.AddMinutes(-1))
                        {
                            LoginInfos = LoginInfos.Where(x => x.Value.CreationTime > DateTime.Now.AddMinutes(-1)).ToDictionary(x => x.Key, x => x.Value);
                            ClearCacheTime = DateTime.Now;
                        }
                        if (LoginInfos.Count > 500) LoginInfos = LoginInfos.Skip(100).Take(400).ToDictionary(x => x.Key, x => x.Value);
                    }
                    lock (LoginInfos) if (LoginInfos.ContainsKey(Token)) loginInfo = LoginInfos[Token];
                    if (loginInfo == null) LoginInfos[Token] = loginInfo = client.GetLoginInfo(Token);
                }
                if (!loginInfo.IsValid())
                {
                    if (!loginInfo.Token.ExpireTime.HasValue) client.Logout(loginInfo.Key);
                }
                return loginInfo;
            }
        }
        public Authenticator(IIdentityClient client, string token, string symbol)
        {
            this.client = client;
            this.Token = token;
            this.Symbol = symbol;
        }
        public string GetToken()
        {
            return Token;
        }
        public bool HasPermission(string policy)
        {
            var module = LoginInfo.Modules.FirstOrDefault(x => x.Symbol == Symbol);
            if (module == null) throw new Exception("Module Not Found");
            var policyExpression = new PolicyExperession(module.Roles, module.Permissions, module.Claims);
            return policyExpression.HasPolicy(policy);
        }
        public bool IsTokenValid()
        {
            var loginInfo = LoginInfo;
            return loginInfo != null && loginInfo.Token.Status == TokenStatus.Active;
        }
    }
}
