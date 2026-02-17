using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [PrimaryKey(nameof(Name), nameof(Guid), nameof(Role))]
    [Table("Setting")]
    public class Setting : Entity<Setting>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Role { get; set; }
        public string Guid { get; set; }
    }
}
