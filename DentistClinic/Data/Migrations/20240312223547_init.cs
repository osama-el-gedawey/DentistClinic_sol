using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistClinic.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analysis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analysis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChiefComplains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiefComplains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurentBalance = table.Column<double>(type: "float", nullable: false,defaultValue:0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Xrays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Xrays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChiefComplainPatients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiefComplainId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiefComplainPatients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiefComplainPatients_ChiefComplains_ChiefComplainId",
                        column: x => x.ChiefComplainId,
                        principalTable: "ChiefComplains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiefComplainPatients_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistorys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistorys_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRecords_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescriptiones_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tplans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tplans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tplans_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistoryImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicalHistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistoryImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistoryImages_MedicalHistorys_MedicalHistoryId",
                        column: x => x.MedicalHistoryId,
                        principalTable: "MedicalHistorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisPrescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cause = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrescriptionId = table.Column<int>(type: "int", nullable: false),
                    AnalysisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisPrescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisPrescriptions_Analysis_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalysisPrescriptions_Prescriptiones_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicinePrescriptiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dose = table.Column<double>(type: "float", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: true),
                    Days = table.Column<int>(type: "int", nullable: true),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePrescriptiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinePrescriptiones_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicinePrescriptiones_Prescriptiones_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XrayPrescriptiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cause = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XrayId = table.Column<int>(type: "int", nullable: false),
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XrayPrescriptiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XrayPrescriptiones_Prescriptiones_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XrayPrescriptiones_Xrays_XrayId",
                        column: x => x.XrayId,
                        principalTable: "Xrays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisPrescriptionImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnalysisPrescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisPrescriptionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisPrescriptionImages_AnalysisPrescriptions_AnalysisPrescriptionId",
                        column: x => x.AnalysisPrescriptionId,
                        principalTable: "AnalysisPrescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XrayPrescriptionImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XrayPrescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XrayPrescriptionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XrayPrescriptionImages_XrayPrescriptiones_XrayPrescriptionId",
                        column: x => x.XrayPrescriptionId,
                        principalTable: "XrayPrescriptiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PatientId",
                table: "Accounts",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisPrescriptionImages_AnalysisPrescriptionId",
                table: "AnalysisPrescriptionImages",
                column: "AnalysisPrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisPrescriptions_AnalysisId",
                table: "AnalysisPrescriptions",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisPrescriptions_PrescriptionId",
                table: "AnalysisPrescriptions",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiefComplainPatients_ChiefComplainId",
                table: "ChiefComplainPatients",
                column: "ChiefComplainId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiefComplainPatients_PatientId",
                table: "ChiefComplainPatients",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryImages_MedicalHistoryId",
                table: "MedicalHistoryImages",
                column: "MedicalHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistorys_PatientId",
                table: "MedicalHistorys",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePrescriptiones_MedicineId",
                table: "MedicinePrescriptiones",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePrescriptiones_PrescriptionId",
                table: "MedicinePrescriptiones",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecords_PatientId",
                table: "PaymentRecords",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptiones_PatientId",
                table: "Prescriptiones",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Tplans_PatientId",
                table: "Tplans",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayPrescriptiones_PrescriptionId",
                table: "XrayPrescriptiones",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayPrescriptiones_XrayId",
                table: "XrayPrescriptiones",
                column: "XrayId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayPrescriptionImages_XrayPrescriptionId",
                table: "XrayPrescriptionImages",
                column: "XrayPrescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AnalysisPrescriptionImages");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "ChiefComplainPatients");

            migrationBuilder.DropTable(
                name: "MedicalHistoryImages");

            migrationBuilder.DropTable(
                name: "MedicinePrescriptiones");

            migrationBuilder.DropTable(
                name: "PaymentRecords");

            migrationBuilder.DropTable(
                name: "Tplans");

            migrationBuilder.DropTable(
                name: "XrayPrescriptionImages");

            migrationBuilder.DropTable(
                name: "AnalysisPrescriptions");

            migrationBuilder.DropTable(
                name: "ChiefComplains");

            migrationBuilder.DropTable(
                name: "MedicalHistorys");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "XrayPrescriptiones");

            migrationBuilder.DropTable(
                name: "Analysis");

            migrationBuilder.DropTable(
                name: "Prescriptiones");

            migrationBuilder.DropTable(
                name: "Xrays");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
