using Bitspco.Identity.Business;
using Bitspco.Identity.Facade.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using Bitspco.Identity.Common.Entities;
using Bitspco.Identity.Common.Models;
using System.Web;
using Bitspco.Framework.Filters.Security.IP;
using Bitspco.Framework.Filters.Security.AntiDos;
using Bitspco.Framework.Filters.Security;
using Bitspco.Framework.Filters.Log;
using Bitspco.Framework.Filters.Security.Authenticate;
using Bitspco.Framework.Filters;
using Bitspco.Framework.Filters.Security.Models;
using Bitspco.Identity.Data.Common.Interfaces;
using Bitspco.Identity.Data;
using Bitspco.Identity.Data.Contexts;

namespace Bitspco.Identity.Facade
{
    [Auth(Policy = "r:Admin|c:Module", Operation = OperationType.Or, MethodNamePolicyEnable = true)]
    public class IdentityController
    {
        private const string Identity_CONNECTIONSTRING = "data source=.;initial catalog=Identity;persist security info=True;user id=;password=;MultipleActiveResultSets=True;App=EntityFramework";

        private IdentityBusiness business;
        private IDataAdapter adapter;

        private Authenticator auth;
        private AuthFilter authFilter = new AuthFilter();
        private LogFilter logFilter = new LogFilter(new Logger());
        private SecurityFilter securityFilter = new SecurityFilter();
        private AntiDosFilter antiDosFilter = new AntiDosFilter();
        private IPFilter ipFilter = new IPFilter();
        private FilterCollection<IFilter> filters = new FilterCollection<IFilter>();

        private IDataAdapter Adapter
        {
            get
            {
                if (adapter != null) return adapter;
                return adapter = new DataAdapter(new IdentityDBContext(Identity_CONNECTIONSTRING));
            }
        }
        private IdentityBusiness Business
        {
            get
            {
                if (business == null)
                {
                    business = new IdentityBusiness(Adapter);
                }
                return business;
            }
        }

        public IdentityController()
        {
            filters.Add(authFilter);
            filters.Add(logFilter);

            antiDosFilter.AttributeEnable = true;
            securityFilter.Filters.Add(antiDosFilter);
            securityFilter.Filters.Add(ipFilter);

            filters.Add(securityFilter);
        }

        public void RegisterSecurity(HttpRequestBase request)
        {
            securityFilter.ClientInfo = new ClientInfo(request);
        }

        public void RegisterAuthenticator(string token)
        {
            auth = new Authenticator(Business) { Symbol = "Identity", Token = token };
            authFilter.SetAuthenticator(auth);
        }
        private T Run<T>(Func<T> func, bool saveChanges = false, object[] arguments = null)
        {
            filters.BeginExecute(frameBack: 2, arguments: arguments);
            try { return func(); }
            finally { filters.EndExecute(); if (saveChanges) Business.SaveChanges(); }
        }
        //------------------------------ CRUD ------------------------------
        #region CRUD
        private IQueryable<T> Set<T>() where T : class => Business.Set<T>();
        private T GetById<T>(object id) where T : class => Business.GetById<T>(id);
        private List<T> GetAll<T>() where T : class => Business.GetAll<T>();
        private int Count<T>() where T : class => Business.Count<T>();
        private T Add<T>(T entity) where T : class { try { return Business.Add(entity); } finally { Business.SaveChanges(); } }
        private T Change<T>(T entity) where T : class { try { return Business.Change(entity); } finally { Business.SaveChanges(); } }
        private T Remove<T>(T entity) where T : class { try { return Business.Remove(entity); } finally { Business.SaveChanges(); } }
        private T Remove<T>(object id) where T : class { try { return Business.Remove(GetById<T>(id)); } finally { Business.SaveChanges(); } }
        #endregion
        #region AuthenticatorApp
        public IQueryable<AuthenticatorApp> GetAllAuthenticatorApps() => Run(() => Set<AuthenticatorApp>());
        public AuthenticatorApp GetAuthenticatorApp(int id) => Run(() => GetById<AuthenticatorApp>(id));
        public AuthenticatorApp AddAuthenticatorApp(AuthenticatorApp obj) => Run(() => Add(obj));
        public AuthenticatorApp ChangeAuthenticatorApp(AuthenticatorApp obj) => Run(() => Change(obj));
        public AuthenticatorApp RemoveAuthenticatorApp(int id) => Run(() => Remove<AuthenticatorApp>(id));
        public IQueryable<UserApp> GetAllUserAppsByAuthenticatorAppId(int id)
        {
            return Run(() => Set<UserApp>().Where(x => x.AuthenticatorAppId == id));
        }
        #endregion
        #region Claim
        public IQueryable<Claim> GetAllClaims() => Run(() => Set<Claim>());
        public Claim GetClaim(int id) => Run(() => GetById<Claim>(id));
        public Claim AddClaim(Claim obj) => Run(() => Add(obj));
        public Claim ChangeClaim(Claim obj) => Run(() => Change(obj));
        public Claim RemoveClaim(int id) => Run(() => Remove<Claim>(id));
        public IQueryable<UserClaim> GetAllUserClaimsByClaimId(int id)
        {
            return Run(() => Set<UserClaim>().Where(x => x.ClaimId == id));
        }
        public UserClaim ChangeUserClaim(UserClaim obj)
        {
            return Run(() => Change(obj));
        }
        #endregion
        #region Event
        public IQueryable<Event> GetAllEvents() => Run(() => Set<Event>());

        public Event GetEvent(int id) => Run(() => GetById<Event>(id));
        public Event AddEvent(Event obj) => Run(() => Add(obj));
        public Event ChangeEvent(Event obj) => Run(() => Change(obj));
        public Event RemoveEvent(int id) => Run(() => Remove<Event>(id));
        #endregion
        #region Module
        public IQueryable<Module> GetAllModules() => Run(() => Set<Module>());
        public Module GetModule(int id) => Run(() => GetById<Module>(id));
        public Module AddModule(Module obj) => Run(() => Add(obj));
        public Module ChangeModule(Module obj) => Run(() => Change(obj));
        public Module RemoveModule(int id) => Run(() => Remove<Module>(id));
        public IQueryable<Claim> GetAllClaimsByModuleId(int id)
        {
            return Set<Claim>().Where(x => x.ModuleId == id);
        }
        public IQueryable<Role> GetAllRolesByModuleId(int id)
        {
            return Run(() => Set<Role>().Where(x => x.ModuleId == id));
        }
        public IQueryable<Permission> GetAllPermissionsByModuleId(int id)
        {
            return Run(() => Set<Permission>().Where(x => x.ModuleId == id));
        }
        public IQueryable<ThirdPartyAccess> GetAllThirdPartyAccesses(int id)
        {
            return Run(() => Set<ThirdPartyAccess>().Where(x => x.ModuleId == id));
        }
        #endregion
        #region Permission
        public IQueryable<Permission> GetAllPermissions() => Run(() => Set<Permission>());
        public Permission GetPermission(int id) => Run(() => GetById<Permission>(id));
        public Permission AddPermission(Permission obj) => Run(() => Add(obj));
        public Permission ChangePermission(Permission obj) => Run(() => Change(obj));
        public Permission RemovePermission(int id) => Run(() => Remove<Permission>(id));
        public IQueryable<RolePermission> GetAllRolePermissionsByPermissionId(int id)
        {
            return Run(() => Set<RolePermission>().Where(x => x.PermissionId == id));
        }
        public IQueryable<UserPermission> GetAllUserPermissionsByPermissionId(int id)
        {
            return Run(() => Set<UserPermission>().Where(x => x.PermissionId == id));
        }
        public RolePermission AddRolePermissionByPermissionId(RolePermission obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserPermission AddUserPermissionByPermissionId(UserPermission obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public RolePermission RemoveRolePermissionByPermissionId(int permissionId, int roleId)
        {
            return Run(() => Remove(Set<RolePermission>().FirstOrDefault(x => x.PermissionId == permissionId && x.RoleId == roleId)), saveChanges: true);
        }
        public UserPermission RemoveUserRoleByPermissionId(int permissionId, int userId)
        {
            return Run(() => Remove(Set<UserPermission>().FirstOrDefault(x => x.PermissionId == permissionId && x.UserId == userId)), saveChanges: true);
        }
        #endregion
        #region Position
        public IQueryable<Position> GetAllPositions() => Run(() => Set<Position>());
        public Position GetPosition(int id) => Run(() => GetById<Position>(id));
        public Position AddPosition(Position obj) => Run(() => Add(obj));
        public Position ChangePosition(Position obj) => Run(() => Change(obj));
        public Position RemovePosition(int id) => Run(() => Remove<Position>(id));
        public IQueryable<User> GetAllUsersByPositionId(int id)
        {
            return Run(() => Set<User>().Where(x => x.PositionId == id));
        }
        #endregion
        #region Question
        public IQueryable<Question> GetAllQuestions() => Run(() => Set<Question>());
        public Question GetQuestion(int id) => Run(() => GetById<Question>(id));
        public Question AddQuestion(Question obj) => Run(() => Add(obj));
        public Question ChangeQuestion(Question obj) => Run(() => Change(obj));
        public Question RemoveQuestion(int id) => Run(() => Remove<Question>(id));
        public IQueryable<UserQuestion> GetAllUserQuestionsByQuestionId(int id)
        {
            return Run(() => Set<UserQuestion>().Where(x => x.QuestionId == id));
        }
        #endregion
        #region Role
        public IQueryable<Role> GetAllRoles() => Run(() => Set<Role>());
        public Role GetRole(int id) => Run(() => GetById<Role>(id));
        public Role AddRole(Role obj) => Run(() => Add(obj));
        public Role ChangeRole(Role obj) => Run(() => Change(obj));
        public Role RemoveRole(int id) => Run(() => Remove<Role>(id));
        public IQueryable<UserRole> GetAllUserRolesByRoleId(int id)
        {
            return Run(() => Set<UserRole>().Where(x => x.RoleId == id));
        }
        public IQueryable<RolePermission> GetAllRolePermissionsByRoleId(int id)
        {
            return Run(() => Set<RolePermission>().Where(x => x.RoleId == id));
        }
        public IQueryable<RoleMember> GetAllRoleParentsByRoleId(int id)
        {
            return Run(() => Set<RoleMember>().Where(x => x.MemberId == id));
        }
        public IQueryable<RoleMember> GetAllRoleMembersByRoleId(int id)
        {
            return Run(() => Set<RoleMember>().Where(x => x.BaseId == id));
        }
        public RolePermission AddRolePermissionByRoleId(RolePermission obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserRole AddUserRoleByRoleId(UserRole obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public RoleMember AddRoleMember(RoleMember obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public RoleMember AddRoleParent(RoleMember obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public RoleMember RemoveRoleMember(int roleId, int memberId)
        {
            return Run(() => Remove(Set<RoleMember>().FirstOrDefault(x => x.BaseId == roleId && x.MemberId == memberId)), saveChanges: true);
        }
        public RoleMember RemoveRoleParent(int roleId, int parentId)
        {
            return Run(() => Remove(Set<RoleMember>().FirstOrDefault(x => x.MemberId == roleId && x.BaseId == parentId)), saveChanges: true);
        }
        public RolePermission RemoveRolePermission(int roleId, int permissionId)
        {
            return Run(() => Remove(Set<RolePermission>().FirstOrDefault(x => x.RoleId == roleId && x.PermissionId == permissionId)), saveChanges: true);
        }
        public UserRole RemoveUserRoleByRoleId(int roleId, int userId)
        {
            return Run(() => Remove(Set<UserRole>().FirstOrDefault(x => x.RoleId == roleId && x.UserId == userId)), saveChanges: true);
        }
        #endregion
        #region ThirdPartyAccess
        public IQueryable<ThirdPartyAccess> GetAllThirdPartyAccesses() => Run(() => Set<ThirdPartyAccess>());
        public ThirdPartyAccess GetThirdPartyAccess(int id) => Run(() => GetById<ThirdPartyAccess>(id));
        public ThirdPartyAccess AddThirdPartyAccess(ThirdPartyAccess obj) => Run(() => Add(obj));
        public ThirdPartyAccess ChangeThirdPartyAccess(ThirdPartyAccess obj) => Run(() => Change(obj));
        public ThirdPartyAccess RemoveThirdPartyAccess(int id) => Run(() => Remove<ThirdPartyAccess>(id));
        public IQueryable<ThirdPartyAppAccess> GetAllThirdPartyAppAccessesByThirdPartyAccessId(int id)
        {
            return Run(() => Set<ThirdPartyAppAccess>().Where(x => x.ThirdPartyAccessId == id));
        }
        #endregion
        #region ThirdPartyApp
        public IQueryable<ThirdPartyApp> GetAllThirdPartyApps() => Run(() => Set<ThirdPartyApp>());
        public ThirdPartyApp GetThirdPartyApp(int id) => Run(() => GetById<ThirdPartyApp>(id));
        public ThirdPartyApp AddThirdPartyApp(ThirdPartyApp obj) => Run(() => Add(obj));
        public ThirdPartyApp ChangeThirdPartyApp(ThirdPartyApp obj) => Run(() => Change(obj));
        public ThirdPartyApp RemoveThirdPartyApp(int id) => Run(() => Remove<ThirdPartyApp>(id));
        public IQueryable<ThirdPartyAppAccess> GetAllThirdPartyAppAccessesByThirdPartyAppId(int id)
        {
            return Run(() => Set<ThirdPartyAppAccess>().Where(x => x.ThirdPartyAppId == id));
        }
        #endregion
        #region Token
        public IQueryable<Token> GetAllTokens() => Run(() => Set<Token>());
        public Token GetToken(int id) => Run(() => GetById<Token>(id));
        public Token AddToken(Token obj) => Run(() => Add(obj));
        public Token ChangeToken(Token obj) => Run(() => Change(obj));
        public Token ExpireToken(int id) => Run(() => Business.ExpireToken(GetToken(id)));
        public Token RemoveToken(int id) => Run(() => Remove<Token>(id));
        public Token GetTokenByKey(string key)
        {
            return Run(() => Business.GetTokenByKey(key));
        }
        public IQueryable<TokenUsage> GetAllTokenUsagesByTokenId(int id)
        {
            return Run(() => Set<TokenUsage>().Where(x => x.TokenId == id));
        }
        #endregion
        #region User
        public IQueryable<User> GetAllUsers() => Run(() => Set<User>());
        public User GetUser(int id) => Run(() => GetById<User>(id));
        public User AddUser(User obj) => Run(() => Business.AddUser(obj), saveChanges: true);
        public User ChangeUser(User obj) => Run(() => Business.ChangeUser(obj), saveChanges: true);
        public User RemoveUser(int id) => Run(() => Remove<User>(id), saveChanges: true);
        public IQueryable<UserRole> GetAllUserRolesByUserId(int id) => Run(() => Set<UserRole>().Where(x => x.UserId == id));
        public IQueryable<UserClaim> GetAllUserClaimsByUserId(int id) => Run(() => Set<UserClaim>().Where(x => x.UserId == id));
        public IQueryable<UserPermission> GetAllUserPermissionsByUserId(int id) => Run(() => Set<UserPermission>().Where(x => x.UserId == id));
        public IQueryable<UserContact> GetAllContactsByUserId(int id) => Run(() => Set<UserContact>().Where(x => x.UserId == id));
        public IQueryable<Token> GetAllTokensByUserId(int id) => Run(() => Set<Token>().Where(x => x.UserId == id));
        public IQueryable<UserMember> GetAllUserMembersByUserId(int id) => Run(() => Set<UserMember>().Where(x => x.BaseId == id));
        public IQueryable<UserMember> GetAllUserParentsByUserId(int id) => Run(() => Set<UserMember>().Where(x => x.MemberId == id));
        public IQueryable<UserApp> GetAllApps(int id) => Run(() => Business.Set<UserApp>().Where(x => x.UserId == id), saveChanges: true);
        public IQueryable<Event> GetAllEvents(int id) => Set<Event>().Where(x => x.UserId == id);
        public IQueryable<UserQuestion> GetAllQuestions(int id) => Set<UserQuestion>().Where(x => x.UserId == id);
        public Token AddTokenByUserId(int id)
        {
            var token = new Token()
            {
                UserId = id,
                Key = Guid.NewGuid().ToString()
            };
            return Run(() => Add(token), saveChanges: true);
        }
        public UserContact AddContact(UserContact obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserMember AddUserMember(UserMember obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserMember AddUserParent(UserMember obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserClaim AddUserClaim(UserClaim obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserRole AddUserRoleByUserId(UserRole obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserPermission AddUserPermissionByUserId(UserPermission obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserApp AddApp(UserApp obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserQuestion AddUserQuestion(UserQuestion obj)
        {
            return Run(() => Add(obj), saveChanges: true);
        }
        public UserContact ApproveContact(int userId, int contactId)
        {
            var obj = GetAllContactsByUserId(userId).FirstOrDefault(x => x.Id == contactId);
            obj.IsVerify = true;
            return Change(obj);
        }

        public UserContact RejectContact(int userId, int contactId)
        {
            var obj = GetAllContactsByUserId(userId).FirstOrDefault(x => x.Id == contactId);
            obj.IsVerify = false;
            return Change(obj);
        }
        public UserContact RemoveContact(int userId, int contactId)
        {
            return Run(() => Remove(Set<UserContact>().Where(x => x.UserId == userId && x.Id == contactId).FirstOrDefault()), saveChanges: true);
        }
        public UserMember RemoveUserMember(int userId, int memberId)
        {
            return Run(() => Remove(Set<UserMember>().Where(x => x.BaseId == userId && x.MemberId == memberId).FirstOrDefault()), saveChanges: true);
        }
        public UserRole RemoveUserRole(int userId, int roleId)
        {
            return Run(() => Remove(Set<UserRole>().Where(x => x.UserId == userId && x.RoleId == roleId).FirstOrDefault()), saveChanges: true);
        }
        public UserMember RemoveUserParent(int userId, int parentId)
        {
            return Run(() => Remove(Set<UserMember>().FirstOrDefault(x => x.MemberId == userId && x.BaseId == parentId)), saveChanges: true);
        }
        public UserClaim RemoveUserClaim(int userId, int claimId)
        {
            return Run(() => Remove(Set<UserClaim>().FirstOrDefault(x => x.UserId == userId && x.ClaimId == claimId)), saveChanges: true);
        }
        public UserPermission RemoveUserPermission(int userId, int permissionId)
        {
            return Run(() => Remove(Set<UserPermission>().FirstOrDefault(x => x.UserId == userId && x.PermissionId == permissionId)), saveChanges: true);
        }
        public Token RemoveTokenByUserId(int userId, string key)
        {
            return Run(() => Business.RemoveTokenByUserId(userId, key), saveChanges: true);
        }
        public UserApp RemoveApp(int userId, int appId)
        {
            return Run(() => Remove(Set<UserApp>().FirstOrDefault(x => x.UserId == userId && x.AuthenticatorAppId == appId)), saveChanges: true);
        }
        public Event RemoveEvent(int userId, int eventId)
        {
            return Run(() => Remove(Set<Event>().FirstOrDefault(x => x.UserId == userId && x.Id == eventId)), saveChanges: true);
        }
        public UserQuestion RemoveUserQuestion(int userId, int questionId)
        {
            return Run(() => Remove(Set<UserQuestion>().FirstOrDefault(x => x.UserId == userId && x.QuestionId == questionId)), saveChanges: true);
        }
        #endregion

        [AuthIgnore]
        public LoginInfo GetLoginInfo(string key)
        {
            return Run(() => Business.GetLoginInfo(key));
        }
        [AuthIgnore]
        public LoginInfo Login(string username, string password)
        {
            return Run(() => Business.Login(username, password), saveChanges: true);
        }
        [AuthIgnore]
        public Token Logout(string key)
        {
            return Run(() => Business.Logout(key));
        }

    }
}
