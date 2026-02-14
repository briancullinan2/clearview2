using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("Permission")]
    public class Permission : Entity<Permission>
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActionable { get; set; }
    }
}
