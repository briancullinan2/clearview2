using EPIC.DataLayer.Customization;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("Patient")]
    public class Patient : Entity<Patient>
    {
        [MaxLength(256)]
        [Category("Demographics")]
        [Display(GroupName = "Basic Info", Order = 0, Name = "First Name", Description = "Fill in patient's first name")]
        public string FirstName { get; set; }
        [MaxLength(256)]
        [Category("Demographics")]
        [Display(GroupName = "Basic Info", Order = 2, Name = "Last Name", Description = "Fill in patient's surname")]
        public string LastName { get; set; }
        [MaxLength(2)]
        [Category("Demographics")]
        [Display(GroupName = "Basic Info", Order = 1, Name = "Middle Initial", Description = "Fill in patient's first letter of middle name if it exists")]
        public string MiddleInitial { get; set; }
        [MaxLength(12)]
        [Category("Demographics")]
        [Display(GroupName = "Basic Info", Order = 3, Name = "Birth Date", Description = "Fill in patient's birth date")]
        public DateTime BirthDate { get; set; }
        [MaxLength(4)]
        [Category("Demographics")]
        [Display(GroupName = "Basic Info", Order = 4, Name = "Age", Description = "Fill in patient's age")]
        public int Age { get; set; }
        [MaxLength(12)]
        [Category("Demographics")]
        [Display(GroupName = "Basic Info", Order = 5, Name = "Gender", Description = "Fill in patient's gender")]
        public Gender Gender { get; set; }
        [Key]
        public int PatientId { get; set; }
    }
}
