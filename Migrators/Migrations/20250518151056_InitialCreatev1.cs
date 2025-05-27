using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatev1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Inventory",
                table: "ModuleData");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Inventory",
                table: "ModuleData");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Inventory",
                table: "ModuleData");

            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "Inventory",
                table: "ModuleData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Inventory",
                table: "ModuleData",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Inventory",
                table: "ModuleData",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "Inventory",
                table: "ModuleData",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                schema: "Inventory",
                table: "ModuleData",
                type: "jsonb",
                nullable: true);
        }
    }
}
