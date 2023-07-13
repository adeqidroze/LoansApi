using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseContext.Data.Migrations
{
    public partial class addingadminsuperuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "FirstName", "IsBlocked", "LastName", "Password", "Password_salt", "Salary", "UserName", "UserRole" },
                values: new object[] { -1, 22, "SuperAdmin", false, "SuperAdmin", "31f2bef03c1e43a37695acb08c18c764ae8562350b64ea022c7e921a3ad4a2d2", "2+QCyOnC8h06goFFTpuHFkhQB4S3byaITkWqTDSX97A=", 10000000.0, "SuperAdmin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
