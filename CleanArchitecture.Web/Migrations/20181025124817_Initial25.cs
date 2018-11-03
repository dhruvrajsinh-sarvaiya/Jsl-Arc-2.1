using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Web.Migrations
{
    public partial class Initial25 : Migration
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
                    AddressLable = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    AppTypeName = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeneficiaryMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UserID = table.Column<long>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WalletTypeID = table.Column<long>(nullable: false),
                    IsWhiteListed = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficiaryMaster", x => x.Id);
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
                name: "BizUserTypeMapping",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<long>(nullable: false),
                    UserType = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizUserTypeMapping", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "CityMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    CityName = table.Column<string>(maxLength: 30, nullable: false),
                    StateID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityMaster", x => x.Id);
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
                name: "CountryMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    CountryName = table.Column<string>(maxLength: 30, nullable: false),
                    CountryCode = table.Column<string>(maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryMaster", x => x.Id);
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
                name: "CustomPassword",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    EnableStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPassword", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemonConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    IPAdd = table.Column<string>(maxLength: 15, nullable: false),
                    PortAdd = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemonConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositCounterLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    NewTxnID = table.Column<string>(nullable: true),
                    PreviousTrnID = table.Column<string>(nullable: true),
                    LastTrnID = table.Column<string>(nullable: true),
                    LastLimit = table.Column<long>(nullable: false),
                    NextBatchPrvID = table.Column<string>(nullable: true),
                    DepositCounterMasterId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositCounterLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositCounterMaster",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecordCount = table.Column<int>(nullable: false),
                    Limit = table.Column<long>(nullable: false),
                    LastTrnID = table.Column<string>(nullable: true),
                    MaxLimit = table.Column<long>(nullable: false),
                    WalletTypeID = table.Column<long>(nullable: false),
                    SerProId = table.Column<long>(nullable: false),
                    PreviousTrnID = table.Column<string>(nullable: true),
                    prevIterationID = table.Column<string>(nullable: true),
                    TPSPickupStatus = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositCounterMaster", x => new { x.WalletTypeID, x.SerProId });
                    table.UniqueConstraint("AK_DepositCounterMaster_SerProId_WalletTypeID", x => new { x.SerProId, x.WalletTypeID });
                });

            migrationBuilder.CreateTable(
                name: "DepositHistory",
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
                    EpochTimePure = table.Column<string>(nullable: true),
                    OrderID = table.Column<long>(nullable: false),
                    IsProcessing = table.Column<byte>(nullable: false),
                    FromAddress = table.Column<string>(maxLength: 50, nullable: false),
                    APITopUpRefNo = table.Column<string>(nullable: true),
                    SystemRemarks = table.Column<string>(nullable: true),
                    RouteTag = table.Column<string>(nullable: true),
                    SerProID = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositHistory", x => new { x.TrnID, x.Address });
                    table.UniqueConstraint("AK_DepositHistory_Address_TrnID", x => new { x.Address, x.TrnID });
                });

            migrationBuilder.CreateTable(
                name: "DeviceMaster",
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
                    DeviceId = table.Column<string>(maxLength: 2000, nullable: false),
                    IsEnable = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMaster", x => x.Id);
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
                name: "IpMaster",
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
                    IpAddress = table.Column<string>(maxLength: 15, nullable: false),
                    IsEnable = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Limits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    MinAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MaxAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MinAmtDaily = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MaxAmtDaily = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MinAmtWeekly = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MaxAmtWeekly = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MinAmtMonthly = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MaxAmtMonthly = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MinRange = table.Column<long>(nullable: false),
                    Maxrange = table.Column<long>(nullable: false),
                    MinRangeDaily = table.Column<long>(nullable: false),
                    MaxRangeDaily = table.Column<long>(nullable: false),
                    MinRangeWeekly = table.Column<long>(nullable: false),
                    MaxRangeWeekly = table.Column<long>(nullable: false),
                    MinRangeMonthly = table.Column<long>(nullable: false),
                    MaxRangeMonthly = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Limits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Market",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    CurrencyName = table.Column<string>(nullable: false),
                    isBaseCurrency = table.Column<short>(nullable: false),
                    ServiceID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Market", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberShadowBalance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    MemberShadowLimitId = table.Column<long>(nullable: false),
                    WalletID = table.Column<long>(nullable: false),
                    ShadowAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    WalletTypeId = table.Column<long>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberShadowBalance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberShadowLimit",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    MemberTypeId = table.Column<long>(nullable: false),
                    ShadowLimitAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    WalletType = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberShadowLimit", x => x.Id);
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
                name: "PoolOrder",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    TrnMode = table.Column<byte>(nullable: false),
                    OMemberID = table.Column<long>(nullable: false),
                    PayMode = table.Column<byte>(nullable: false),
                    OrderAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    DiscPer = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    DiscRs = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    OBankID = table.Column<short>(nullable: false),
                    OBranchName = table.Column<string>(nullable: true),
                    OAccountNo = table.Column<string>(nullable: true),
                    OChequeNo = table.Column<string>(nullable: true),
                    OChequeDate = table.Column<DateTime>(nullable: false),
                    DMemberID = table.Column<long>(nullable: false),
                    DBankID = table.Column<short>(nullable: false),
                    DAccountNo = table.Column<string>(nullable: false),
                    ORemarks = table.Column<string>(nullable: true),
                    DeliveryAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    DRemarks = table.Column<string>(nullable: true),
                    DeliveryGivenBy = table.Column<long>(nullable: false),
                    DeliveryGivenDate = table.Column<DateTime>(nullable: false),
                    AlertRec = table.Column<byte>(nullable: false),
                    CashChargePer = table.Column<double>(nullable: false),
                    CashChargeRs = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    WalletAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    PGId = table.Column<int>(nullable: false),
                    CouponNo = table.Column<long>(nullable: false),
                    IsChargeAccepted = table.Column<bool>(nullable: false),
                    IsDebited = table.Column<bool>(nullable: false),
                    WalletID = table.Column<long>(nullable: false),
                    TrnNo = table.Column<long>(nullable: false),
                    CancelID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolOrder", x => x.Id);
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
                    CountryID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductConfiguration", x => x.Id);
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
                    SerProDetailID = table.Column<long>(nullable: false),
                    ProductID = table.Column<long>(nullable: false),
                    Priority = table.Column<short>(nullable: false),
                    StatusCheckUrl = table.Column<string>(nullable: true),
                    ValidationUrl = table.Column<string>(nullable: true),
                    TransactionUrl = table.Column<string>(nullable: true),
                    LimitId = table.Column<long>(nullable: false),
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
                name: "ServiceDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ServiceId = table.Column<long>(nullable: false),
                    ServiceDetailJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    SMSCode = table.Column<string>(maxLength: 5, nullable: false),
                    ServiceType = table.Column<short>(nullable: false),
                    LimitId = table.Column<long>(nullable: false),
                    WalletTypeID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    AppKey = table.Column<string>(maxLength: 50, nullable: false),
                    APIKey = table.Column<string>(maxLength: 50, nullable: false),
                    SecretKey = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProviderDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ServiceProID = table.Column<long>(nullable: false),
                    ProTypeID = table.Column<long>(nullable: false),
                    AppTypeID = table.Column<long>(nullable: false),
                    TrnTypeID = table.Column<long>(nullable: false),
                    LimitID = table.Column<long>(nullable: false),
                    DemonConfigID = table.Column<long>(nullable: false),
                    ServiceProConfigID = table.Column<long>(nullable: false),
                    ThirPartyAPIID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviderDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProviderMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ProviderName = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviderMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProviderType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ServiveProTypeName = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceStastics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ServiceId = table.Column<long>(nullable: false),
                    MarketCap = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    VolGlobal = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MaxSupply = table.Column<long>(nullable: false),
                    CirculatingSupply = table.Column<long>(nullable: false),
                    IssuePrice = table.Column<decimal>(nullable: false),
                    IssueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceStastics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypeMapping",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ServiceId = table.Column<long>(nullable: false),
                    TrnType = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypeMapping", x => x.Id);
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
                name: "StateMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    StateName = table.Column<string>(maxLength: 30, nullable: false),
                    StateCode = table.Column<string>(maxLength: 2, nullable: false),
                    CountryID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StckingScheme",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    MaxLimitAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MinLimitAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    WalletType = table.Column<long>(nullable: false),
                    TimePeriod = table.Column<string>(nullable: false),
                    Percent = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StckingScheme", x => x.Id);
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
                    SerProConfigurationID = table.Column<long>(nullable: false),
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
                name: "ThirdPartyAPIResponseConfiguration",
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
                    table.PrimaryKey("PK_ThirdPartyAPIResponseConfiguration", x => x.Id);
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
                name: "TradeBuyRequest",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Id = table.Column<long>(nullable: false),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    PickupDate = table.Column<DateTime>(nullable: false),
                    MemberID = table.Column<long>(nullable: false),
                    TrnNo = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PairID = table.Column<long>(nullable: false),
                    ServiceID = table.Column<long>(nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    BidPrice = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    PaidQty = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    PaidServiceID = table.Column<long>(nullable: false),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    PendingQty = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    IsCancel = table.Column<short>(nullable: false),
                    IsPartialProceed = table.Column<short>(nullable: false),
                    IsProcessing = table.Column<short>(nullable: false),
                    SellStockID = table.Column<long>(nullable: false),
                    BuyStockID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeBuyRequest", x => x.TrnNo);
                });

            migrationBuilder.CreateTable(
                name: "TradeCancelQueue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    TrnNo = table.Column<long>(nullable: false),
                    DeliverServiceID = table.Column<long>(nullable: false),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    PendingBuyQty = table.Column<decimal>(nullable: false),
                    DeliverQty = table.Column<decimal>(nullable: false),
                    OrderType = table.Column<short>(nullable: true),
                    DeliverBidPrice = table.Column<decimal>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    StatusMsg = table.Column<string>(nullable: false),
                    OrderID = table.Column<long>(nullable: false),
                    SettledDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeCancelQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeGraphDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    PairId = table.Column<long>(nullable: false),
                    DataDate = table.Column<DateTime>(nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    High24Hr = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    Low24Hr = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    TodayClose = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    TodayOpen = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    HighWeek = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    LowWeek = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    High52Week = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    Low52Week = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    LTP = table.Column<decimal>(type: "decimal(18, 8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeGraphDetail", x => new { x.Id, x.PairId, x.DataDate });
                    table.UniqueConstraint("AK_TradeGraphDetail_DataDate_Id_PairId", x => new { x.DataDate, x.Id, x.PairId });
                });

            migrationBuilder.CreateTable(
                name: "TradePairDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    PairId = table.Column<long>(nullable: false),
                    BuyMinQty = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    BuyMaxQty = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    SellMinQty = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    SellMaxQty = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    BuyMinPrice = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    BuyMaxPrice = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    SellMinPrice = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    SellMaxPrice = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    BuyFees = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    SellFees = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    FeesCurrency = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradePairDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradePairMaster",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Id = table.Column<long>(nullable: false),
                    PairName = table.Column<string>(nullable: false),
                    SecondaryCurrencyId = table.Column<long>(nullable: false),
                    WalletMasterID = table.Column<long>(nullable: false),
                    BaseCurrencyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradePairMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradePairStastics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    PairId = table.Column<long>(nullable: false),
                    CurrentRate = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    LTP = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    ChangePer24 = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    ChangeVol24 = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    High24Hr = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    Low24Hr = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    HighWeek = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    LowWeek = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    High52Week = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    Low52Week = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    CurrencyPrice = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    UpDownBit = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradePairStastics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradePoolMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    PairName = table.Column<string>(maxLength: 50, nullable: false),
                    ProductID = table.Column<long>(nullable: false),
                    SellServiceID = table.Column<long>(nullable: false),
                    BuyServiceID = table.Column<long>(nullable: false),
                    BidPrice = table.Column<long>(nullable: false),
                    CountPerPrice = table.Column<long>(nullable: false),
                    TotalQty = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    Landing = table.Column<decimal>(type: "decimal(37, 16)", nullable: false),
                    OnProcessing = table.Column<short>(nullable: false),
                    TPSPickupStatus = table.Column<short>(nullable: false),
                    IsSleepMode = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradePoolMaster", x => new { x.Id, x.SellServiceID, x.BuyServiceID, x.BidPrice });
                    table.UniqueConstraint("AK_TradePoolMaster_BidPrice_BuyServiceID_CountPerPrice_Id_SellServiceID", x => new { x.BidPrice, x.BuyServiceID, x.CountPerPrice, x.Id, x.SellServiceID });
                });

            migrationBuilder.CreateTable(
                name: "TradeStopLoss",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    TrnNo = table.Column<long>(nullable: false),
                    ordertype = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeStopLoss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeTransactionQueue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    TrnNo = table.Column<long>(nullable: false),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    MemberID = table.Column<long>(nullable: false),
                    TrnType = table.Column<short>(nullable: false),
                    TrnTypeName = table.Column<string>(nullable: true),
                    PairID = table.Column<long>(nullable: false),
                    PairName = table.Column<string>(nullable: false),
                    OrderWalletID = table.Column<long>(nullable: false),
                    DeliveryWalletID = table.Column<long>(nullable: false),
                    BuyQty = table.Column<decimal>(nullable: false),
                    BidPrice = table.Column<decimal>(nullable: false),
                    SellQty = table.Column<decimal>(nullable: false),
                    AskPrice = table.Column<decimal>(nullable: false),
                    Order_Currency = table.Column<string>(nullable: true),
                    OrderTotalQty = table.Column<decimal>(nullable: false),
                    Delivery_Currency = table.Column<string>(nullable: true),
                    DeliveryTotalQty = table.Column<decimal>(nullable: false),
                    StatusCode = table.Column<long>(nullable: false),
                    StatusMsg = table.Column<string>(nullable: true),
                    ServiceID = table.Column<long>(nullable: false),
                    ProductID = table.Column<long>(nullable: false),
                    SerProID = table.Column<long>(nullable: false),
                    RoutID = table.Column<int>(nullable: false),
                    TrnRefNo = table.Column<long>(nullable: true),
                    IsCancelled = table.Column<short>(nullable: false),
                    SettledBuyQty = table.Column<decimal>(nullable: false),
                    SettledSellQty = table.Column<decimal>(nullable: false),
                    SettledDate = table.Column<DateTime>(nullable: true),
                    TakerPer = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeTransactionQueue", x => new { x.Id, x.TrnNo });
                });

            migrationBuilder.CreateTable(
                name: "TradeTransactionStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    TrnNo = table.Column<long>(nullable: false),
                    SettledQty = table.Column<decimal>(nullable: false),
                    TotalQty = table.Column<decimal>(nullable: false),
                    DeliveredQty = table.Column<decimal>(nullable: false),
                    PendingQty = table.Column<decimal>(nullable: false),
                    SoldPrice = table.Column<decimal>(nullable: false),
                    BidPrice = table.Column<decimal>(nullable: false),
                    OrderID = table.Column<long>(nullable: false),
                    StockID = table.Column<long>(nullable: false),
                    SellStockID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeTransactionStatus", x => x.Id);
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
                    GUID = table.Column<Guid>(nullable: false),
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
                    ProductID = table.Column<long>(nullable: false),
                    RouteID = table.Column<long>(nullable: false),
                    StatusCode = table.Column<long>(nullable: false),
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
                name: "TrnAcBatch",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnAcBatch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferencesMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UserID = table.Column<long>(nullable: false),
                    IsWhitelisting = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferencesMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStacking",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    SchemeId = table.Column<long>(nullable: false),
                    WalletId = table.Column<long>(nullable: false),
                    StackingAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    WalletType = table.Column<string>(maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStacking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletAllowTrns",
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
                    TrnType = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletAllowTrns", x => x.Id);
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
                    WalletId = table.Column<long>(nullable: false),
                    ToWalletId = table.Column<long>(nullable: false),
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
                name: "WalletLimitConfiguration",
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
                    TrnType = table.Column<int>(nullable: false),
                    LimitPerHour = table.Column<decimal>(nullable: false),
                    LimitPerDay = table.Column<decimal>(nullable: false),
                    LimitPerTransaction = table.Column<decimal>(nullable: false),
                    LifeTime = table.Column<decimal>(nullable: true),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletLimitConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletMasters",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Walletname = table.Column<string>(maxLength: 50, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    WalletTypeID = table.Column<long>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    AccWalletID = table.Column<string>(maxLength: 16, nullable: false),
                    UserID = table.Column<long>(nullable: false),
                    PublicAddress = table.Column<string>(maxLength: 50, nullable: false),
                    IsDefaultWallet = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletMasters", x => x.AccWalletID);
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
                name: "WalletTransactionOrders",
                columns: table => new
                {
                    OrderID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    OWalletID = table.Column<long>(nullable: false),
                    DWalletID = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    WalletType = table.Column<string>(maxLength: 5, nullable: false),
                    OTrnNo = table.Column<long>(nullable: false),
                    DTrnNo = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StatusMsg = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactionOrders", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactionQueues",
                columns: table => new
                {
                    TrnNo = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<Guid>(maxLength: 50, nullable: false),
                    TrnType = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    TrnRefNo = table.Column<long>(nullable: false),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    WalletID = table.Column<long>(nullable: false),
                    WalletType = table.Column<string>(maxLength: 5, nullable: false),
                    MemberID = table.Column<long>(nullable: false),
                    TimeStamp = table.Column<string>(maxLength: 50, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StatusMsg = table.Column<string>(maxLength: 50, nullable: false),
                    SettedAmt = table.Column<decimal>(type: "decimal(18, 8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactionQueues", x => x.TrnNo);
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
                    Discription = table.Column<string>(maxLength: 100, nullable: false),
                    IsDepositionAllow = table.Column<short>(nullable: false),
                    IsWithdrawalAllow = table.Column<short>(nullable: false),
                    IsTransactionWallet = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTypeMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZipCodeMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ZipCode = table.Column<long>(nullable: false),
                    ZipAreaName = table.Column<string>(maxLength: 30, nullable: false),
                    CityID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipCodeMaster", x => x.Id);
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
                name: "AppType");

            migrationBuilder.DropTable(
                name: "BeneficiaryMaster");

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
                name: "BizUserTypeMapping");

            migrationBuilder.DropTable(
                name: "CityMaster");

            migrationBuilder.DropTable(
                name: "CommAPIServiceMaster");

            migrationBuilder.DropTable(
                name: "CommServiceMaster");

            migrationBuilder.DropTable(
                name: "CommServiceproviderMaster");

            migrationBuilder.DropTable(
                name: "CommServiceTypeMaster");

            migrationBuilder.DropTable(
                name: "CountryMaster");

            migrationBuilder.DropTable(
                name: "CustomPassword");

            migrationBuilder.DropTable(
                name: "DemonConfiguration");

            migrationBuilder.DropTable(
                name: "DepositCounterLog");

            migrationBuilder.DropTable(
                name: "DepositCounterMaster");

            migrationBuilder.DropTable(
                name: "DepositHistory");

            migrationBuilder.DropTable(
                name: "DeviceMaster");

            migrationBuilder.DropTable(
                name: "EmailQueue");

            migrationBuilder.DropTable(
                name: "IpMaster");

            migrationBuilder.DropTable(
                name: "Limits");

            migrationBuilder.DropTable(
                name: "LoginLog");

            migrationBuilder.DropTable(
                name: "Market");

            migrationBuilder.DropTable(
                name: "MemberShadowBalance");

            migrationBuilder.DropTable(
                name: "MemberShadowLimit");

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
                name: "PoolOrder");

            migrationBuilder.DropTable(
                name: "ProductConfiguration");

            migrationBuilder.DropTable(
                name: "RegisterType");

            migrationBuilder.DropTable(
                name: "RequestFormatMaster");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "RouteConfiguration");

            migrationBuilder.DropTable(
                name: "ServiceDetail");

            migrationBuilder.DropTable(
                name: "ServiceMaster");

            migrationBuilder.DropTable(
                name: "ServiceProConfiguration");

            migrationBuilder.DropTable(
                name: "ServiceProviderDetail");

            migrationBuilder.DropTable(
                name: "ServiceProviderMaster");

            migrationBuilder.DropTable(
                name: "ServiceProviderType");

            migrationBuilder.DropTable(
                name: "ServiceStastics");

            migrationBuilder.DropTable(
                name: "ServiceTypeMapping");

            migrationBuilder.DropTable(
                name: "ServiceTypeMaster");

            migrationBuilder.DropTable(
                name: "StateMaster");

            migrationBuilder.DropTable(
                name: "StckingScheme");

            migrationBuilder.DropTable(
                name: "TemplateMaster");

            migrationBuilder.DropTable(
                name: "TempOtpMaster");

            migrationBuilder.DropTable(
                name: "TempUserRegister");

            migrationBuilder.DropTable(
                name: "ThirdPartyAPIConfiguration");

            migrationBuilder.DropTable(
                name: "ThirdPartyAPIResponseConfiguration");

            migrationBuilder.DropTable(
                name: "ToDoItems");

            migrationBuilder.DropTable(
                name: "TradeBitGoDelayAddressess");

            migrationBuilder.DropTable(
                name: "TradeBuyRequest");

            migrationBuilder.DropTable(
                name: "TradeCancelQueue");

            migrationBuilder.DropTable(
                name: "TradeGraphDetail");

            migrationBuilder.DropTable(
                name: "TradePairDetail");

            migrationBuilder.DropTable(
                name: "TradePairMaster");

            migrationBuilder.DropTable(
                name: "TradePairStastics");

            migrationBuilder.DropTable(
                name: "TradePoolMaster");

            migrationBuilder.DropTable(
                name: "TradeStopLoss");

            migrationBuilder.DropTable(
                name: "TradeTransactionQueue");

            migrationBuilder.DropTable(
                name: "TradeTransactionStatus");

            migrationBuilder.DropTable(
                name: "TransactionAccounts");

            migrationBuilder.DropTable(
                name: "TransactionQueue");

            migrationBuilder.DropTable(
                name: "TrnAcBatch");

            migrationBuilder.DropTable(
                name: "UserPreferencesMaster");

            migrationBuilder.DropTable(
                name: "UserStacking");

            migrationBuilder.DropTable(
                name: "WalletAllowTrns");

            migrationBuilder.DropTable(
                name: "WalletLedgers");

            migrationBuilder.DropTable(
                name: "WalletLimitConfiguration");

            migrationBuilder.DropTable(
                name: "WalletMasters");

            migrationBuilder.DropTable(
                name: "WalletOrders");

            migrationBuilder.DropTable(
                name: "WalletTransactionOrders");

            migrationBuilder.DropTable(
                name: "WalletTransactionQueues");

            migrationBuilder.DropTable(
                name: "WalletTypeMasters");

            migrationBuilder.DropTable(
                name: "ZipCodeMaster");

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
