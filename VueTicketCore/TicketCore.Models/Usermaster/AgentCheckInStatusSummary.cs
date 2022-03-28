using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Usermaster
{
    [Table("AgentCheckInStatusSummary")]
    public class AgentCheckInStatusSummary
    {
        [Key]
        public int AgentStatusId { get; set; }
        public bool AgentStatus { get; set; }
        public long UserId { get; set; }
    }
}