using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class GroupStudentConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_solved_task_students_StudentId",
                table: "solved_task");

            migrationBuilder.DropForeignKey(
                name: "FK_solved_task_tasks_TaskId",
                table: "solved_task");

            migrationBuilder.DropTable(
                name: "GroupStudent");

            migrationBuilder.DropTable(
                name: "SolvedExtendedTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_solved_task",
                table: "solved_task");

            migrationBuilder.RenameTable(
                name: "solved_task",
                newName: "solved_tasks");

            migrationBuilder.RenameColumn(
                name: "worth",
                table: "difficulties",
                newName: "scores");

            migrationBuilder.RenameIndex(
                name: "IX_solved_task_TaskId",
                table: "solved_tasks",
                newName: "IX_solved_tasks_TaskId");

            migrationBuilder.AddColumn<long>(
                name: "InstitutionId",
                table: "tasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "tasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "TeacherId",
                table: "tasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "answer",
                table: "solved_tasks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<long>(
                name: "FileAnswerId",
                table: "solved_tasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "solved_tasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "Scores",
                table: "solved_tasks",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddPrimaryKey(
                name: "PK_solved_tasks",
                table: "solved_tasks",
                columns: new[] { "StudentId", "TaskId" });

            migrationBuilder.CreateTable(
                name: "file_data",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<byte[]>(type: "bytea", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_data", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GroupStudents",
                columns: table => new
                {
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    is_approved = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupStudents", x => new { x.GroupId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_GroupStudents_group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupStudents_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tasks_InstitutionId",
                table: "tasks",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_TeacherId",
                table: "tasks",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_solved_tasks_FileAnswerId",
                table: "solved_tasks",
                column: "FileAnswerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_StudentId",
                table: "GroupStudents",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_solved_tasks_file_data_FileAnswerId",
                table: "solved_tasks",
                column: "FileAnswerId",
                principalTable: "file_data",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_solved_tasks_students_StudentId",
                table: "solved_tasks",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_solved_tasks_tasks_TaskId",
                table: "solved_tasks",
                column: "TaskId",
                principalTable: "tasks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_institutions_InstitutionId",
                table: "tasks",
                column: "InstitutionId",
                principalTable: "institutions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_teachers_TeacherId",
                table: "tasks",
                column: "TeacherId",
                principalTable: "teachers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_solved_tasks_file_data_FileAnswerId",
                table: "solved_tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_solved_tasks_students_StudentId",
                table: "solved_tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_solved_tasks_tasks_TaskId",
                table: "solved_tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_institutions_InstitutionId",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_teachers_TeacherId",
                table: "tasks");

            migrationBuilder.DropTable(
                name: "file_data");

            migrationBuilder.DropTable(
                name: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_tasks_InstitutionId",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_TeacherId",
                table: "tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_solved_tasks",
                table: "solved_tasks");

            migrationBuilder.DropIndex(
                name: "IX_solved_tasks_FileAnswerId",
                table: "solved_tasks");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "FileAnswerId",
                table: "solved_tasks");

            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "solved_tasks");

            migrationBuilder.DropColumn(
                name: "Scores",
                table: "solved_tasks");

            migrationBuilder.RenameTable(
                name: "solved_tasks",
                newName: "solved_task");

            migrationBuilder.RenameColumn(
                name: "scores",
                table: "difficulties",
                newName: "worth");

            migrationBuilder.RenameIndex(
                name: "IX_solved_tasks_TaskId",
                table: "solved_task",
                newName: "IX_solved_task_TaskId");

            migrationBuilder.AlterColumn<string>(
                name: "answer",
                table: "solved_task",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_solved_task",
                table: "solved_task",
                columns: new[] { "StudentId", "TaskId" });

            migrationBuilder.CreateTable(
                name: "GroupStudent",
                columns: table => new
                {
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    StudentsUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupStudent", x => new { x.GroupsId, x.StudentsUserId });
                    table.ForeignKey(
                        name: "FK_GroupStudent_group_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupStudent_students_StudentsUserId",
                        column: x => x.StudentsUserId,
                        principalTable: "students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolvedExtendedTasks",
                columns: table => new
                {
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    AnswerAsFile = table.Column<byte[]>(type: "bytea", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false, defaultValue: "application/pdf"),
                    final_grade = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    is_checked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    solve_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolvedExtendedTasks", x => new { x.StudentId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_SolvedExtendedTasks_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolvedExtendedTasks_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudent_StudentsUserId",
                table: "GroupStudent",
                column: "StudentsUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SolvedExtendedTasks_TaskId",
                table: "SolvedExtendedTasks",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_solved_task_students_StudentId",
                table: "solved_task",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_solved_task_tasks_TaskId",
                table: "solved_task",
                column: "TaskId",
                principalTable: "tasks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
