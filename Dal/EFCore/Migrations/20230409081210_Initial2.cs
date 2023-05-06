using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Institutions_InstitutionId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Subjects_SubjectId",
                table: "GroupsStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Teachers_TeacherId",
                table: "GroupsStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudent_Groups_GroupsId",
                table: "GroupStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudent_Students_StudentsUserId",
                table: "GroupStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_Institutions_Admins_AdminId",
                table: "Institutions");

            migrationBuilder.DropForeignKey(
                name: "FK_SolvedTasks_Students_StudentId",
                table: "SolvedTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_SolvedTasks_tasks_TaskId",
                table: "SolvedTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Institutions_InstitutionId",
                table: "GroupsStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Users_UserId",
                table: "GroupsStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_Subjects_SubjectsId",
                table: "SubjectTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_Teachers_TeachersUserId",
                table: "SubjectTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_Subjects_SubjectId",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Institutions_InstitutionId",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Users_UserId",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "GroupsStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolvedTasks",
                table: "SolvedTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Institutions",
                table: "Institutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "GroupsStudent");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "teachers");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "subjects");

            migrationBuilder.RenameTable(
                name: "GroupsStudent",
                newName: "students");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "applications");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "admins");

            migrationBuilder.RenameTable(
                name: "SolvedTasks",
                newName: "solved_task");

            migrationBuilder.RenameTable(
                name: "Institutions",
                newName: "institution");

            migrationBuilder.RenameTable(
                name: "GroupsStudent",
                newName: "group");

            migrationBuilder.RenameIndex(
                name: "IX_Teachers_InstitutionId",
                table: "teachers",
                newName: "IX_teachers_InstitutionId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_InstitutionId",
                table: "students",
                newName: "IX_students_InstitutionId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_InstitutionId",
                table: "applications",
                newName: "IX_applications_InstitutionId");

            migrationBuilder.RenameIndex(
                name: "IX_SolvedTasks_TaskId",
                table: "solved_task",
                newName: "IX_solved_task_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Institutions_AdminId",
                table: "institution",
                newName: "IX_institution_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_TeacherId",
                table: "group",
                newName: "IX_group_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_SubjectId",
                table: "group",
                newName: "IX_group_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_invitation_code",
                table: "group",
                newName: "IX_group_invitation_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_teachers",
                table: "teachers",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_subjects",
                table: "subjects",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_students",
                table: "students",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_applications",
                table: "applications",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_admins",
                table: "admins",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_solved_task",
                table: "solved_task",
                columns: new[] { "StudentId", "TaskId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_institution",
                table: "institution",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_group",
                table: "group",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_admins_users_UserId",
                table: "admins",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_applications_institution_InstitutionId",
                table: "applications",
                column: "InstitutionId",
                principalTable: "institution",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_subjects_SubjectId",
                table: "group",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_teachers_TeacherId",
                table: "group",
                column: "TeacherId",
                principalTable: "teachers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudent_group_GroupsId",
                table: "GroupStudent",
                column: "GroupsId",
                principalTable: "group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudent_students_StudentsUserId",
                table: "GroupStudent",
                column: "StudentsUserId",
                principalTable: "students",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_institution_admins_AdminId",
                table: "institution",
                column: "AdminId",
                principalTable: "admins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_students_institution_InstitutionId",
                table: "students",
                column: "InstitutionId",
                principalTable: "institution",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_students_users_UserId",
                table: "students",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_subjects_SubjectsId",
                table: "SubjectTeacher",
                column: "SubjectsId",
                principalTable: "subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_teachers_TeachersUserId",
                table: "SubjectTeacher",
                column: "TeachersUserId",
                principalTable: "teachers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_subjects_SubjectId",
                table: "tasks",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teachers_institution_InstitutionId",
                table: "teachers",
                column: "InstitutionId",
                principalTable: "institution",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teachers_users_UserId",
                table: "teachers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admins_users_UserId",
                table: "admins");

            migrationBuilder.DropForeignKey(
                name: "FK_applications_institution_InstitutionId",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "FK_group_subjects_SubjectId",
                table: "group");

            migrationBuilder.DropForeignKey(
                name: "FK_group_teachers_TeacherId",
                table: "group");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudent_group_GroupsId",
                table: "GroupStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudent_students_StudentsUserId",
                table: "GroupStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_institution_admins_AdminId",
                table: "institution");

            migrationBuilder.DropForeignKey(
                name: "FK_solved_task_students_StudentId",
                table: "solved_task");

            migrationBuilder.DropForeignKey(
                name: "FK_solved_task_tasks_TaskId",
                table: "solved_task");

            migrationBuilder.DropForeignKey(
                name: "FK_students_institution_InstitutionId",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_students_users_UserId",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_subjects_SubjectsId",
                table: "SubjectTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_teachers_TeachersUserId",
                table: "SubjectTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_subjects_SubjectId",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_teachers_institution_InstitutionId",
                table: "teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_teachers_users_UserId",
                table: "teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_teachers",
                table: "teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_subjects",
                table: "subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_students",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_applications",
                table: "applications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_admins",
                table: "admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_solved_task",
                table: "solved_task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_institution",
                table: "institution");

            migrationBuilder.DropPrimaryKey(
                name: "PK_group",
                table: "group");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "teachers",
                newName: "Teachers");

            migrationBuilder.RenameTable(
                name: "subjects",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "students",
                newName: "GroupsStudent");

            migrationBuilder.RenameTable(
                name: "applications",
                newName: "Applications");

            migrationBuilder.RenameTable(
                name: "admins",
                newName: "Admins");

            migrationBuilder.RenameTable(
                name: "solved_task",
                newName: "SolvedTasks");

            migrationBuilder.RenameTable(
                name: "institution",
                newName: "Institutions");

            migrationBuilder.RenameTable(
                name: "group",
                newName: "GroupsStudent");

            migrationBuilder.RenameIndex(
                name: "IX_teachers_InstitutionId",
                table: "Teachers",
                newName: "IX_Teachers_InstitutionId");

            migrationBuilder.RenameIndex(
                name: "IX_students_InstitutionId",
                table: "GroupsStudent",
                newName: "IX_Students_InstitutionId");

            migrationBuilder.RenameIndex(
                name: "IX_applications_InstitutionId",
                table: "Applications",
                newName: "IX_Applications_InstitutionId");

            migrationBuilder.RenameIndex(
                name: "IX_solved_task_TaskId",
                table: "SolvedTasks",
                newName: "IX_SolvedTasks_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_institution_AdminId",
                table: "Institutions",
                newName: "IX_Institutions_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_group_TeacherId",
                table: "GroupsStudent",
                newName: "IX_Groups_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_group_SubjectId",
                table: "GroupsStudent",
                newName: "IX_Groups_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_group_invitation_code",
                table: "GroupsStudent",
                newName: "IX_Groups_invitation_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "GroupsStudent",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolvedTasks",
                table: "SolvedTasks",
                columns: new[] { "StudentId", "TaskId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Institutions",
                table: "Institutions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "GroupsStudent",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Institutions_InstitutionId",
                table: "Applications",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Subjects_SubjectId",
                table: "GroupsStudent",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Teachers_TeacherId",
                table: "GroupsStudent",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudent_Groups_GroupsId",
                table: "GroupStudent",
                column: "GroupsId",
                principalTable: "GroupsStudent",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudent_Students_StudentsUserId",
                table: "GroupStudent",
                column: "StudentsUserId",
                principalTable: "GroupsStudent",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Institutions_Admins_AdminId",
                table: "Institutions",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolvedTasks_Students_StudentId",
                table: "SolvedTasks",
                column: "StudentId",
                principalTable: "GroupsStudent",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolvedTasks_tasks_TaskId",
                table: "SolvedTasks",
                column: "TaskId",
                principalTable: "tasks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Institutions_InstitutionId",
                table: "GroupsStudent",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Users_UserId",
                table: "GroupsStudent",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_Subjects_SubjectsId",
                table: "SubjectTeacher",
                column: "SubjectsId",
                principalTable: "Subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_Teachers_TeachersUserId",
                table: "SubjectTeacher",
                column: "TeachersUserId",
                principalTable: "Teachers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_Subjects_SubjectId",
                table: "tasks",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Institutions_InstitutionId",
                table: "Teachers",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Users_UserId",
                table: "Teachers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
