using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UserConfUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_institutions_InstitutionId",
                table: "users");

            migrationBuilder.AlterColumn<long>(
                name: "InstitutionId",
                table: "users",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_users_institutions_InstitutionId",
                table: "users",
                column: "InstitutionId",
                principalTable: "institutions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_institutions_InstitutionId",
                table: "users");

            migrationBuilder.AlterColumn<long>(
                name: "InstitutionId",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_institutions_InstitutionId",
                table: "users",
                column: "InstitutionId",
                principalTable: "institutions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
