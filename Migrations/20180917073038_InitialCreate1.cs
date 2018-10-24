using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Web.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserPhotos_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserPhotos",
                table: "ApplicationUserPhotos");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "BizUserToken");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "BizUser");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "BizUserRole");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "BizUserLogin");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "BizUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "BizRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "BizRolesClaims");

            migrationBuilder.RenameTable(
                name: "ApplicationUserPhotos",
                newName: "BizUserPhotos");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "BizUserRole",
                newName: "IX_BizUserRole_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "BizUserLogin",
                newName: "IX_BizUserLogin_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "BizUserClaims",
                newName: "IX_BizUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "BizRolesClaims",
                newName: "IX_BizRolesClaims_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserPhotos_ApplicationUserId",
                table: "BizUserPhotos",
                newName: "IX_BizUserPhotos_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BizUserToken",
                table: "BizUserToken",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BizUser",
                table: "BizUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BizUserRole",
                table: "BizUserRole",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BizUserLogin",
                table: "BizUserLogin",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BizUserClaims",
                table: "BizUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BizRoles",
                table: "BizRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BizRolesClaims",
                table: "BizRolesClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BizUserPhotos",
                table: "BizUserPhotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BizRolesClaims_BizRoles_RoleId",
                table: "BizRolesClaims",
                column: "RoleId",
                principalTable: "BizRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BizUserClaims_BizUser_UserId",
                table: "BizUserClaims",
                column: "UserId",
                principalTable: "BizUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BizUserLogin_BizUser_UserId",
                table: "BizUserLogin",
                column: "UserId",
                principalTable: "BizUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BizUserPhotos_BizUser_ApplicationUserId",
                table: "BizUserPhotos",
                column: "ApplicationUserId",
                principalTable: "BizUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BizUserRole_BizRoles_RoleId",
                table: "BizUserRole",
                column: "RoleId",
                principalTable: "BizRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BizUserRole_BizUser_UserId",
                table: "BizUserRole",
                column: "UserId",
                principalTable: "BizUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BizUserToken_BizUser_UserId",
                table: "BizUserToken",
                column: "UserId",
                principalTable: "BizUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BizRolesClaims_BizRoles_RoleId",
                table: "BizRolesClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_BizUserClaims_BizUser_UserId",
                table: "BizUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_BizUserLogin_BizUser_UserId",
                table: "BizUserLogin");

            migrationBuilder.DropForeignKey(
                name: "FK_BizUserPhotos_BizUser_ApplicationUserId",
                table: "BizUserPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_BizUserRole_BizRoles_RoleId",
                table: "BizUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_BizUserRole_BizUser_UserId",
                table: "BizUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_BizUserToken_BizUser_UserId",
                table: "BizUserToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BizUserToken",
                table: "BizUserToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BizUserRole",
                table: "BizUserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BizUserPhotos",
                table: "BizUserPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BizUserLogin",
                table: "BizUserLogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BizUserClaims",
                table: "BizUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BizUser",
                table: "BizUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BizRolesClaims",
                table: "BizRolesClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BizRoles",
                table: "BizRoles");

            migrationBuilder.RenameTable(
                name: "BizUserToken",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "BizUserRole",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "BizUserPhotos",
                newName: "ApplicationUserPhotos");

            migrationBuilder.RenameTable(
                name: "BizUserLogin",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "BizUserClaims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "BizUser",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "BizRolesClaims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "BizRoles",
                newName: "AspNetRoles");

            migrationBuilder.RenameIndex(
                name: "IX_BizUserRole_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_BizUserPhotos_ApplicationUserId",
                table: "ApplicationUserPhotos",
                newName: "IX_ApplicationUserPhotos_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BizUserLogin_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BizUserClaims_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BizRolesClaims_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserPhotos",
                table: "ApplicationUserPhotos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserPhotos_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserPhotos",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
