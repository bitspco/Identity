using Bitspco.Identity.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitspco.Identity.Common.Interfaces
{
    public interface IIdentityClient
    {
        List<T> GetAll<T>();
        T GetById<T>(int id);
        T Add<T>(T obj);
        T Change<T>(T obj);
        T Remove<T>(int id);
        //===================================================
        List<UserRole> GetAllUserRolesByUserId(int id);
        List<TokenUsage> GetAllTokenUsagesByTokenId(int id);
        List<ThirdPartyAppAccess> GetAllThirdPartyAppAccessesByThirdPartyAppId(int id);
        List<UserPermission> GetAllUserPermissionsByUserId(int id);
        List<ThirdPartyAppAccess> GetAllThirdPartyAppAccessesByThirdPartyAccessId(int id);
        List<UserRole> GetAllUserRolesByRoleId(int id);
        List<Claim> GetAllClaimsByModuleId(int id);
        List<UserQuestion> GetAllUserQuestionsByQuestionId(int id);
        List<UserClaim> GetAllUserClaimsByUserId(int id);
        List<RolePermission> GetAllRolePermissionsByRoleId(int id);
        List<User> GetAllUsersByPositionId(int id);
        List<RolePermission> GetAllRolePermissionsByPermissionId(int id);
        List<Role> GetAllRolesByModuleId(int id);
        List<UserClaim> GetAllUserClaimsByClaimId(int id);
        List<RoleMember> GetAllRoleParentsByRoleId(int id);
        List<UserContact> GetAllContactsByUserId(int id);
        List<RoleMember> GetAllRoleMembersByRoleId(int id);
        List<Token> GetAllTokensByUserId(int id);
        List<UserPermission> GetAllUserPermissionsByPermissionId(int id);
        List<UserMember> GetAllUserMembersByUserId(int id);
        List<Permission> GetAllPermissionsByModuleId(int id);
        List<UserApp> GetAllUserAppsByAuthenticatorAppId(int id);
        List<UserMember> GetAllUserParentsByUserId(int id);
        //====================================================
        UserContact AddContact(int userId, UserContact obj);
        UserMember AddUserMember(int userId, UserMember obj);
        UserMember AddUserParent(int userId, UserMember obj);
        UserClaim AddUserClaim(int userId, UserClaim obj);
        UserPermission AddUserPermissionByPermissionId(int permissionId, UserPermission obj);
        UserPermission AddUserPermissionByUserId(int userId, UserPermission obj);
        UserRole AddUserRoleByRoleId(int roleId, UserRole obj);
        UserRole AddUserRoleByUserId(int userId, UserRole obj);
        RolePermission AddRolePermissionByPermissionId(int permissionId, RolePermission obj);
        RolePermission AddRolePermissionByRoleId(int roleId, RolePermission obj);
        RoleMember AddRoleMember(int roleId, RoleMember obj);
        RoleMember AddRoleParent(int roleId, RoleMember obj);
        //====================================================
        UserContact RemoveContact(int userId, int contactId);
        UserMember RemoveUserMember(int userId, int memberId);
        UserRole RemoveUserRole(int userId, int roleId);
        RolePermission RemoveRolePermission(int roleId, int permissionId);
        UserPermission RemoveUserPermission(int userId, int permissionId);
        RoleMember RemoveRoleMember(int roleId, int memberId);
        RoleMember RemoveRoleParent(int roleId, int parentId);
        UserMember RemoveUserParent(int userId, int parentId);
        UserClaim RemoveUserClaim(int userId, int claimId);
    }
}
