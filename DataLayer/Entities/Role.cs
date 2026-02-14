using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("Role")]
    public class Role : Entity<Role>
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
    }
}
