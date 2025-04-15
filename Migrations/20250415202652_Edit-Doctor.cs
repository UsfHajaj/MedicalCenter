using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalCenter.Migrations
{
    /// <inheritdoc />
    public partial class EditDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfessionalStatement",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfessionalStatement",
                table: "Doctors");
        }
    }
}
