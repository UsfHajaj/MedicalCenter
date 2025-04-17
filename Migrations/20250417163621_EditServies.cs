using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalCenter.Migrations
{
    /// <inheritdoc />
    public partial class EditServies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_MedicalCenter_MedicalCenterId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Specializations_SpecializationId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "MedicalCenterId",
                table: "Doctors",
                newName: "MedicalCentersId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_MedicalCenterId",
                table: "Doctors",
                newName: "IX_Doctors_MedicalCentersId");

            migrationBuilder.AlterColumn<int>(
                name: "SpecializationId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_MedicalCenter_MedicalCentersId",
                table: "Doctors",
                column: "MedicalCentersId",
                principalTable: "MedicalCenter",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Specializations_SpecializationId",
                table: "Services",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_MedicalCenter_MedicalCentersId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Specializations_SpecializationId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "MedicalCentersId",
                table: "Doctors",
                newName: "MedicalCenterId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_MedicalCentersId",
                table: "Doctors",
                newName: "IX_Doctors_MedicalCenterId");

            migrationBuilder.AlterColumn<int>(
                name: "SpecializationId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_MedicalCenter_MedicalCenterId",
                table: "Doctors",
                column: "MedicalCenterId",
                principalTable: "MedicalCenter",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Specializations_SpecializationId",
                table: "Services",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id");
        }
    }
}
