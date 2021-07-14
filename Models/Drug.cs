using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMedical.Models
{
    public class Drug
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<PrescriptedDrugInfo> PrescriptedDrugInfos { get; set; }
    }
}
