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
            migrationBuilder.DropTable(
                name: "SubjectTeacher");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "SubjectTeacher",
                columns: table => new
                {
                    SubjectsId = table.Column<long>(type: "bigint", nullable: false),
                    TeachersUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTeacher", x => new { x.SubjectsId, x.TeachersUserId });
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_teachers_TeachersUserId",
                        column: x => x.TeachersUserId,
                        principalTable: "teachers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeacher_TeachersUserId",
                table: "SubjectTeacher",
                column: "TeachersUserId");
        }
    }
}
