using System;
using System.Collections.Generic;

namespace CodeMedical.Models
{
    public class Prescription
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public string Series { get; set; }
        public int Number { get; set; }
        public DateTime IssueDate { get; set; }
        public ICollection<PrescriptedDrugInfo> PrescriptedDrugInfos { get; set; }
        public Patient Patient { get; set; }
    }
}
