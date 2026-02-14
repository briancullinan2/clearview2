using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("Role")]
    public class Role : Entity<Role>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
