using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("User")]
    public class User : Entity<User>
    {
        [Key]
        [MaxLength(256)]
        [Category("User Info")]
        [Display(Name = "Globally Unique ID", Description = "Server assigned GUID for synchronization tracking")]
        public string Guid { get; private set; }
        [MaxLength(256)]
        [Category("User Info")]
        [Display(GroupName = "General Info", Order = 0, Name = "First Name", Description = "Fill in users first name")]
        public string FirstName { get; set; }
        [MaxLength(256)]
        [Category("User Info")]
        [Display(GroupName = "General Info", Order = 2, Name = "Last Name", Description = "Fill in users surname")]
        public string LastName { get; set; }
        [Category("User Info")]
        [MaxLength(2)]
        [StringLength(2, MinimumLength = 0)]
        [Display(GroupName = "General Info", Order = 1, Name = "Middle Initial", Description = "Fill in users first letter of middle name if it exists")]
        public string MiddleInitial { get; set; }
        [MaxLength(256)]
        [Category("User Info")]
        [Display(GroupName = "Login Info", Order = 1, Name = "User name", Description = "Fill in users username to use at login")]
        public string Username { get; set; }
        [MaxLength(256)]
        [Category("User Info")]
        [StringLength(16, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(GroupName = "Login Info", Order = 1, Name = "Password", Description = "Set a new password")]
        public string Password { get; set; }
        [MaxLength(256)]
        [NotMapped]
        [Category("User Info")]
        [Display(GroupName = "Login Info", Order = 1, Name = "Confirm Password", Description = "Confirm a new password")]
        public string Confirm { get; set; }

    }
}
