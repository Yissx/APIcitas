using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCitas.Service.Data.Migrations
{
    public partial class LikedEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "Created");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Users",
                newName: "CreatedAt");
        }
    }
}
