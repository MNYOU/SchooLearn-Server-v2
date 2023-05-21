using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class SubjectConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subjects_teachers_TeacherId",
                table: "subjects");

            migrationBuilder.DropIndex(
                name: "IX_subjects_TeacherId",
                table: "subjects");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "subjects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TeacherId",
                table: "subjects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_subjects_TeacherId",
                table: "subjects",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_subjects_teachers_TeacherId",
                table: "subjects",
                column: "TeacherId",
                principalTable: "teachers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
