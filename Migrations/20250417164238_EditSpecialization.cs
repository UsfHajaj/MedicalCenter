using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalCenter.Migrations
{
    /// <inheritdoc />
    public partial class EditSpecialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialization_Specializations_SpecializationId",
                table: "DoctorSpecialization");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialization_Specializations_SpecializationId",
                table: "DoctorSpecialization",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialization_Specializations_SpecializationId",
                table: "DoctorSpecialization");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialization_Specializations_SpecializationId",
                table: "DoctorSpecialization",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id");
        }
    }
}
