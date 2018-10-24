using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Web.Migrations
{
    public partial class InitialCreate22092018_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "OTP",
                table: "BizUser",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OTP",
                table: "BizUser",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
