using EPIC.DataLayer.Customization;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("Patient")]
    public class Patient : IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public DateTime BirthDate { get; set; }
        public PatientFields.Gender Gender { get; set; }
        public int PatientId { get; set; }
    }
}
