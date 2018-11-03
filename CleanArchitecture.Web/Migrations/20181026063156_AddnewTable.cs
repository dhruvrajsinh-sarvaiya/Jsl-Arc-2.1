using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Web.Migrations
{
    public partial class AddnewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StckingScheme",
                table: "StckingScheme");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberShadowLimit",
                table: "MemberShadowLimit");

            migrationBuilder.DropColumn(
                name: "SerProConfigurationID",
                table: "ThirdPartyAPIConfiguration");

            migrationBuilder.AddColumn<int>(
                name: "WalletTrnType",
                table: "WalletTransactionQueues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TimeStamp",
                table: "ThirdPartyAPIConfiguration",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ConvertAmount",
                table: "RouteConfiguration",
                type: "decimal(18, 8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StckingScheme",
                table: "StckingScheme",
                column: "WalletType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberShadowLimit",
                table: "MemberShadowLimit",
                column: "MemberTypeId");

            migrationBuilder.CreateTable(
                name: "WalletLimitConfigurationMaster",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                    table.PrimaryKey("PK_WalletLimitConfigurationMaster", x => x.TrnType);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletLimitConfigurationMaster");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StckingScheme",
                table: "StckingScheme");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberShadowLimit",
                table: "MemberShadowLimit");

            migrationBuilder.DropColumn(
                name: "WalletTrnType",
                table: "WalletTransactionQueues");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "ThirdPartyAPIConfiguration");

            migrationBuilder.DropColumn(
                name: "ConvertAmount",
                table: "RouteConfiguration");

            migrationBuilder.AddColumn<long>(
                name: "SerProConfigurationID",
                table: "ThirdPartyAPIConfiguration",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StckingScheme",
                table: "StckingScheme",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberShadowLimit",
                table: "MemberShadowLimit",
                column: "Id");
        }
    }
}
