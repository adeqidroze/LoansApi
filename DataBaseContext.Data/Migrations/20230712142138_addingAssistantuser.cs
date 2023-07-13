using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseContext.Data.Migrations
{
    public partial class addingAssistantuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "FirstName", "IsBlocked", "LastName", "Password", "Password_salt", "Salary", "UserName", "UserRole" },
                values: new object[] { -2, 0, "Assistant", false, "Assistant", "c67cbe0b9e69b6295cd6bc6fc2cfac94f0457de05c17a1c9a3b894ae23c394b9", "9+QCyOnC8h06goFFTpuHFkhQB4S3byaITkWqTDSX97g=", 0.0, "Assistant", "Assistant" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -2);
        }
    }
}
