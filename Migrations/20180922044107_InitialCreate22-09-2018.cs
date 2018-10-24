using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Web.Migrations
{
    public partial class InitialCreate22092018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "BizUser",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "OTP",
                table: "BizUser",
                nullable: false,
                defaultValue: 0);

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
                    ModeIdId = table.Column<int>(nullable: true),
                    HostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginLog_Mode_ModeIdId",
                        column: x => x.ModeIdId,
                        principalTable: "Mode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginLog_ModeIdId",
                table: "LoginLog",
                column: "ModeIdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginLog");

            migrationBuilder.DropTable(
                name: "Mode");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "BizUser");

            migrationBuilder.DropColumn(
                name: "OTP",
                table: "BizUser");
        }
    }
}
