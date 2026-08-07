using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElearningSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationMinutesToExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Exams");
        }
    }
}
