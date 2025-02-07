using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarcusBilOchBluffAB.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Images",
                table: "Cars",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Cars",
                newName: "Images");
        }
    }
}
