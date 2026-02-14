using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.DataLayer.Entities
{
    [Table("Message")]
    public class Message : Entity<Message>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsActive { get; set; }
        public int MessageType { get; set; }
        public int MessageId { get; set; }
        public string Source { get; set; }
    }
}
