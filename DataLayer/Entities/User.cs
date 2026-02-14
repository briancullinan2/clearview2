using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("User")]
    public class User : Entity<User>
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }

    }
}
