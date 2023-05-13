using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UserConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InstitutionId",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_users_InstitutionId",
                table: "users",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_institutions_InstitutionId",
                table: "users",
                column: "InstitutionId",
                principalTable: "institutions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_institutions_InstitutionId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_InstitutionId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "users");
        }
    }
}
