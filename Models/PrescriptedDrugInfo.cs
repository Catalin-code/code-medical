namespace CodeMedical.Models
{
    public class PrescriptedDrugInfo
    {
        public int ID { get; set; }
        public int PrescriptionID { get; set; }
        public int DrugID { get; set; }
        public int Quantity { get; set; }
        public double Dosage { get; set; }
        public Prescription Prescription { get; set; }
        public Drug Drug { get; set; }
    }
}
