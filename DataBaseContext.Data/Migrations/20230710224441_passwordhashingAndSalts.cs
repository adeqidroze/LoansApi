using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseContext.Data.Migrations
{
    public partial class passwordhashingAndSalts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password_salt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password_salt",
                table: "Users");
        }
    }
}
