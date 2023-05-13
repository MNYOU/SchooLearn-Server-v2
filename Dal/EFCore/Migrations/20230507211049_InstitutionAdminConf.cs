using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InstitutionAdminConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_institutions_admins_AdminId",
                table: "institutions");

            migrationBuilder.RenameColumn(
                name: "PrimaryInvitationCode",
                table: "institutions",
                newName: "primary_invitation_code");

            migrationBuilder.AlterColumn<long>(
                name: "AdminId",
                table: "institutions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "primary_invitation_code",
                table: "institutions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_institutions_admins_AdminId",
                table: "institutions",
                column: "AdminId",
                principalTable: "admins",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_institutions_admins_AdminId",
                table: "institutions");

            migrationBuilder.RenameColumn(
                name: "primary_invitation_code",
                table: "institutions",
                newName: "PrimaryInvitationCode");

            migrationBuilder.AlterColumn<long>(
                name: "AdminId",
                table: "institutions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PrimaryInvitationCode",
                table: "institutions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_institutions_admins_AdminId",
                table: "institutions",
                column: "AdminId",
                principalTable: "admins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
