namespace Bitspco.Identity.Data.Contexts
{
    using Bitspco.Identity.Common.Entities;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class IdentityDBContext : DbContext
    {
        public DbSet<AuthenticatorApp> AuthenticatorApps { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleMember> RoleMembers { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ThirdPartyApp> ThirdPartyApps { get; set; }
        public DbSet<ThirdPartyAccess> ThirdPartyAccesses { get; set; }
        public DbSet<ThirdPartyAppAccess> ThirdPartyAppAccesses { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenUsage> TokenUsages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserApp> UserSecurityApps { get; set; }
        public DbSet<UserQuestion> UserSecurityQuestions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserMember> UserMembers { get; set; }

        public IdentityDBContext() : base("name=Default") { }
        public IdentityDBContext(string connectionString) : base(DbProviderFactories.GetFactory("System.Data.SqlClient").CreateConnection(), true)
        {
            Database.Connection.ConnectionString = connectionString;
            Configuration.ProxyCreationEnabled = true;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<UserMember>().HasRequired(x => x.Base).WithMany().HasForeignKey(x => x.BaseId).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserMember>().HasRequired(x => x.Member).WithMany().HasForeignKey(x => x.MemberId).WillCascadeOnDelete(false);

            modelBuilder.Entity<RoleMember>().HasRequired(x => x.Base).WithMany().HasForeignKey(x => x.BaseId).WillCascadeOnDelete(false);
            modelBuilder.Entity<RoleMember>().HasRequired(x => x.Member).WithMany().HasForeignKey(x => x.MemberId).WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRole>().HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserRole>().HasRequired(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).WillCascadeOnDelete(false);

            modelBuilder.Entity<RolePermission>().HasRequired(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).WillCascadeOnDelete(false);
            modelBuilder.Entity<RolePermission>().HasRequired(x => x.Permission).WithMany().HasForeignKey(x => x.PermissionId).WillCascadeOnDelete(false);

            modelBuilder.Entity<ThirdPartyAppAccess>().HasRequired(x => x.ThirdPartyApp).WithMany().HasForeignKey(x => x.ThirdPartyAppId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ThirdPartyAppAccess>().HasRequired(x => x.ThirdPartyAccess).WithMany().HasForeignKey(x => x.ThirdPartyAccessId).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}