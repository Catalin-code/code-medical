using CodeMedical.Models;
using System;
using System.Linq;

namespace CodeMedical.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MedicalOfficeContext context)
        {
            context.Database.EnsureCreated();

            //Look for any patient
            if (context.Patients.Any())
            {
                return;  //Patient has been already created
            }

            var drugs = new Drug[]
            {
                new Drug{Name="Ibuprofen"},
                new Drug{Name="Paracetamol"},
                new Drug{Name="Vitamin C"},
                new Drug{Name="Cough Drops"},
                new Drug{Name="Nasal Spray"}
            };
            foreach (Drug d in drugs)
            {
                context.Drugs.Add(d);
            }
            context.SaveChanges();

            var patients = new Patient[]
            {
                new Patient{LastName="Despa", FirstName="Catalin-Daniel", BirthDate=DateTime.Parse("1999-06-07")},
                new Patient{LastName="Tudorache", FirstName="Gabriela-Daniela", BirthDate=DateTime.Parse("1999-05-05")},
                new Patient{LastName="Fieraru", FirstName="Andi-Gabriel", BirthDate=DateTime.Parse("2000-03-26")}
            };
            foreach (Patient pa in patients)
            {
                context.Patients.Add(pa);
            }
            context.SaveChanges();

            var prescriptions = new Prescription[]
            {
                new Prescription{PatientID=1, Series="NPHGDV", Number=1001, IssueDate=DateTime.Parse("2021-01-03")},
                new Prescription{PatientID=2, Series="NPHGDV", Number=1002, IssueDate=DateTime.Parse("2021-04-27")},
                new Prescription{PatientID=3, Series="NPHGDV", Number=1003, IssueDate=DateTime.Parse("2021-07-12")}
            };
            foreach (Prescription pr in prescriptions)
            {
                context.Prescriptions.Add(pr);
            }
            context.SaveChanges();

            var prescriptedDrugsInfos = new PrescriptedDrugInfo[]
            {
                new PrescriptedDrugInfo{PrescriptionID=1, DrugID=1, Quantity=12, Dosage=4.0},
                new PrescriptedDrugInfo{PrescriptionID=1, DrugID=3, Quantity=32, Dosage=6.0},
                new PrescriptedDrugInfo{PrescriptionID=2, DrugID=2, Quantity=12, Dosage=4.0},
                new PrescriptedDrugInfo{PrescriptionID=2, DrugID=4, Quantity=32, Dosage=6.0},
                new PrescriptedDrugInfo{PrescriptionID=3, DrugID=4, Quantity=32, Dosage=6.0},
                new PrescriptedDrugInfo{PrescriptionID=3, DrugID=5, Quantity=1, Dosage=6.0}
            };
            foreach (PrescriptedDrugInfo pdi in prescriptedDrugsInfos)
            {
                context.PrescriptedDrugInfos.Add(pdi);
            }
            context.SaveChanges();
        }
    }
}