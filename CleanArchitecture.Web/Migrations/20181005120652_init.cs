using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Web.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressMasters",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    WalletId = table.Column<long>(nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: false),
                    IsDefaultAddress = table.Column<byte>(nullable: false),
                    SerProID = table.Column<long>(nullable: false),
                    CoinName = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BizRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BizUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 250, nullable: true),
                    LastName = table.Column<string>(maxLength: 250, nullable: true),
                    Mobile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommAPIServiceMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    APID = table.Column<long>(nullable: false),
                    CommServiceID = table.Column<long>(nullable: false),
                    SenderID = table.Column<string>(maxLength: 60, nullable: false),
                    SMSSendURL = table.Column<string>(maxLength: 200, nullable: false),
                    SMSBalURL = table.Column<string>(maxLength: 200, nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommAPIServiceMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommServiceMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    CommServiceID = table.Column<long>(nullable: false),
                    RequestID = table.Column<long>(nullable: false),
                    CommSerproID = table.Column<long>(nullable: false),
                    ServiceName = table.Column<string>(maxLength: 60, nullable: false),
                    ResponseSuccess = table.Column<string>(nullable: true),
                    ResponseFailure = table.Column<string>(nullable: true),
                    ParsingDataID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommServiceMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommServiceproviderMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    CommSerproID = table.Column<long>(nullable: false),
                    CommServiceTypeID = table.Column<long>(nullable: false),
                    SerproName = table.Column<string>(maxLength: 60, nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommServiceproviderMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommServiceTypeMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    CommServiceTypeID = table.Column<long>(nullable: false),
                    ServiceTypeID = table.Column<long>(nullable: false),
                    CommServiceTypeName = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommServiceTypeMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cultures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositHistorys",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TrnID = table.Column<string>(maxLength: 100, nullable: false),
                    SMSCode = table.Column<string>(nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: false),
                    Confirmations = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    StatusMsg = table.Column<string>(maxLength: 100, nullable: false),
                    TimeEpoch = table.Column<string>(nullable: false),
                    ConfirmedTime = table.Column<string>(nullable: false),
                    EpochTimePure = table.Column<string>(nullable: false),
                    OrderID = table.Column<long>(nullable: false),
                    IsProcessing = table.Column<byte>(nullable: false),
                    FromAddress = table.Column<string>(maxLength: 50, nullable: false),
                    APITopUpRefNo = table.Column<string>(nullable: true),
                    SystemRemarks = table.Column<string>(nullable: true),
                    RouteTag = table.Column<string>(nullable: true),
                    SerProID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositHistorys", x => x.TrnID);
                });

            migrationBuilder.CreateTable(
                name: "EmailQueue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Recepient = table.Column<string>(maxLength: 50, nullable: false),
                    Body = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(maxLength: 50, nullable: false),
                    CC = table.Column<string>(maxLength: 500, nullable: true),
                    BCC = table.Column<string>(maxLength: 500, nullable: true),
                    Attachment = table.Column<string>(maxLength: 500, nullable: true),
                    SendBy = table.Column<short>(nullable: false),
                    EmailType = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessagingQueue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    MobileNo = table.Column<long>(nullable: false),
                    SMSText = table.Column<string>(maxLength: 200, nullable: false),
                    RespText = table.Column<string>(maxLength: 1000, nullable: true),
                    SMSServiceID = table.Column<short>(nullable: false),
                    SMSSendBy = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagingQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModeType = table.Column<string>(maxLength: 50, nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationQueue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Subject = table.Column<string>(maxLength: 50, nullable: true),
                    Message = table.Column<string>(maxLength: 200, nullable: false),
                    DeviceID = table.Column<string>(maxLength: 500, nullable: false),
                    TickerText = table.Column<string>(maxLength: 200, nullable: false),
                    ContentTitle = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictApplications",
                columns: table => new
                {
                    ClientId = table.Column<string>(nullable: false),
                    ClientSecret = table.Column<string>(nullable: true),
                    ConcurrencyToken = table.Column<string>(nullable: true),
                    ConsentType = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: false),
                    Permissions = table.Column<string>(nullable: true),
                    PostLogoutRedirectUris = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    RedirectUris = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictScopes",
                columns: table => new
                {
                    ConcurrencyToken = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Properties = table.Column<string>(nullable: true),
                    Resources = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtpMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RegTypeId = table.Column<int>(nullable: false),
                    OTP = table.Column<string>(maxLength: 6, nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ExpirTime = table.Column<DateTime>(nullable: false),
                    EnableStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 30, nullable: false),
                    ServiceID = table.Column<long>(nullable: false),
                    StateID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProviderConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    SerProName = table.Column<string>(maxLength: 30, nullable: true),
                    AppType = table.Column<short>(nullable: false),
                    ThirPartyAPIID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisterType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    ActiveStatus = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestFormatMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    RequestID = table.Column<long>(maxLength: 60, nullable: false),
                    contentType = table.Column<string>(maxLength: 60, nullable: false),
                    MethodType = table.Column<string>(maxLength: 20, nullable: false),
                    RequestFormat = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestFormatMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    RouteName = table.Column<string>(maxLength: 30, nullable: false),
                    ServiceID = table.Column<long>(nullable: false),
                    SerProID = table.Column<long>(nullable: false),
                    ProductID = table.Column<long>(nullable: false),
                    Priority = table.Column<short>(nullable: false),
                    StatusCheckUrl = table.Column<string>(nullable: true),
                    ValidationUrl = table.Column<string>(nullable: true),
                    TransactionUrl = table.Column<string>(nullable: true),
                    MinimumAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MaximumAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    OpCode = table.Column<string>(maxLength: 50, nullable: true),
                    TrnType = table.Column<int>(nullable: false),
                    IsDelayAddress = table.Column<byte>(nullable: false),
                    ProviderWalletID = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ServiceName = table.Column<string>(maxLength: 30, nullable: true),
                    SMSCode = table.Column<string>(maxLength: 10, nullable: true),
                    ServiceType = table.Column<short>(nullable: false),
                    MinimumAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MaximumAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypeMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ServiceTypeID = table.Column<long>(nullable: false),
                    ServiceTypeName = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypeMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    TemplateID = table.Column<long>(nullable: false),
                    CommServiceTypeID = table.Column<long>(nullable: false),
                    TemplateName = table.Column<string>(maxLength: 50, nullable: false),
                    Content = table.Column<string>(maxLength: 1024, nullable: false),
                    AdditionalInfo = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempOtpMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RegTypeId = table.Column<int>(nullable: false),
                    OTP = table.Column<string>(maxLength: 6, nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ExpirTime = table.Column<DateTime>(nullable: false),
                    EnableStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempOtpMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempUserRegister",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    RegTypeId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStemp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 250, nullable: true),
                    LastName = table.Column<string>(maxLength: 250, nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    RegisterStatus = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempUserRegister", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThirdPartyAPIConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    APIName = table.Column<string>(maxLength: 30, nullable: false),
                    APISendURL = table.Column<string>(nullable: false),
                    APIValidateURL = table.Column<string>(nullable: true),
                    APIBalURL = table.Column<string>(nullable: true),
                    APIStatusCheckURL = table.Column<string>(nullable: true),
                    APIRequestBody = table.Column<string>(nullable: true),
                    TransactionIdPrefix = table.Column<string>(nullable: true),
                    MerchantCode = table.Column<string>(nullable: true),
                    UserID = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    ResponseSuccess = table.Column<string>(nullable: true),
                    ResponseFailure = table.Column<string>(nullable: true),
                    ResponseHold = table.Column<string>(nullable: true),
                    AuthHeader = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    MethodType = table.Column<string>(nullable: true),
                    HashCode = table.Column<string>(nullable: true),
                    HashCodeRecheck = table.Column<string>(nullable: true),
                    HashType = table.Column<short>(nullable: false),
                    AppType = table.Column<short>(nullable: false),
                    ParsingDataID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirdPartyAPIConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThirPartyAPIResponseConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    BalanceRegex = table.Column<string>(nullable: true),
                    StatusRegex = table.Column<string>(nullable: true),
                    StatusMsgRegex = table.Column<string>(nullable: true),
                    ResponseCodeRegex = table.Column<string>(nullable: true),
                    ErrorCodeRegex = table.Column<string>(nullable: true),
                    TrnRefNoRegex = table.Column<string>(nullable: true),
                    OprTrnRefNoRegex = table.Column<string>(nullable: true),
                    Param1Regex = table.Column<string>(nullable: true),
                    Param2Regex = table.Column<string>(nullable: true),
                    Param3Regex = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirPartyAPIResponseConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToDoItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDone = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeBitGoDelayAddressess",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    WalletId = table.Column<long>(nullable: false),
                    WalletTypeId = table.Column<long>(nullable: false),
                    TrnID = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: false),
                    GenerateBit = table.Column<byte>(nullable: false),
                    CoinName = table.Column<string>(maxLength: 5, nullable: false),
                    BitgoWalletId = table.Column<string>(maxLength: 100, nullable: false),
                    CoinSpecific = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeBitGoDelayAddressess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    BatchNo = table.Column<long>(nullable: false),
                    RefNo = table.Column<long>(nullable: false),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    WalletID = table.Column<long>(nullable: false),
                    CrAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    DrAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    Remarks = table.Column<string>(maxLength: 150, nullable: false),
                    IsSettled = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionQueue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    TrnMode = table.Column<short>(nullable: false),
                    TrnType = table.Column<short>(nullable: false),
                    MemberID = table.Column<long>(nullable: false),
                    MemberMobile = table.Column<string>(nullable: false),
                    SMSCode = table.Column<string>(maxLength: 10, nullable: false),
                    TransactionAccount = table.Column<string>(maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    ServiceID = table.Column<long>(nullable: false),
                    SerProID = table.Column<long>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    RoutID = table.Column<int>(nullable: false),
                    StatusCode = table.Column<short>(nullable: false),
                    StatusMsg = table.Column<string>(nullable: true),
                    VerifyDone = table.Column<short>(nullable: false),
                    TrnRefNo = table.Column<string>(nullable: true),
                    AdditionalInfo = table.Column<string>(nullable: true),
                    ChargePer = table.Column<decimal>(type: "decimal(18, 8)", nullable: true),
                    ChargeRs = table.Column<decimal>(type: "decimal(18, 8)", nullable: true),
                    ChargeType = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletLedgers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    WalletMasterId = table.Column<long>(nullable: false),
                    ToWalletMasterId = table.Column<long>(nullable: false),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    ServiceTypeID = table.Column<int>(nullable: false),
                    TrnType = table.Column<int>(nullable: false),
                    TrnNo = table.Column<long>(nullable: false),
                    CrAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    DrAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    PreBal = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    PostBal = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    Remarks = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletLedgers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletMasters",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Walletname = table.Column<string>(maxLength: 50, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    WalletTypeID = table.Column<long>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    CoinName = table.Column<string>(nullable: false),
                    UserID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletOrders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderType = table.Column<int>(nullable: false),
                    OWalletMasterID = table.Column<long>(nullable: false),
                    DWalletMasterID = table.Column<long>(nullable: false),
                    OrderAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ORemarks = table.Column<string>(maxLength: 100, nullable: false),
                    DeliveryAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    DRemarks = table.Column<string>(nullable: true),
                    DeliveryGivenBy = table.Column<long>(nullable: true),
                    DeliveryGivenDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletTypeMasters",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    WalletTypeName = table.Column<string>(maxLength: 50, nullable: false),
                    Discription = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTypeMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BizRolesClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizRolesClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BizRolesClaims_BizRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BizRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BizUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BizUserClaims_BizUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BizUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BizUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_BizUserLogin_BizUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BizUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BizUserPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentType = table.Column<string>(nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    ApplicationUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizUserPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BizUserPhotos_BizUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "BizUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BizUserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_BizUserRole_BizRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BizRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BizUserRole_BizUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BizUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BizUserToken",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_BizUserToken_BizUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BizUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    CultureId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoginLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    IPAddress = table.Column<string>(maxLength: 15, nullable: false),
                    DeviceID = table.Column<string>(maxLength: 20, nullable: false),
                    ModeId = table.Column<int>(nullable: true),
                    HostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginLog_Mode_ModeId",
                        column: x => x.ModeId,
                        principalTable: "Mode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictAuthorizations",
                columns: table => new
                {
                    ApplicationId = table.Column<string>(nullable: true),
                    ConcurrencyToken = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: false),
                    Properties = table.Column<string>(nullable: true),
                    Scopes = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictTokens",
                columns: table => new
                {
                    ApplicationId = table.Column<string>(nullable: true),
                    AuthorizationId = table.Column<string>(nullable: true),
                    ConcurrencyToken = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(nullable: true),
                    ExpirationDate = table.Column<DateTimeOffset>(nullable: true),
                    Id = table.Column<string>(nullable: false),
                    Payload = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    ReferenceId = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "OpenIddictAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "BizRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BizRolesClaims_RoleId",
                table: "BizRolesClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "BizUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "BizUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BizUserClaims_UserId",
                table: "BizUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BizUserLogin_UserId",
                table: "BizUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BizUserPhotos_ApplicationUserId",
                table: "BizUserPhotos",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BizUserRole_RoleId",
                table: "BizUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginLog_ModeId",
                table: "LoginLog",
                column: "ModeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_ClientId",
                table: "OpenIddictApplications",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictAuthorizations_ApplicationId",
                table: "OpenIddictAuthorizations",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictScopes_Name",
                table: "OpenIddictScopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ApplicationId",
                table: "OpenIddictTokens",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_AuthorizationId",
                table: "OpenIddictTokens",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ReferenceId",
                table: "OpenIddictTokens",
                column: "ReferenceId",
                unique: true,
                filter: "[ReferenceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_CultureId",
                table: "Resources",
                column: "CultureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressMasters");

            migrationBuilder.DropTable(
                name: "BizRolesClaims");

            migrationBuilder.DropTable(
                name: "BizUserClaims");

            migrationBuilder.DropTable(
                name: "BizUserLogin");

            migrationBuilder.DropTable(
                name: "BizUserPhotos");

            migrationBuilder.DropTable(
                name: "BizUserRole");

            migrationBuilder.DropTable(
                name: "BizUserToken");

            migrationBuilder.DropTable(
                name: "CommAPIServiceMaster");

            migrationBuilder.DropTable(
                name: "CommServiceMaster");

            migrationBuilder.DropTable(
                name: "CommServiceproviderMaster");

            migrationBuilder.DropTable(
                name: "CommServiceTypeMaster");

            migrationBuilder.DropTable(
                name: "DepositHistorys");

            migrationBuilder.DropTable(
                name: "EmailQueue");

            migrationBuilder.DropTable(
                name: "LoginLog");

            migrationBuilder.DropTable(
                name: "MessagingQueue");

            migrationBuilder.DropTable(
                name: "NotificationQueue");

            migrationBuilder.DropTable(
                name: "OpenIddictScopes");

            migrationBuilder.DropTable(
                name: "OpenIddictTokens");

            migrationBuilder.DropTable(
                name: "OtpMaster");

            migrationBuilder.DropTable(
                name: "ProductConfiguration");

            migrationBuilder.DropTable(
                name: "ProviderConfiguration");

            migrationBuilder.DropTable(
                name: "RegisterType");

            migrationBuilder.DropTable(
                name: "RequestFormatMaster");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "RouteConfiguration");

            migrationBuilder.DropTable(
                name: "ServiceConfiguration");

            migrationBuilder.DropTable(
                name: "ServiceTypeMaster");

            migrationBuilder.DropTable(
                name: "TemplateMaster");

            migrationBuilder.DropTable(
                name: "TempOtpMaster");

            migrationBuilder.DropTable(
                name: "TempUserRegister");

            migrationBuilder.DropTable(
                name: "ThirdPartyAPIConfiguration");

            migrationBuilder.DropTable(
                name: "ThirPartyAPIResponseConfiguration");

            migrationBuilder.DropTable(
                name: "ToDoItems");

            migrationBuilder.DropTable(
                name: "TradeBitGoDelayAddressess");

            migrationBuilder.DropTable(
                name: "TransactionAccounts");

            migrationBuilder.DropTable(
                name: "TransactionQueue");

            migrationBuilder.DropTable(
                name: "WalletLedgers");

            migrationBuilder.DropTable(
                name: "WalletMasters");

            migrationBuilder.DropTable(
                name: "WalletOrders");

            migrationBuilder.DropTable(
                name: "WalletTypeMasters");

            migrationBuilder.DropTable(
                name: "BizRoles");

            migrationBuilder.DropTable(
                name: "BizUser");

            migrationBuilder.DropTable(
                name: "Mode");

            migrationBuilder.DropTable(
                name: "OpenIddictAuthorizations");

            migrationBuilder.DropTable(
                name: "Cultures");

            migrationBuilder.DropTable(
                name: "OpenIddictApplications");
        }
    }
}
