using Bitspco.Identity.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Bitspco.Identity.Common.Entities;
using Bitspco.Identity.Common.Models;
using Bitspco.Framework.Common;
using Bitspco.Framework.Http;

namespace Bitspco.Identity.Client
{
    public class IdentityClientVersion1WebApi : IIdentityClientVersion1
    {
        private string baseUrl;
        private string token;
        public IdentityClientVersion1WebApi(string baseUrl, string token = null)
        {
            this.baseUrl = baseUrl + (baseUrl.Last() == '/' ? "" : "/");
            this.token = token;
        }
        private HttpClient GetHttpClient(string url)
        {
            var http = new HttpClient(baseUrl + url);
            if(token != null) http.AddHeader("Authorization", token);
            return http;
        }
        private T CheckResult<T>(OperationResult<T> op)
        {
            if (op.Success) return op.Data;
            return default(T);
        }
        private string GetName<T>()
        {
            var name = typeof(T).Name;
            if (name.Last() == 'y') return name.Substring(0, name.Length - 1) + "ies";
            return name + "s";
        }
        public IdentityClientVersion1WebApi SetToken(string token) { this.token = token; return this; }

        public List<T> GetAll<T>() => GetHttpClient(GetName<T>()).Get<List<T>>();
        public T GetById<T>(int id) => GetHttpClient(GetName<T>() + $"/{id}").Get<T>();
        public T Add<T>(T obj) => CheckResult(GetHttpClient(GetName<T>()).Post<OperationResult<T>>());
        public T Change<T>(T obj) => CheckResult(GetHttpClient(GetName<T>()).Patch<OperationResult<T>>());
        public T Remove<T>(int id) => CheckResult(GetHttpClient(GetName<T>() + $"/{id}").Delete<OperationResult<T>>());
        //====================================================================================
        public LoginInfo GetLoginInfo(string key) => CheckResult(GetHttpClient("Auth").Get<OperationResult<LoginInfo>>(new { key }));
        public LoginInfo Login(string username, string password) => CheckResult(GetHttpClient("Auth").Get<OperationResult<LoginInfo>>(new { username, password }));
        public Token Logout(string key) => CheckResult(GetHttpClient($"Auth?key={key}").Delete<OperationResult<Token>>());
        //====================================================================================
        public List<UserRole> GetAllUserRolesByUserId(int id) => GetHttpClient($"Users/{id}/Roles").Get<List<UserRole>>();
        public List<TokenUsage> GetAllTokenUsagesByTokenId(int id) => GetHttpClient($"Tokens/{id}/Usages").Get<List<TokenUsage>>();
        public List<ThirdPartyAppAccess> GetAllThirdPartyAppAccessesByThirdPartyAppId(int id) => GetHttpClient($"ThirdPartyApps/{id}/Accesses").Get<List<ThirdPartyAppAccess>>();
        public List<UserPermission> GetAllUserPermissionsByUserId(int id) => GetHttpClient($"Users/{id}/Permissions").Get<List<UserPermission>>();
        public List<ThirdPartyAppAccess> GetAllThirdPartyAppAccessesByThirdPartyAccessId(int id) => GetHttpClient($"ThirdPartyAccesses/{id}/Apps").Get<List<ThirdPartyAppAccess>>();
        public List<UserRole> GetAllUserRolesByRoleId(int id) => GetHttpClient($"Roles/{id}/Users").Get<List<UserRole>>();
        public List<Claim> GetAllClaimsByModuleId(int id) => GetHttpClient($"Modules/{id}/Claims").Get<List<Claim>>();
        public List<UserQuestion> GetAllUserQuestionsByQuestionId(int id) => GetHttpClient($"Questions/{id}/Users").Get<List<UserQuestion>>();
        public List<UserClaim> GetAllUserClaimsByUserId(int id) => GetHttpClient($"Users/{id}/Claims").Get<List<UserClaim>>();
        public List<RolePermission> GetAllRolePermissionsByRoleId(int id) => GetHttpClient($"Roles/{id}/Permissions").Get<List<RolePermission>>();
        public List<User> GetAllUsersByPositionId(int id) => GetHttpClient($"Positions/{id}/Users").Get<List<User>>();
        public List<RolePermission> GetAllRolePermissionsByPermissionId(int id) => GetHttpClient($"Permissions/{id}/Roles").Get<List<RolePermission>>();
        public List<Role> GetAllRolesByModuleId(int id) => GetHttpClient($"Modules/{id}/Roles").Get<List<Role>>();
        public List<UserClaim> GetAllUserClaimsByClaimId(int id) => GetHttpClient($"Claims/{id}/Users").Get<List<UserClaim>>();
        public List<RoleMember> GetAllRoleParentsByRoleId(int id) => GetHttpClient($"Roles/{id}/Parents").Get<List<RoleMember>>();
        public List<UserContact> GetAllContactsByUserId(int id) => GetHttpClient($"Users/{id}/Contacts").Get<List<UserContact>>();
        public List<RoleMember> GetAllRoleMembersByRoleId(int id) => GetHttpClient($"Roles/{id}/Members").Get<List<RoleMember>>();
        public List<Token> GetAllTokensByUserId(int id) => GetHttpClient($"Users/{id}/Tokens").Get<List<Token>>();
        public List<UserPermission> GetAllUserPermissionsByPermissionId(int id) => GetHttpClient($"Permissions/{id}/Users").Get<List<UserPermission>>();
        public List<UserMember> GetAllUserMembersByUserId(int id) => GetHttpClient($"Users/{id}/Members").Get<List<UserMember>>();
        public List<Permission> GetAllPermissionsByModuleId(int id) => GetHttpClient($"Modules/{id}/Permissions").Get<List<Permission>>();
        public List<UserApp> GetAllUserAppsByAuthenticatorAppId(int id) => GetHttpClient($"AuthenticatorApps/{id}/Users").Get<List<UserApp>>();
        public List<UserMember> GetAllUserParentsByUserId(int id) => GetHttpClient($"Users/{id}/Parents").Get<List<UserMember>>();
        //====================================================================================
        public UserContact AddContact(int userId, UserContact obj) => CheckResult(GetHttpClient($"Users/{userId}/Contacts").Post<OperationResult<UserContact>>());
        public UserMember AddUserMember(int userId, UserMember obj) => CheckResult(GetHttpClient($"Users/{userId}/Members").Post<OperationResult<UserMember>>());
        public UserMember AddUserParent(int userId, UserMember obj) => CheckResult(GetHttpClient($"Users/{userId}/Parents").Post<OperationResult<UserMember>>());
        public UserClaim AddUserClaim(int userId, UserClaim obj) => CheckResult(GetHttpClient($"Users/{userId}/Claims").Post<OperationResult<UserClaim>>());
        public UserPermission AddUserPermissionByPermissionId(int permissionId, UserPermission obj) => CheckResult(GetHttpClient($"Permissions/{permissionId}/Users").Post<OperationResult<UserPermission>>());
        public UserPermission AddUserPermissionByUserId(int userId, UserPermission obj) => CheckResult(GetHttpClient($"Users/{userId}/Permissions").Post<OperationResult<UserPermission>>());
        public UserRole AddUserRoleByRoleId(int roleId, UserRole obj) => CheckResult(GetHttpClient($"Roles/{roleId}/Users").Post<OperationResult<UserRole>>());
        public UserRole AddUserRoleByUserId(int userId, UserRole obj) => CheckResult(GetHttpClient($"Users/{userId}/Roles").Post<OperationResult<UserRole>>());
        public RolePermission AddRolePermissionByPermissionId(int permissionId, RolePermission obj) => CheckResult(GetHttpClient($"Permissions/{permissionId}/Roles").Post<OperationResult<RolePermission>>());
        public RolePermission AddRolePermissionByRoleId(int roleId, RolePermission obj) => CheckResult(GetHttpClient($"Roles/{roleId}/Permissions").Post<OperationResult<RolePermission>>());
        public RoleMember AddRoleMember(int roleId, RoleMember obj) => CheckResult(GetHttpClient($"Roles/{roleId}/Members").Post<OperationResult<RoleMember>>());
        public RoleMember AddRoleParent(int roleId, RoleMember obj) => CheckResult(GetHttpClient($"Roles/{roleId}/Parents").Post<OperationResult<RoleMember>>());
        //====================================================================================
        public UserContact RemoveContact(int userId, int contactId) => CheckResult(GetHttpClient($"Users/{userId}/Contacts/{contactId}").Delete<OperationResult<UserContact>>());
        public UserMember RemoveUserMember(int userId, int memberId) => CheckResult(GetHttpClient($"Users/{userId}/Members/{memberId}").Delete<OperationResult<UserMember>>());
        public UserRole RemoveUserRole(int userId, int roleId) => CheckResult(GetHttpClient($"Users/{userId}/Roles/{roleId}").Delete<OperationResult<UserRole>>());
        public RolePermission RemoveRolePermission(int roleId, int permissionId) => CheckResult(GetHttpClient($"Roles/{roleId}/Permissions/{permissionId}").Delete<OperationResult<RolePermission>>());
        public UserPermission RemoveUserPermission(int userId, int permissionId) => CheckResult(GetHttpClient($"Users/{userId}/Permissions/{permissionId}").Delete<OperationResult<UserPermission>>());
        public RoleMember RemoveRoleMember(int roleId, int memberId) => CheckResult(GetHttpClient($"Roles/{roleId}/Members/{memberId}").Delete<OperationResult<RoleMember>>());
        public RoleMember RemoveRoleParent(int roleId, int parentId) => CheckResult(GetHttpClient($"Roles/{roleId}/Parents/{parentId}").Delete<OperationResult<RoleMember>>());
        public UserMember RemoveUserParent(int userId, int parentId) => CheckResult(GetHttpClient($"Users/{userId}/Parents/{parentId}").Delete<OperationResult<UserMember>>());
        public UserClaim RemoveUserClaim(int userId, int claimId) => CheckResult(GetHttpClient($"Users/{userId}/Claims/{claimId}").Delete<OperationResult<UserClaim>>());
    }
}
