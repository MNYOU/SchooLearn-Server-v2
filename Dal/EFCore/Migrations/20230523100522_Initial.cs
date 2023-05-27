using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "difficulties",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    scores = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_difficulties", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "file_data",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    file_name = table.Column<string>(type: "longtext", nullable: false),
                    content = table.Column<byte[]>(type: "longblob", nullable: false),
                    content_type = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_data", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "institutions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    tin = table.Column<long>(type: "bigint", fixedLength: true, maxLength: 12, nullable: false),
                    WebAddress = table.Column<string>(type: "longtext", nullable: false),
                    is_confirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    primary_invitation_code = table.Column<string>(type: "longtext", nullable: true),
                    invitation_code_for_teachers = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institutions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    is_reviewed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    InstitutionId = table.Column<long>(type: "bigint", nullable: false),
                    result = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.id);
                    table.ForeignKey(
                        name: "FK_applications_institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nickname = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    login = table.Column<string>(type: "longtext", nullable: false),
                    email = table.Column<string>(type: "longtext", nullable: false),
                    password = table.Column<string>(type: "longtext", nullable: false),
                    role = table.Column<short>(type: "smallint", nullable: false),
                    InstitutionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "institutions",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    InstitutionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_admins_institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_admins_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    InstitutionId = table.Column<long>(type: "bigint", nullable: true),
                    is_confirmed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_students_institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "institutions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_students_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    InstitutionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_teachers_institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teachers_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "group",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: true),
                    invitation_code = table.Column<string>(type: "varchar(255)", nullable: false),
                    TeacherId = table.Column<long>(type: "bigint", nullable: false),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group", x => x.id);
                    table.ForeignKey(
                        name: "FK_group_subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_group_teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "teachers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false),
                    DifficultyId = table.Column<long>(type: "bigint", nullable: false),
                    answer = table.Column<string>(type: "longtext", nullable: true),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsExtended = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    creation_datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    execution_period = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InstitutionId = table.Column<long>(type: "bigint", nullable: false),
                    TeacherId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_tasks_difficulties_DifficultyId",
                        column: x => x.DifficultyId,
                        principalTable: "difficulties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tasks_institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tasks_subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tasks_teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "teachers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GroupStudents",
                columns: table => new
                {
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    is_approved = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "solved_tasks",
                columns: table => new
                {
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    answer = table.Column<string>(type: "longtext", nullable: true),
                    FileAnswerId = table.Column<long>(type: "bigint", nullable: true),
                    IsChecked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Scores = table.Column<float>(type: "float", nullable: false),
                    solve_time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solved_tasks", x => new { x.StudentId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_solved_tasks_file_data_FileAnswerId",
                        column: x => x.FileAnswerId,
                        principalTable: "file_data",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_solved_tasks_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solved_tasks_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_admins_InstitutionId",
                table: "admins",
                column: "InstitutionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_applications_InstitutionId",
                table: "applications",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_group_invitation_code",
                table: "group",
                column: "invitation_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_group_SubjectId",
                table: "group",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_group_TeacherId",
                table: "group",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_StudentId",
                table: "GroupStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTask_TasksId",
                table: "GroupTask",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_solved_tasks_FileAnswerId",
                table: "solved_tasks",
                column: "FileAnswerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_solved_tasks_TaskId",
                table: "solved_tasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_students_InstitutionId",
                table: "students",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_DifficultyId",
                table: "tasks",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_InstitutionId",
                table: "tasks",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_SubjectId",
                table: "tasks",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_TeacherId",
                table: "tasks",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_teachers_InstitutionId",
                table: "teachers",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_users_InstitutionId",
                table: "users",
                column: "InstitutionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "GroupStudents");

            migrationBuilder.DropTable(
                name: "GroupTask");

            migrationBuilder.DropTable(
                name: "solved_tasks");

            migrationBuilder.DropTable(
                name: "group");

            migrationBuilder.DropTable(
                name: "file_data");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "difficulties");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "teachers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "institutions");
        }
    }
}
