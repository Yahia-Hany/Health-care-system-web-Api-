using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Health_care_system__web_Api_.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "Name", "Specialization" },
                values: new object[,]
                {
                    { 5, "Dr. Ahmed", "Cardiology" },
                    { 6, "Dr. Ali", "Dentist" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "BirthDate", "Name" },
                values: new object[,]
                {
                    { 4, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omar" },
                    { 5, new DateTime(1998, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "mahmoud" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "DoctorId", "PatientId", "AppointmentDate" },
                values: new object[,]
                {
                    { 6, 4, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumns: new[] { "DoctorId", "PatientId" },
                keyValues: new object[] { 6, 4 });

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumns: new[] { "DoctorId", "PatientId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
