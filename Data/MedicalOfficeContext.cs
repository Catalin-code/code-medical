using CodeMedical.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeMedical.Data
{
    public class MedicalOfficeContext : DbContext
    {
        public MedicalOfficeContext(DbContextOptions<MedicalOfficeContext> options) : base(options)
        {
        }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptedDrugInfo> PrescriptedDrugInfos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Drug>().ToTable("Drug");
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Prescription>().ToTable("Prescription");
            modelBuilder.Entity<PrescriptedDrugInfo>().ToTable("QuantityAndDosage");
        }
    }
}
