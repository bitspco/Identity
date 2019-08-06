using Bitspco.Identity.Client.WebApi.V1;
using Bitspco.Identity.Common.Entities;
using Bitspco.Identity.Common.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bitspco.Identity.Client.Test
{
    [TestClass]
    public class ClientTest
    {
        private IIdentityClientVersion1 client;
        public ClientTest()
        {
            client = new IdentityClientVersion1WebApi("http://localhost:13248/", "b1470e77-f78f-4f42-867e-4a500e09f93a");
        }
        //====================================================================================
        [TestMethod]
        public void GetLoginInfo() => client.GetLoginInfo("aa");
        [TestMethod]
        public void Login() => client.Login("admin", "123");
        //====================================================================================
        [TestMethod]
        public void GetAllUserRolesByUserId() => client.GetAllUserRolesByUserId(1);
        [TestMethod]
        public void GetAllTokenUsagesByTokenId() => client.GetAllTokenUsagesByTokenId(1);
        [TestMethod]
        public void GetAllThirdPartyAppAccessesByThirdPartyAppId() => client.GetAllThirdPartyAppAccessesByThirdPartyAppId(1);
        [TestMethod]
        public void GetAllUserPermissionsByUserId() => client.GetAllUserPermissionsByUserId(1);
        [TestMethod]
        public void GetAllThirdPartyAppAccessesByThirdPartyAccessId() => client.GetAllThirdPartyAppAccessesByThirdPartyAccessId(1);
        [TestMethod]
        public void GetAllUserRolesByRoleId() => client.GetAllUserRolesByRoleId(1);
        [TestMethod]
        public void GetAllClaimsByModuleId() => client.GetAllClaimsByModuleId(1);
        [TestMethod]
        public void GetAllUserQuestionsByQuestionId() => client.GetAllUserQuestionsByQuestionId(1);
        [TestMethod]
        public void GetAllUserClaimsByUserId() => client.GetAllUserClaimsByUserId(1);
        [TestMethod]
        public void GetAllRolePermissionsByRoleId() => client.GetAllRolePermissionsByRoleId(1);
        [TestMethod]
        public void GetAllUsersByPositionId() => client.GetAllUsersByPositionId(1);
        [TestMethod]
        public void GetAllRolePermissionsByPermissionId() => client.GetAllRolePermissionsByPermissionId(1);
        [TestMethod]
        public void GetAllRolesByModuleId() => client.GetAllRolesByModuleId(1);
        [TestMethod]
        public void GetAllUserClaimsByClaimId() => client.GetAllUserClaimsByClaimId(1);
        [TestMethod]
        public void GetAllRoleParentsByRoleId() => client.GetAllRoleParentsByRoleId(1);
        [TestMethod]
        public void GetAllContactsByUserId() => client.GetAllContactsByUserId(1);
        [TestMethod]
        public void GetAllRoleMembersByRoleId() => client.GetAllRoleMembersByRoleId(1);
        [TestMethod]
        public void GetAllTokensByUserId() => client.GetAllTokensByUserId(1);
        [TestMethod]
        public void GetAllUserPermissionsByPermissionId() => client.GetAllUserPermissionsByPermissionId(1);
        [TestMethod]
        public void GetAllUserMembersByUserId() => client.GetAllUserMembersByUserId(1);
        [TestMethod]
        public void GetAllPermissionsByModuleId() => client.GetAllPermissionsByModuleId(1);
        [TestMethod]
        public void GetAllUserAppsByAuthenticatorAppId() => client.GetAllUserAppsByAuthenticatorAppId(1);
        [TestMethod]
        public void GetAllUserParentsByUserId() => client.GetAllUserParentsByUserId(1);
        //====================================================================================
        [TestMethod]
        public void AddContact() => client.AddContact(1, new UserContact());
        [TestMethod]
        public void AddUserMember() => client.AddUserMember(1, new UserMember() { MemberId = 1 });
        [TestMethod]
        public void AddUserParent() => client.AddUserParent(1, new UserMember() { BaseId = 1 });
        [TestMethod]
        public void AddUserClaim() => client.AddUserClaim(1, new UserClaim() { ClaimId = 1 });
        [TestMethod]
        public void AddUserPermissionByPermissionId() => client.AddUserPermissionByPermissionId(1, new UserPermission() { UserId = 1 });
        [TestMethod]
        public void AddUserPermissionByUserId() => client.AddUserPermissionByUserId(1, new UserPermission() { PermissionId = 1 });
        [TestMethod]
        public void AddUserRoleByRoleId() => client.AddUserRoleByRoleId(1, new UserRole() { UserId = 1 });
        [TestMethod]
        public void AddUserRoleByUserId() => client.AddUserRoleByUserId(1, new UserRole() { RoleId = 1 });
        [TestMethod]
        public void AddRolePermissionByPermissionId() => client.AddRolePermissionByPermissionId(1, new RolePermission() { RoleId = 1 });
        [TestMethod]
        public void AddRolePermissionByRoleId() => client.AddRolePermissionByRoleId(1, new RolePermission() { PermissionId = 1 });
        [TestMethod]
        public void AddRoleMember() => client.AddRoleMember(1, new RoleMember() { MemberId = 1 });
        [TestMethod]
        public void AddRoleParent() => client.AddRoleParent(1, new RoleMember() { BaseId = 1 });
        //====================================================================================
        [TestMethod]
        public void RemoveContact() => client.RemoveContact(1, 1);
        [TestMethod]
        public void RemoveUserMember() => client.RemoveUserMember(1, 1);
        [TestMethod]
        public void RemoveUserRole() => client.RemoveUserRole(1, 2);
        [TestMethod]
        public void RemoveRolePermission() => client.RemoveRolePermission(1, 1);
        [TestMethod]
        public void RemoveUserPermission() => client.RemoveUserPermission(1, 1);
        [TestMethod]
        public void RemoveRoleMember() => client.RemoveRoleMember(1, 1);
        [TestMethod]
        public void RemoveRoleParent() => client.RemoveRoleParent(1, 1);
        [TestMethod]
        public void RemoveUserParent() => client.RemoveUserParent(1, 1);
        [TestMethod]
        public void RemoveUserClaim() => client.RemoveUserClaim(1, 1);
    }
}
