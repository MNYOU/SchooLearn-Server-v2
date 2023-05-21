using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AdminConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "institutions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AdminId",
                table: "institutions",
                type: "bigint",
                nullable: true);
        }
    }
}
