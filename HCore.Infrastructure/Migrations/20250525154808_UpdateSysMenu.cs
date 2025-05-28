using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSysMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PermissionName",
                table: "SysMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionName",
                table: "SysMenus");
        }
    }
}
