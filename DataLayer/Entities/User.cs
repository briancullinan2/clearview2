using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("User")]
    public class User : Entity<User>
    {
        [Key]
        [Required] // EF Core needs this for non-nullable PK
        [MaxLength(256)]
        [Category("User Info")]
        [Display(Name = "Globally Unique ID", Description = "Server assigned GUID for synchronization tracking")]
        public string Guid { get; private set; } = System.Guid.NewGuid().ToString();

        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(256)]
        [Category("User Info")]
        [Display(GroupName = "General Info", Order = 0, Name = "First Name", Description = "Fill in user's first name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(256)]
        [Category("User Info")]
        [Display(GroupName = "General Info", Order = 2, Name = "Last Name", Description = "Fill in user's surname")]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(2)]
        [StringLength(2, MinimumLength = 0)]
        [Category("User Info")]
        [Display(GroupName = "General Info", Order = 1, Name = "Middle Initial", Description = "Fill in user's first letter of middle name if it exists")]
        public string MiddleInitial { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(256)]
        [Category("User Info")]
        [Display(GroupName = "Login Info", Order = 0, Name = "User name", Description = "Fill in user's username to use at login")]
        // If using EF Core 9+, you can use [Index(IsUnique = true)] on the class level
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(256)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [DataType(DataType.Password)]
        [Category("User Info")]
        [Display(GroupName = "Login Info", Order = 1, Name = "Password", Description = "Set a new password")]
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Category("User Info")]
        [Display(GroupName = "Login Info", Order = 2, Name = "Confirm Password", Description = "Confirm a new password")]
        public string Confirm { get; set; }
    }
}