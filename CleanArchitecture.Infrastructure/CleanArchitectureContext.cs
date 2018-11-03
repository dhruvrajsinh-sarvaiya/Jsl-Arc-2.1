using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Services;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Entities.Culture;
using CleanArchitecture.Core.Entities.Resource;
using CleanArchitecture.Core.Entities.Modes;
using CleanArchitecture.Core.Entities.Log;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Entities.Wallet;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Entities.UserChangeLog;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CleanArchitecture.Infrastructure
{
    public partial class CleanArchitectureContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        private readonly UserResolveService _userService;

        // Account Management
        public string CurrentUserId { get; internal set; }
        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<ApplicationUserPhotos> ApplicationUserPhotos { get; set; }
        public virtual DbSet<ApplicationRole> Roles { get; set; }
        public virtual DbSet<Cultures> Cultures { get; set; }
        public virtual DbSet<Resources> Resources { get; set; }
        public virtual DbSet<Mode> Mode { get; set; }
        public virtual DbSet<LoginLog> LoginLog { get; set; }

        public virtual DbSet<RegisterType> RegisterType { get; set; }
        public virtual DbSet<TempUserRegister> TempUserRegister { get; set; }
        public virtual DbSet<TempOtpMaster> TempOtpMaster { get; set; }
        public virtual DbSet<OtpMaster> OtpMaster { get; set; }
        public DbSet<IpHistory> IpHistory { get; set; } //ntrivedi 02112018 for migration
        public DbSet<LoginHistory> LoginHistory { get; set; } //ntrivedi 02112018 for migration
        // add by nirav savariya for create password for login with mobile and email
        public virtual DbSet<CustomPassword> CustomPassword { get; set; }
        public virtual DbSet<DeviceMaster> DeviceMaster { get; set; }
        public virtual DbSet<UserLogChange> UserLogChange { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<MessagingQueue> MessagingQueue { get; set; }
        public DbSet<EmailQueue> EmailQueue { get; set; }
        public DbSet<NotificationQueue> NotificationQueue { get; set; }
        public DbSet<CommAPIServiceMaster> CommAPIServiceMaster { get; set; }
        public DbSet<CommServiceMaster> CommServiceMaster { get; set; }
        public DbSet<CommServiceproviderMaster> CommServiceproviderMaster { get; set; }
        public DbSet<CommServiceTypeMaster> CommServiceTypeMaster { get; set; }
        public DbSet<RequestFormatMaster> RequestFormatMaster { get; set; }
        public DbSet<ServiceTypeMaster> ServiceTypeMaster { get; set; }
        public DbSet<TemplateMaster> TemplateMaster { get; set; }
        public DbSet<UserKeyMaster> UserKeyMaster { get; set; }
        // wallet tables
        public DbSet<WalletTypeMaster> WalletTypeMasters { get; set; }
        public DbSet<WalletMaster> WalletMasters { get; set; }
        public DbSet<WalletOrder> WalletOrders { get; set; }
        public DbSet<TransactionAccount> TransactionAccounts { get; set; }
        public DbSet<WalletLedger> WalletLedgers { get; set; }
        public DbSet<DepositHistory> DepositHistory { get; set; }
        public DbSet<TradeBitGoDelayAddresses> TradeBitGoDelayAddressess { get; set; }
        public DbSet<AddressMaster> AddressMasters { get; set; }
        public DbSet<WalletAllowTrn> WalletAllowTrns { get; set; }
        public DbSet<WalletTransactionOrder> WalletTransactionOrders { get; set; }
        public DbSet<WalletTransactionQueue> WalletTransactionQueues { get; set; }
        public DbSet<TrnAcBatch> TrnAcBatch { get; set; }
        public DbSet<WalletLimitConfiguration> WalletLimitConfiguration { get; set; }
        public DbSet<DepositCounterLog> DepositCounterLog { get; set; }
        public DbSet<DepositCounterMaster> DepositCounterMaster { get; set; }
        public DbSet<TradeDepositCompletedTrn> TradeDepositCompletedTrn { get; set; }

        //vsolnki 24-10-2018
        public DbSet<UserStacking> UserStacking { get; set; }
        public DbSet<MemberShadowBalance> MemberShadowBalance { get; set; }
        public DbSet<MemberShadowLimit> MemberShadowLimit { get; set; }
        public DbSet<StckingScheme> StckingScheme { get; set; }

        public DbSet<BizUserTypeMapping> BizUserTypeMapping { get; set; }
        public DbSet<UserPreferencesMaster> UserPreferencesMaster { get; set; }
        public DbSet<BeneficiaryMaster> BeneficiaryMaster { get; set; }
        public DbSet<WalletLimitConfigurationMaster> WalletLimitConfigurationMaster { get; set; }
        public DbSet<WithdrawHistory> WithdrawHistory { get; set; }
        public DbSet<ConvertFundHistory> ConvertFundHistory { get; set; }
        
        //========Transaction Tables
        public DbSet<TradeTransactionQueue> TradeTransactionQueue { get; set; }
        public DbSet<TradePairMaster> TradePairMaster { get; set; }
        public DbSet<TradePairDetail> TradePairDetail { get; set; }
        public DbSet<TransactionQueue> TransactionQueue { get; set; }
        //public DbSet<ServiceConfiguration> ServiceConfiguration { get; set; }
        public DbSet<ProductConfiguration> ProductConfiguration { get; set; }
        //public DbSet<ProviderConfiguration> ProviderConfiguration { get; set; }
        public DbSet<RouteConfiguration> RouteConfiguration { get; set; }
        public DbSet<ThirdPartyAPIConfiguration> ThirdPartyAPIConfiguration { get; set; }
        public DbSet<ThirdPartyAPIResponseConfiguration> ThirdPartyAPIResponseConfiguration { get; set; }
        public DbSet<TradePoolMaster> TradePoolMaster { get; set; }
        public DbSet <TradeCancelQueue> TradeCancelQueue { get; set; }
        public DbSet <TradeTransactionStatus> TradeTransactionStatus { get; set; }
        public DbSet <Market> Market { get; set; }

        public DbQuery<GetTradingSummary> GetTradingSummary { get; set; }  //komal 24-10-2018 
        public DbQuery<TradeHistoryResponce> TradeHistoryInfo { get; set; } //komal 11-10-2018
        public DbQuery<RecentOrderRespose> RecentOrderRespose { get; set; } //komal 12-10-2018
        public DbQuery<CommunicationProviderList> CommunicationProviderList { get; set; }
        public DbQuery<TransactionProviderResponse> TransactionProviderResponse { get; set; } // ntrivedi 03-10-2018
        public DbQuery<ActiveOrderDataResponse> ActiveOrderDataResponse { get; set; } //komal 12-10-2018
        public DbQuery<GetBuySellBook> BuyerSellerInfo { get; set; } //uday 12-10-2018
        public DbQuery<GetGraphResponse> GetGraphResponse { get; set; } //uday 22-10-2018
        //Add Tables for Service Master (Not Commited)

        public DbSet<ServiceMaster> ServiceMaster { get; set; }
        public DbSet<ServiceDetail> ServiceDetail { get; set; }
        public DbSet<ServiceStastics> ServiceStastics { get; set; }
        public DbSet<Limits> Limits { get; set; }
        public DbSet<AppType> AppType { get; set; }
        public DbSet<DemonConfiguration> DemonConfiguration { get; set; }
        public DbSet<ServiceProConfiguration> ServiceProConfiguration { get; set; }
        public DbSet<ServiceProviderDetail> ServiceProviderDetail { get; set; }
        public DbSet<ServiceProviderType> ServiceProviderType { get; set; }
        public DbSet<ServiceProviderMaster> ServiceProviderMaster { get; set; }
        public DbSet<TradeStopLoss> TradeStopLoss { get; set; }
       public DbSet<IpMaster> IpMaster { get; set; }

        public DbSet<TradeBuyRequest> TradeBuyRequest { get; set; }
        public DbSet<CountryMaster> CountryMaster { get; set; } //uday 17-10-2018
        public DbSet<StateMaster> StateMaster { get; set; } //uday 17-10-2018
        public DbSet<PoolOrder> PoolOrder { get; set; } //uday 17-10-2018
        public DbSet<CityMaster> CityMaster { get; set; } //uday 22-10-2018
        public DbSet<ZipCodeMaster> ZipCodeMaster { get; set; } //uday 22-10-2018
        public DbSet<TradeGraphDetail> TradeGraphDetail { get; set; } //uday 22-10-2018
        public DbSet<ServiceTypeMapping> ServiceTypeMapping { get; set; } //uday 24-10-2018
        public DbSet<TradePairStastics> TradePairStastics { get; set; } //uday 25-10-2018
        public DbSet<TransactionRequest> TransactionRequest { get; set; } //Rita 27-10-2018
        public DbSet<FavouritePair> FavouritePair { get; set; } //Uday 29-10-2018
        public DbSet<ChargeRuleMaster> ChargeRuleMaster { get; set; } //Uday 30-10-2018
        public DbSet<LimitRuleMaster> LimitRuleMaster { get; set; } //Uday 30-10-2018
        public DbSet<TradeBuyerList> TradeBuyerList { get; set; } //Uday 30-10-2018
        public DbSet<TradeSellerList> TradeSellerList { get; set; } //Uday 30-10-2018
        public DbSet<SettledTradeTransactionQueue> SettledTradeTransactionQueue { get; set; } //Rita 31-10-2018
        public DbSet<TradePoolConfiguration> TradePoolConfiguration { get; set; } //Rita 31-10-2018
        public DbSet<TradePoolQueue> TradePoolQueue { get; set; } //Rita 31-10-2018
        public DbSet<TransactionStatus> TransactionStatus { get; set; } //Rita 31-10-2018
        public CleanArchitectureContext(DbContextOptions<CleanArchitectureContext> options, UserResolveService userService) : base(options)
        {
            _userService = userService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("UpdatedAt");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<string>("CreatedBy");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<string>("UpdatedBy");
            }


            // modelBuilder.Entity<ApplicationUser>(b => { b.ToTable("asp_net_users"); });

            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserPhotos>().ToTable("BizUserPhotos");
            modelBuilder.Entity<ApplicationRole>().ToTable("BizRoles");
            modelBuilder.Entity<ApplicationUser>().ToTable("BizUser");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("BizRolesClaims");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("BizUserClaims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("BizUserLogin");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("BizUserRole");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("BizUserToken");

            //modelBuilder.Entity<IdentityUser>().ToTable("MyUsers").Property(p => p.Id).HasColumnName("UserId");
            ////modelBuilder.Entity<ApplicationUsers>().ToTable("MyUsers").Property(p => p.Id).HasColumnName("UserId");

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //modelBuilder.Entity<TradeTransactionQueue>().HasKey(e => new { e.Id, e.TrnNo }); // komal 04-10-2018 composite primary key
            modelBuilder.Entity<TradePoolMaster>().HasKey(e => new {e.Id ,e.SellServiceID ,e.BuyServiceID ,e.BidPrice }); // komal 11-10-2018 composite primary key

            modelBuilder.Entity<WithdrawHistory>().HasKey(e => new {  e.TrnID, e.Address }); // vsolanki 2018-10-29 composite primary key

            modelBuilder.Entity<DepositCounterMaster>().HasKey(e => new {e.WalletTypeID, e.SerProId}); // Rita 22-10-2018 composite primary key
            modelBuilder.Entity<TradeGraphDetail>().HasKey(e => new { e.Id, e.TranNo }); // Uday 22-10-2018 composite primary key
            modelBuilder.Entity<DepositHistory>().HasKey(e => new { e.TrnID, e.Address }); // ntrivedi 23-10-2018 composite primary key
            modelBuilder.Entity<BizUserTypeMapping>().Property(p =>p.UserID).ValueGeneratedNever(); // ntrivedi 
            modelBuilder.Entity<MemberShadowLimit>().Property(p => p.MemberTypeId).ValueGeneratedNever(); // ntrivedi 
            modelBuilder.Entity<StckingScheme>().Property(p => p.WalletType).ValueGeneratedNever(); // ntrivedi 
            modelBuilder.Entity<WalletLimitConfigurationMaster>().Property(p => p.TrnType).ValueGeneratedNever();
            modelBuilder.Entity<WalletMaster>().Property(x => x.AccWalletID).ValueGeneratedNever();
            modelBuilder.Entity<WalletMaster>().Property(e => e.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore; // ntrivedi https://github.com/aspnet/EntityFrameworkCore/issues/7380 29-10-2018
            modelBuilder.Entity<TradeBuyRequest>().HasKey(e => new { e.Id, e.TrnNo });
            modelBuilder.Entity<ChargeRuleMaster>().HasKey(e => new { e.TrnType, e.WalletType });
            modelBuilder.Entity<LimitRuleMaster>().HasKey(e => new { e.TrnType, e.WalletType });
            modelBuilder.Entity<TradeDepositCompletedTrn>().HasKey(e => new { e.Address, e.TrnID }); // ntrivedi 30-10-2018 composite primary key
            modelBuilder.Entity<TradeDepositCompletedTrn>().Property(x => x.Address).ValueGeneratedNever(); // ntrivedi 30-10-2018 
            modelBuilder.Entity<TradeDepositCompletedTrn>().Property(x => x.TrnID).ValueGeneratedNever(); // ntrivedi 30-10-2018 pk and autoid are different
            modelBuilder.Entity<TradeDepositCompletedTrn>().Property(e => e.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore; // ntrivedi 30102018 https://github.com/aspnet/EntityFrameworkCore/issues/7380 29-10-2018
            //modelBuilder.Entity<TradeBuyerList>().HasKey(e => new {e.Id, e.TrnNo });
            modelBuilder.Entity<TradeSellerList>().HasKey(e => new { e.TrnNo, e.PoolID });            
            modelBuilder.Entity<WalletLimitConfiguration>().HasKey(e => new { e.TrnType, e.WalletId }); // ntrivedi 31102018
            modelBuilder.Entity<WalletLimitConfiguration>().Property(x => x.TrnType).ValueGeneratedNever();// ntrivedi 31102018
            modelBuilder.Entity<UserPreferencesMaster>().Property(x => x.UserID).ValueGeneratedNever();// ntrivedi 31102018
            modelBuilder.Entity<WalletLimitConfiguration>().Property(x => x.WalletId).ValueGeneratedNever();// ntrivedi 31102018
            modelBuilder.Entity<WalletLimitConfiguration>().Property(e => e.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore; // ntrivedi 30102018 https://github.com/aspnet/EntityFrameworkCore/issues/7380 29-10-2018
            modelBuilder.Entity<UserPreferencesMaster>().Property(e => e.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            modelBuilder.Entity<TradeBitGoDelayAddresses>().Property(e => e.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            modelBuilder.Entity<TradeBitGoDelayAddresses>().Property(x => x.TrnID).ValueGeneratedNever();// ntrivedi 31102018
            modelBuilder.Entity<TradeTransactionQueue>().Property(x => x.TrnNo).ValueGeneratedNever();//rita 11-3-2018 ,error : The properties 'TradeTransactionQueue.TrnNo', 'TradeTransactionQueue.Id' are configured to use 'Identity' value generator
            modelBuilder.Entity<TradeBuyerList>().Property(x => x.TrnNo).ValueGeneratedNever();//rita 11-3-2018
            modelBuilder.Entity<TradeBuyerList>().Property(e => e.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;//rita 2-11-2018 for error cannot update identity column id
            modelBuilder.Entity<TradeTransactionQueue>().Property(e => e.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;//rita 2-11-2018 for error cannot update identity column id
            modelBuilder.Entity<TradeSellerList>().Property(e => e.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;//rita 3-11-2018 for error cannot update identity column id
            modelBuilder.Entity<TransactionStatus>().HasKey(e => new { e.TrnNo, e.ServiceID,e.SerProID });//Rita 31-10-2018 for composite Primary key

        }

        /// <summary>
        /// Override SaveChanges so we can call the new AuditEntities method.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            this.AuditEntities();
            return base.SaveChanges();
        }
        /// <summary>
        /// Override SaveChangesAsync so we can call the new AuditEntities method.
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.AuditEntities();

            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Method that will set the Audit properties for every added or modified Entity marked with the 
        /// IAuditable interface.
        /// </summary>
        private void AuditEntities()
        {

            DateTime now = DateTime.Now;
            // Get the authenticated user name 
            string userName = _userService.GetUser();

            // For every changed entity marked as IAditable set the values for the audit properties
            foreach (EntityEntry<IAuditable> entry in ChangeTracker.Entries<IAuditable>())
            {
                // If the entity was added.
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedBy").CurrentValue = userName;
                    entry.Property("CreatedAt").CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified) // If the entity was updated
                {
                    entry.Property("UpdatedBy").CurrentValue = userName;
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
            }
        }


        #region  Comment
        /*

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-5JVOHDJQ\\SQLEXPRESS;Initial Catalog=CleanArchitecture;Persist Security Info=True;User ID=sa;Password=admin_1");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUserPhotos>(entity =>
        {
            entity.HasIndex(e => e.ApplicationUserId)
                .IsUnique();

            entity.HasOne(d => d.ApplicationUser)
                .WithOne(p => p.ApplicationUserPhotos)
                .HasForeignKey<ApplicationUserPhotos>(d => d.ApplicationUserId)
                .HasConstraintName("FK_ApplicationUserPhotos_AspNetUsers_ApplicationUserId");
        });

        modelBuilder.Entity<OpenIddictApplications>(entity =>
        {
            entity.HasIndex(e => e.ClientId)
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.ClientId).IsRequired();

            entity.Property(e => e.Type).IsRequired();
        });

        modelBuilder.Entity<OpenIddictAuthorizations>(entity =>
        {
            entity.HasIndex(e => e.ApplicationId);

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Status).IsRequired();

            entity.Property(e => e.Subject).IsRequired();

            entity.Property(e => e.Type).IsRequired();

            entity.HasOne(d => d.Application)
                .WithMany(p => p.OpenIddictAuthorizations)
                .HasForeignKey(d => d.ApplicationId);
        });

        modelBuilder.Entity<OpenIddictScopes>(entity =>
        {
            entity.HasIndex(e => e.Name)
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<OpenIddictTokens>(entity =>
        {
            entity.HasIndex(e => e.ApplicationId);

            entity.HasIndex(e => e.AuthorizationId);

            entity.HasIndex(e => e.ReferenceId)
                .IsUnique()
                .HasFilter("([ReferenceId] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Subject).IsRequired();

            entity.Property(e => e.Type).IsRequired();

            entity.HasOne(d => d.Application)
                .WithMany(p => p.OpenIddictTokens)
                .HasForeignKey(d => d.ApplicationId);

            entity.HasOne(d => d.Authorization)
                .WithMany(p => p.OpenIddictTokens)
                .HasForeignKey(d => d.AuthorizationId);
        });

        modelBuilder.Entity<Resources>(entity =>
        {
            entity.HasIndex(e => e.CultureId);

            entity.HasOne(d => d.Culture)
                .WithMany(p => p.Resources)
                .HasForeignKey(d => d.CultureId);
        });

        modelBuilder.Entity<RoleClaims>(entity =>
        {
            entity.HasIndex(e => e.RoleId)
                .HasName("IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.RoleClaims)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName)
                .HasName("RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Description).HasMaxLength(250);

            entity.Property(e => e.Name).HasMaxLength(256);

            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<UserClaims>(entity =>
        {
            entity.HasIndex(e => e.UserId)
                .HasName("IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserClaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
        });

        modelBuilder.Entity<UserLogins>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId)
                .HasName("IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
        });

        modelBuilder.Entity<UserRoles>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasIndex(e => e.RoleId)
                .HasName("IX_AspNetUserRoles_RoleId");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail)
                .HasName("EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName)
                .HasName("UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);

            entity.Property(e => e.FirstName).HasMaxLength(250);

            entity.Property(e => e.LastName).HasMaxLength(250);

            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<UserTokens>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
        });
    }
    */

        #endregion
    }

   
}
