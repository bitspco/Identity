using Bitspco.Framework.Filters.Security.Authenticate;
using Bitspco.Identity.Business;
using Bitspco.Identity.Common;
using Bitspco.Identity.Common.Enums;
using Bitspco.Identity.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bitspco.Identity.Facade.Filters
{
    public class Authenticator : IAuthenticator
    {
        private static Dictionary<string, LoginInfo> LoginInfos = new Dictionary<string, LoginInfo>();
        private static DateTime ClearCacheTime = DateTime.Now;

        private IdentityBusiness business;
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
                    if (loginInfo == null)
                    {
                        loginInfo = business.GetLoginInfo(Token);
                        if (loginInfo != null) LoginInfos[Token] = loginInfo;
                    }
                }
                if (loginInfo != null && !loginInfo.IsValid())
                {
                    if (!loginInfo.Token.ExpireTime.HasValue) business.Logout(loginInfo.Key);
                }
                return loginInfo;
            }
        }
        public Authenticator(IdentityBusiness business)
        {
            this.business = business;
        }
        public string GetToken()
        {
            return Token;
        }
        public bool HasPermission(string policy)
        {
            var module = GetModuleInfo();
            var policyExpression = new PolicyExperession(module.Roles, module.Permissions, module.Claims);
            return policyExpression.HasPolicy(policy);
        }
        public bool IsTokenValid()
        {
            var loginInfo = LoginInfo;
            return loginInfo != null && loginInfo.Token.Status == TokenStatus.Active;
        }
        public ModuleInfo GetModuleInfo()
        {
            return LoginInfo.Modules.FirstOrDefault(x => x.Symbol == Symbol);
        }
        public T GetClaim<T>(string symbol)
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
