using EPIC.DataLayer.Customization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataLayer.Entities
{
    public class Patient
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public DateTime BirthDate { get; set; }
        public PatientFields.Gender Gender { get; set; }
    }
}
