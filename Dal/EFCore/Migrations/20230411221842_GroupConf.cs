using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class GroupConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_institution_InstitutionId",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "FK_institution_admins_AdminId",
                table: "institution");

            migrationBuilder.DropForeignKey(
                name: "FK_students_institution_InstitutionId",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_teachers_institution_InstitutionId",
                table: "teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_institution",
                table: "institution");

            migrationBuilder.RenameTable(
                name: "institution",
                newName: "institutions");

            migrationBuilder.RenameIndex(
                name: "IX_institution_AdminId",
                table: "institutions",
                newName: "IX_institutions_AdminId");

            migrationBuilder.AlterColumn<short>(
                name: "role",
                table: "users",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_institutions",
                table: "institutions",
                column: "id");

            migrationBuilder.CreateTable(
                name: "GroupTask",
                columns: table => new
                {
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    TasksId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTask", x => new { x.GroupsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_GroupTask_group_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupTask_tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupTask_TasksId",
                table: "GroupTask",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_applications_institutions_InstitutionId",
                table: "applications",
                column: "InstitutionId",
                principalTable: "institutions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_institutions_admins_AdminId",
                table: "institutions",
                column: "AdminId",
                principalTable: "admins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_institutions_InstitutionId",
                table: "students",
                column: "InstitutionId",
                principalTable: "institutions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_teachers_institutions_InstitutionId",
                table: "teachers",
                column: "InstitutionId",
                principalTable: "institutions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_institutions_InstitutionId",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "FK_institutions_admins_AdminId",
                table: "institutions");

            migrationBuilder.DropForeignKey(
                name: "FK_students_institutions_InstitutionId",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_teachers_institutions_InstitutionId",
                table: "teachers");

            migrationBuilder.DropTable(
                name: "GroupTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_institutions",
                table: "institutions");

            migrationBuilder.RenameTable(
                name: "institutions",
                newName: "institution");

            migrationBuilder.RenameIndex(
                name: "IX_institutions_AdminId",
                table: "institution",
                newName: "IX_institution_AdminId");

            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_institution",
                table: "institution",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_applications_institution_InstitutionId",
                table: "applications",
                column: "InstitutionId",
                principalTable: "institution",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_institution_admins_AdminId",
                table: "institution",
                column: "AdminId",
                principalTable: "admins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_institution_InstitutionId",
                table: "students",
                column: "InstitutionId",
                principalTable: "institution",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_teachers_institution_InstitutionId",
                table: "teachers",
                column: "InstitutionId",
                principalTable: "institution",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
