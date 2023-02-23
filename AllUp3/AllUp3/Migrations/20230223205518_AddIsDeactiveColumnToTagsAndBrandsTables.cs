using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllUp3.Migrations
{
    public partial class AddIsDeactiveColumnToTagsAndBrandsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Brands");
        }
    }
}
