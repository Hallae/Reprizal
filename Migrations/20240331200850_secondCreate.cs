using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myApi.Migrations
{
    public partial class secondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing columns
            migrationBuilder.DropColumn(
                name: "author",
                table: "Application");
            migrationBuilder.DropColumn(
                name: "id",
                table: "Application");

            // Recreate the 'author' column as Guid
            migrationBuilder.AddColumn<Guid>(
                name: "author",
                table: "Application",
                type: "uniqueidentifier",
                nullable: false);

            // Recreate the 'id' column as Guid with IDENTITY
            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "Application",
                type: "uniqueidentifier",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", "IdentityColumn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the newly created columns
            migrationBuilder.DropColumn(
                name: "author",
                table: "Application");
            migrationBuilder.DropColumn(
                name: "id",
                table: "Application");

            // Recreate the original columns
            migrationBuilder.AddColumn<string>(
                name: "author",
                table: "Application",
                type: "nvarchar(max)",
                nullable: false);
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Application",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
