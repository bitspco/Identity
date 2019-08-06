using Bitspco.Identity.Common.Entities;
using Bitspco.Identity.Common.Enums;
using Bitspco.Identity.Common.Models;
using Bitspco.Identity.Data.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bitspco.Identity.Business
{
    public class IdentityBusiness
    {
        private static bool installed = false;

        private IDataAdapter adapter;

        public IdentityBusiness(IDataAdapter adapter)
        {
            this.adapter = adapter;
            if (!installed) Install();
        }
        private void Install()
        {
            //------ User -----//
            var user = Set<User>().FirstOrDefault();
            if (user == null)
            {
                using (var transaction = adapter.BeginTransaction())
                {
                    try
                    {
                        user = AddUser(new User()
                        {
                            Name = "Admin",
                            Username = "Admin",
                            Password = "123",
                        });
                        //------ Module -----//
                        var module = AddAndSave(new Module() { Name = "Identity", Symbol = "Identity", Status = ModuleStatus.Active });
                        //------ Role -----//
                        var role = Add(new Role() { ModuleId = module.Id, Name = "Admin", Symbol = "Admin", Status = RoleStatus.Active });
                        //------ Permission -----//
                        Permission permission;
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "AuthenticatorApp", Symbol = "AuthenticatorApp" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "Claim", Symbol = "Claim" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "Event", Symbol = "Event" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "Module", Symbol = "Module" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "Permission", Symbol = "Permission" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "Position", Symbol = "Position" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "Question", Symbol = "Question" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "Role", Symbol = "Role" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "Social", Symbol = "Social" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "Token", Symbol = "Token" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        permission = AddAndSave(new Permission() { ModuleId = module.Id, Name = "User", Symbol = "User" });
                        AddAndSave(new RolePermission(role.Id, permission.Id));
                        //------ UserRole -----//
                        AddAndSave(new UserRole(user.Id, role.Id));
                        AddToken(new Token() { UserId = user.Id });
                        transaction.Commit();
                        installed = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //------------------------------ CRUD ------------------------------
        #region CRUD
        public IQueryable<T> Set<T>() where T : class => adapter.Select<T>();
        public T GetById<T>(object id) where T : class => adapter.GetById<T>(id);
        public List<T> GetAll<T>() where T : class => adapter.GetAll<T>();
        public int Count<T>() where T : class => adapter.Count<T>();
        public T Add<T>(T entity) where T : class => adapter.Insert(entity);
        public T AddAndSave<T>(T entity) where T : class => adapter.InsertAndSave(entity);
        public T Change<T>(T entity) where T : class => adapter.Update(entity);
        public T ChangeAndSave<T>(T entity) where T : class => adapter.UpdateAndSave(entity);
        public T Remove<T>(T entity) where T : class => adapter.Delete(entity);
        public T RemoveAndSave<T>(T entity) where T : class => adapter.DeleteAndSave(entity);
        public void SaveChanges(SystemCode system = SystemCode.Identity) => adapter.SaveChanges(system);
        #endregion

        public Token Get(string key)
        {
            return Set<Token>().FirstOrDefault(x => x.Key == key);
        }
        public Token Logout(string key)
        {
            var obj = GetTokenByKey(key);
            return ExpireToken(obj);
        }
        public Token ExpireToken(Token obj)
        {
            obj.ExpireTime = DateTime.Now;
            obj.Status = TokenStatus.Expired;
            return ChangeAndSave(obj);
        }
        public User GetUser(string username)
        {
            return Set<User>().FirstOrDefault(x => x.Username == username);
        }
        public User GetUser(string username, string password)
        {
            var user = GetUser(username);
            if (user != null && user.Password == HashPassword(password, user.SecretKey)) return user;
            return null;
        }
        private string HashPassword(string password, string secretKey)
        {
            HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(secretKey));
            return Encoding.ASCII.GetString(hmac.ComputeHash(Encoding.ASCII.GetBytes(password)));
        }
        public User AddUser(User obj)
        {
            obj.SecretKey = Guid.NewGuid().ToString();
            obj.Password = HashPassword(obj.Password, obj.SecretKey);
            return Add(obj);
        }
        public Token AddToken(Token obj)
        {
            obj.Key = Guid.NewGuid().ToString();
            return AddAndSave(obj);
        }
        public Token GetTokenByKey(string key)
        {
            return Set<Token>().FirstOrDefault(x => x.Key == key);
        }
        public LoginInfo GetLoginInfo(string key)
        {
            return GetLoginInfo(GetTokenByKey(key));
        }
        public LoginInfo GetLoginInfo(Token token)
        {
            if (token == null) return null;
            var user = token.User;
            var moduleInfos = new List<ModuleInfo>();
            var userRoles = Set<UserRole>().Where(x => x.UserId == user.Id).ToList();
            var userPermissions = Set<UserPermission>().Where(x => x.UserId == user.Id).ToList();
            var userClaims = Set<UserClaim>().Where(x => x.UserId == user.Id).ToList();

            foreach (var item in GetAll<Module>())
            {
                var moduleInfo = new ModuleInfo();
                moduleInfo.Id = item.Id;
                moduleInfo.Symbol = item.Symbol;
                moduleInfo.Roles = userRoles.Where(x => x.Role.ModuleId == item.Id).ToList().ConvertAll(x => x.Role.Symbol);
                moduleInfo.Permissions = new List<string>();
                foreach (var rolePermission in userRoles.Where(x => x.Role.ModuleId == item.Id).ToList().ConvertAll(x => x.Role.Permissions))
                    moduleInfo.Permissions = moduleInfo.Permissions.Concat(rolePermission.ConvertAll(x => x.Permission.Symbol)).ToList();
                moduleInfo.Claims = userClaims.Where(x => x.Claim.ModuleId == item.Id).ToDictionary(x => x.Claim.Symbol, x => x.Value);
                moduleInfos.Add(moduleInfo);
            }

            return new LoginInfo()
            {
                Id = token.Id,
                Key = token.Key,
                UserId = user.Id,
                Username = user.Username,
                Name = user.Name,
                Token = token,
                Modules = moduleInfos
            };
        }
        public LoginInfo Login(string username, string password)
        {
            var user = Set<User>().FirstOrDefault(x => x.Username == username);
            if (user == null) throw new Exception("User not found");
            if (HashPassword(password, user.SecretKey) != user.Password) throw new Exception("Login Failed");

            var lastToken = Set<Token>().OrderByDescending(x => x.Id).Skip(10).FirstOrDefault(x => x.UserId == user.Id);
            //if (lastToken != null && DateTime.Now.AddHours(-1) < lastToken.CreationTime) throw new Exception("Can't get token now.Try again later");

            var token = new Token()
            {
                Key = Guid.NewGuid().ToString(),
                UserId = user.Id,
                User = user,
                IsNeedVerfication = (user.IsAppAuthenticatorEnable || user.IsContactAuthenticatorEnable)
            };
            if (user.IsContactAuthenticatorEnable) token.VerficationCode = new Random().Next(10000, 99999);

            using (var transaction = adapter.BeginTransaction())
            {
                try
                {

                    var activeTokens = Set<Token>().Where(x => x.UserId == user.Id && x.Status != TokenStatus.Expired).ToList();
                    var expiringCount = activeTokens.Count - user.MaxTokenCount;
                    for (int i = 0; i <= expiringCount; i++)
                        ExpireToken(activeTokens[i]);
                    AddToken(token);
                    transaction.Commit();
                    return GetLoginInfo(token);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Token RemoveTokenByUserId(int userId, string key)
        {
            var token = GetTokenByKey(key);
            if (token.UserId != userId) throw new Exception("Operation Failed");
            return Remove(token);
        }

        public User ChangeUser(User obj)
        {
            var user = GetById<User>(obj.Id);
            user.Name = obj.Name;
            user.Username = obj.Username;
            user.NationalId = obj.NationalId;
            user.Birthday = obj.Birthday;
            user.Gender = obj.Gender;
            user.Description = obj.Description;
            user.PositionId = obj.PositionId;
            user.Timeout = obj.Timeout;
            user.MaxTokenCount = obj.MaxTokenCount;
            user.IsNeedChangePassword = obj.IsNeedChangePassword;
            user.Status = obj.Status;
            return Change(user);
        }
    }
}
