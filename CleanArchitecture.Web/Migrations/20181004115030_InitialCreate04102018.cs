using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Web.Migrations
{
    public partial class InitialCreate04102018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempOtpMaster_RegisterType_RegisterTypeId",
                table: "TempOtpMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_TempOtpMaster_BizUser_UserId",
                table: "TempOtpMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_TempUserRegister_RegisterType_RegisterTypeId",
                table: "TempUserRegister");

            migrationBuilder.DropTable(
                name: "ThirPartyAPIConfiguration");

            migrationBuilder.DropIndex(
                name: "IX_TempUserRegister_RegisterTypeId",
                table: "TempUserRegister");

            migrationBuilder.DropIndex(
                name: "IX_TempOtpMaster_RegisterTypeId",
                table: "TempOtpMaster");

            migrationBuilder.DropIndex(
                name: "IX_TempOtpMaster_UserId",
                table: "TempOtpMaster");

            migrationBuilder.DropColumn(
                name: "CustomerMobile",
                table: "TransactionQueue");

            migrationBuilder.DropColumn(
                name: "SMSPwd",
                table: "TransactionQueue");

            migrationBuilder.DropColumn(
                name: "SMSText",
                table: "TransactionQueue");

            migrationBuilder.DropColumn(
                name: "RegisterTypeId",
                table: "TempUserRegister");

            migrationBuilder.DropColumn(
                name: "EnableStatus",
                table: "TempOtpMaster");

            migrationBuilder.DropColumn(
                name: "RegisterTypeId",
                table: "TempOtpMaster");

            migrationBuilder.RenameColumn(
                name: "TrnNo",
                table: "TransactionQueue",
                newName: "Id");

            migrationBuilder.AddColumn<long>(
                name: "UserID",
                table: "WalletMasters",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "StatusMsg",
                table: "TransactionQueue",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "TransactionQueue",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TransactionQueue",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TransactionAccount",
                table: "TransactionQueue",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UpdatedBy",
                table: "TransactionQueue",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "TransactionQueue",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "IsDelayAddress",
                table: "RouteConfiguration",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "ContentTitle",
                table: "NotificationQueue",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TickerText",
                table: "NotificationQueue",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ParsingDataID",
                table: "CommServiceMaster",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ResponseFailure",
                table: "CommServiceMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseSuccess",
                table: "CommServiceMaster",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThirdPartyAPIConfiguration");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "WalletMasters");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TransactionQueue");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TransactionQueue");

            migrationBuilder.DropColumn(
                name: "TransactionAccount",
                table: "TransactionQueue");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TransactionQueue");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "TransactionQueue");

            migrationBuilder.DropColumn(
                name: "IsDelayAddress",
                table: "RouteConfiguration");

            migrationBuilder.DropColumn(
                name: "ContentTitle",
                table: "NotificationQueue");

            migrationBuilder.DropColumn(
                name: "TickerText",
                table: "NotificationQueue");

            migrationBuilder.DropColumn(
                name: "ParsingDataID",
                table: "CommServiceMaster");

            migrationBuilder.DropColumn(
                name: "ResponseFailure",
                table: "CommServiceMaster");

            migrationBuilder.DropColumn(
                name: "ResponseSuccess",
                table: "CommServiceMaster");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TransactionQueue",
                newName: "TrnNo");

            migrationBuilder.AlterColumn<string>(
                name: "StatusMsg",
                table: "TransactionQueue",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerMobile",
                table: "TransactionQueue",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SMSPwd",
                table: "TransactionQueue",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SMSText",
                table: "TransactionQueue",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RegisterTypeId",
                table: "TempUserRegister",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableStatus",
                table: "TempOtpMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "RegisterTypeId",
                table: "TempOtpMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ThirPartyAPIConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    APIBalURL = table.Column<string>(nullable: true),
                    APIName = table.Column<string>(maxLength: 30, nullable: false),
                    APIRequestBody = table.Column<string>(nullable: true),
                    APISendURL = table.Column<string>(nullable: false),
                    APIStatusCheckURL = table.Column<string>(nullable: true),
                    APIValidateURL = table.Column<string>(nullable: true),
                    AppType = table.Column<short>(nullable: false),
                    AuthHeader = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    HashCode = table.Column<string>(nullable: true),
                    HashCodeRecheck = table.Column<string>(nullable: true),
                    HashType = table.Column<short>(nullable: false),
                    MerchantCode = table.Column<string>(nullable: true),
                    MethodType = table.Column<string>(nullable: true),
                    ParsingDataID = table.Column<long>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    ResponseFailure = table.Column<string>(nullable: true),
                    ResponseHold = table.Column<string>(nullable: true),
                    ResponseSuccess = table.Column<string>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    TransactionIdPrefix = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirPartyAPIConfiguration", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TempUserRegister_RegisterTypeId",
                table: "TempUserRegister",
                column: "RegisterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TempOtpMaster_RegisterTypeId",
                table: "TempOtpMaster",
                column: "RegisterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TempOtpMaster_UserId",
                table: "TempOtpMaster",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TempOtpMaster_RegisterType_RegisterTypeId",
                table: "TempOtpMaster",
                column: "RegisterTypeId",
                principalTable: "RegisterType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TempOtpMaster_BizUser_UserId",
                table: "TempOtpMaster",
                column: "UserId",
                principalTable: "BizUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TempUserRegister_RegisterType_RegisterTypeId",
                table: "TempUserRegister",
                column: "RegisterTypeId",
                principalTable: "RegisterType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
