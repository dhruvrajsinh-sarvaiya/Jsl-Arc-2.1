using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Web.Migrations
{
    public partial class InitialCreate22092018_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginLog_Mode_ModeIdId",
                table: "LoginLog");

            migrationBuilder.RenameColumn(
                name: "ModeIdId",
                table: "LoginLog",
                newName: "ModeId");

            migrationBuilder.RenameIndex(
                name: "IX_LoginLog_ModeIdId",
                table: "LoginLog",
                newName: "IX_LoginLog_ModeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginLog_Mode_ModeId",
                table: "LoginLog",
                column: "ModeId",
                principalTable: "Mode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginLog_Mode_ModeId",
                table: "LoginLog");

            migrationBuilder.RenameColumn(
                name: "ModeId",
                table: "LoginLog",
                newName: "ModeIdId");

            migrationBuilder.RenameIndex(
                name: "IX_LoginLog_ModeId",
                table: "LoginLog",
                newName: "IX_LoginLog_ModeIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginLog_Mode_ModeIdId",
                table: "LoginLog",
                column: "ModeIdId",
                principalTable: "Mode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
