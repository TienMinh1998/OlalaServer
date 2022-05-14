using Microsoft.EntityFrameworkCore.Migrations;

namespace APIProject.Domain.Migrations
{
    public partial class innitSchemaDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Trasaction");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Orders",
                newSchema: "Trasaction");

            migrationBuilder.RenameTable(
                name: "OrderComplainImages",
                newName: "OrderComplainImages",
                newSchema: "Trasaction");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Carts",
                newSchema: "Trasaction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "Trasaction",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "OrderComplainImages",
                schema: "Trasaction",
                newName: "OrderComplainImages");

            migrationBuilder.RenameTable(
                name: "Carts",
                schema: "Trasaction",
                newName: "Carts");
        }
    }
}
