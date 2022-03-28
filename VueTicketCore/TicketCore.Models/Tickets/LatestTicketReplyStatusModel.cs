using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace TicketCore.Models.Tickets
{
    [Table("LatestTicketReplyStatus")]
    public class LatestTicketReplyStatusModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TicketReplyLatestId { get; set; }
        public long? TicketId { get; set; }
        public long? TicketReplyId { get; set; }
        public long? RepliedUserId { get; set; }
        public string StatusInfo { get; set; }
    }
}