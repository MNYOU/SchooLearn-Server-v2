using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class TasksFileConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_solved_tasks_file_data_FileAnswerId",
                table: "solved_tasks");

            migrationBuilder.AlterColumn<long>(
                name: "FileAnswerId",
                table: "solved_tasks",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_solved_tasks_file_data_FileAnswerId",
                table: "solved_tasks",
                column: "FileAnswerId",
                principalTable: "file_data",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_solved_tasks_file_data_FileAnswerId",
                table: "solved_tasks");

            migrationBuilder.AlterColumn<long>(
                name: "FileAnswerId",
                table: "solved_tasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_solved_tasks_file_data_FileAnswerId",
                table: "solved_tasks",
                column: "FileAnswerId",
                principalTable: "file_data",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
