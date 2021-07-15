using System.Collections.Generic;

namespace CodeMedical.Models
{
    public class Drug
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<PrescriptedDrugInfo> PrescriptedDrugInfos { get; set; }
    }
}
