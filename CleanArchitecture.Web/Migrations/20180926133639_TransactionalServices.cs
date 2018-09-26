using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Web.Migrations
{
    public partial class TransactionalServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommAPIServiceMaster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    APID = table.Column<long>(nullable: false),
                    CommServiceID = table.Column<long>(nullable: false),
                    SenderID = table.Column<string>(maxLength: 60, nullable: false),
                    SMSSendURL = table.Column<string>(maxLength: 50, nullable: false),
                    SMSBalURL = table.Column<string>(maxLength: 50, nullable: false),
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
                    Status = table.Column<short>(nullable: false),
                    CommServiceID = table.Column<long>(nullable: false),
                    RequestID = table.Column<long>(nullable: false),
                    CommSerproID = table.Column<long>(nullable: false),
                    ServiceName = table.Column<string>(maxLength: 60, nullable: false)
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
                    Status = table.Column<short>(nullable: false),
                    CommSerproID = table.Column<long>(nullable: false),
                    CommServiceTypeID = table.Column<long>(nullable: false),
                    SerproName = table.Column<string>(maxLength: 60, nullable: false),
                    UserID = table.Column<string>(maxLength: 20, nullable: false),
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
                name: "EmailQueue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
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
                name: "NotificationQueue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    MobileNo = table.Column<long>(nullable: false),
                    Message = table.Column<string>(maxLength: 200, nullable: false),
                    DeviceID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationQueue", x => x.Id);
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
                    Status = table.Column<short>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 30, nullable: false),
                    ServceID = table.Column<long>(nullable: false),
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
                    Status = table.Column<short>(nullable: false),
                    SerProName = table.Column<string>(maxLength: 30, nullable: true),
                    AppType = table.Column<short>(nullable: false),
                    APIServiceID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderConfiguration", x => x.Id);
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
                    Status = table.Column<short>(nullable: false),
                    RouteName = table.Column<string>(maxLength: 30, nullable: false),
                    ServceID = table.Column<long>(nullable: false),
                    SerProID = table.Column<long>(nullable: false),
                    ProductID = table.Column<long>(nullable: false),
                    Priority = table.Column<short>(nullable: false),
                    StatusCheckUrl = table.Column<string>(nullable: true),
                    ValidationUrl = table.Column<string>(nullable: true),
                    TransactionUrl = table.Column<string>(nullable: true),
                    MinimumAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    MaximumAmount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    OpCode = table.Column<string>(maxLength: 50, nullable: true)
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
                name: "ThirPartyAPIConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    APIName = table.Column<string>(maxLength: 30, nullable: false),
                    APISendURL = table.Column<string>(nullable: false),
                    APIValidateURL = table.Column<string>(nullable: true),
                    APIBalURL = table.Column<string>(nullable: true),
                    APIStatusCheckURL = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_ThirPartyAPIConfiguration", x => x.Id);
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
                    Status = table.Column<short>(nullable: false),
                    BalanceRegex = table.Column<string>(nullable: true),
                    StatusRegex = table.Column<string>(nullable: true),
                    StatusMsgRegex = table.Column<string>(nullable: true),
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
                name: "TransactionAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
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
                    TrnNo = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    TrnMode = table.Column<short>(nullable: false),
                    TrnType = table.Column<short>(nullable: false),
                    MemberID = table.Column<long>(nullable: false),
                    MemberMobile = table.Column<string>(nullable: false),
                    SMSText = table.Column<string>(nullable: false),
                    SMSCode = table.Column<string>(maxLength: 10, nullable: false),
                    CustomerMobile = table.Column<string>(maxLength: 15, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    SMSPwd = table.Column<string>(maxLength: 4, nullable: false),
                    ServiceID = table.Column<long>(nullable: false),
                    SerProID = table.Column<long>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    RoutID = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    StatusCode = table.Column<short>(nullable: false),
                    StatusMsg = table.Column<string>(nullable: false),
                    VerifyDone = table.Column<short>(nullable: false),
                    TrnRefNo = table.Column<string>(nullable: true),
                    AdditionalInfo = table.Column<string>(nullable: true),
                    ChargePer = table.Column<decimal>(type: "decimal(18, 8)", nullable: true),
                    ChargeRs = table.Column<decimal>(type: "decimal(18, 8)", nullable: true),
                    ChargeType = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionQueue", x => x.TrnNo);
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
                    Status = table.Column<short>(nullable: false),
                    WalletMasterId = table.Column<long>(nullable: false),
                    TrnDate = table.Column<DateTime>(nullable: false),
                    ServiceTypeID = table.Column<int>(nullable: false),
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
                    Status = table.Column<short>(nullable: false),
                    Balance = table.Column<decimal>(nullable: false),
                    WalletTypeID = table.Column<long>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false)
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
                    Status = table.Column<short>(nullable: false),
                    WalletTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTypeMasters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommAPIServiceMaster");

            migrationBuilder.DropTable(
                name: "CommServiceMaster");

            migrationBuilder.DropTable(
                name: "CommServiceproviderMaster");

            migrationBuilder.DropTable(
                name: "CommServiceTypeMaster");

            migrationBuilder.DropTable(
                name: "EmailQueue");

            migrationBuilder.DropTable(
                name: "MessagingQueue");

            migrationBuilder.DropTable(
                name: "NotificationQueue");

            migrationBuilder.DropTable(
                name: "ProductConfiguration");

            migrationBuilder.DropTable(
                name: "ProviderConfiguration");

            migrationBuilder.DropTable(
                name: "RequestFormatMaster");

            migrationBuilder.DropTable(
                name: "RouteConfiguration");

            migrationBuilder.DropTable(
                name: "ServiceConfiguration");

            migrationBuilder.DropTable(
                name: "ServiceTypeMaster");

            migrationBuilder.DropTable(
                name: "TemplateMaster");

            migrationBuilder.DropTable(
                name: "ThirPartyAPIConfiguration");

            migrationBuilder.DropTable(
                name: "ThirPartyAPIResponseConfiguration");

            migrationBuilder.DropTable(
                name: "ToDoItems");

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
        }
    }
}
