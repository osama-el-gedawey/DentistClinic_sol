using DentistClinic.Configurations;
using DentistClinic.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DentistClinic.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Analysis> Analysis { get; set; }
        public virtual DbSet<AnalysisPrescription> AnalysisPrescriptions { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<ChiefComplain> ChiefComplains { get; set; }
        public virtual DbSet<ChiefComplainPatient> ChiefComplainPatients { get; set; }
        public virtual DbSet<MedicalHistory> MedicalHistorys { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<MedicinePrescriptione> MedicinePrescriptiones { get; set; }
        public virtual DbSet<PaymentRecord> PaymentRecords { get; set; }
        public virtual DbSet<Prescription> Prescriptiones { get; set; }
        public virtual DbSet<Tplans> Tplans { get; set; }
        public virtual DbSet<Xray> Xrays { get; set; }
        public virtual DbSet<XrayPrescription> XrayPrescriptiones { get; set; }
        public virtual DbSet<MedicalHistoryImage> MedicalHistoryImages { get; set; }
        public virtual DbSet<AnalysisPrescriptionImage> AnalysisPrescriptionImages { get; set; }
        public virtual DbSet<XrayPrescriptionImage> XrayPrescriptionImages { get; set; }
        public virtual DbSet<ContactMsg> ContactMsgs { get; set; }
        public virtual DbSet<Tooth> Teeth { get; set; }

        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new PatientEntityTypeConfiguration().Configure(modelBuilder.Entity<Patient>());
            new TPlansEntityTypeConfiguration().Configure(modelBuilder.Entity<Tplans>());

        }

    }
}
