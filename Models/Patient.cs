using System;
using System.Collections.Generic;

namespace CodeMedical.Models
{
    public class Patient
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
