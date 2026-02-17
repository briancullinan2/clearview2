using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("User")]
    public class User : Entity<User>
    {
        [Key]
        [Display(Name = "Globally Unique ID", Description = "Server assigned GUID for synchronization tracking")]
        public string Guid { get; private set; }
        [Category("General Info")]
        [Display(Order = 0, Name = "First Name", Description = "Fill in users first name")]
        public string FirstName { get; set; }
        [Category("General Info")]
        [Display(Order = 2, Name = "Last Name", Description = "Fill in users surname")]
        public string LastName { get; set; }
        [Category("General Info")]
        [Display(Order = 1, Name = "Middle Initial", Description = "Fill in users first letter of middle name if it exists")]
        public string MiddleInitial { get; set; }

    }
}
