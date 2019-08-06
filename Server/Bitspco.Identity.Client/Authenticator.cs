using Bitspco.Framework.Filters.Security.Authenticate;
using Bitspco.Identity.Common;
using Bitspco.Identity.Common.Enums;
using Bitspco.Identity.Common.Interfaces;
using Bitspco.Identity.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitspco.Identity.Client
{
    public class Authenticator : IAuthenticator
    {
        private static Dictionary<string, LoginInfo> LoginInfos = new Dictionary<string, LoginInfo>();
        private static DateTime ClearCacheTime = DateTime.Now;

        private LoginInfo loginInfo;
        private IIdentityAuthClient client;
        private string token;

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
                    lock (LoginInfos) if (LoginInfos.ContainsKey(token)) loginInfo = LoginInfos[token];
                    if (loginInfo == null)
                    {
                        loginInfo = client.GetLoginInfo(token);
                        if (loginInfo != null) LoginInfos[token] = loginInfo;
                    }
                }
                if (loginInfo != null && !loginInfo.IsValid())
                {
                    if (!loginInfo.Token.ExpireTime.HasValue) client.Logout(loginInfo.Key);
                }
                return loginInfo;
            }
        }
        public Authenticator(string token, IIdentityAuthClient client)
        {
            this.token = token;
            this.client = client;
        }
        public virtual string GetToken()
        {
            return token;
        }
        public virtual bool HasPermission(string policy)
        {
            var module = GetModuleInfo();
            var policyExpression = new PolicyExperession(module.Roles, module.Permissions, module.Claims);
            return policyExpression.HasPolicy(policy);
        }
        public virtual bool IsTokenValid()
        {
            var loginInfo = LoginInfo;
            return loginInfo != null && loginInfo.Token.Status == TokenStatus.Active;
        }
        public virtual ModuleInfo GetModuleInfo()
        {
            return LoginInfo.Modules.FirstOrDefault(x => x.Symbol == Symbol);
        }
        public virtual T GetClaim<T>(string symbol)
        {
            try
            {
                var module = GetModuleInfo();
                if (module.Claims.ContainsKey(symbol)) return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(module.Claims[symbol]);
            }
            catch (Exception) { }
            return default(T);
        }
    }
}
